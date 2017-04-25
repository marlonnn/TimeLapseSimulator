using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Summer.System.Log;
using Summer.System.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Summer.System.IO
{
    /// <summary>
    /// Pdf操作类，将iTextSharp中提供的方法进行封装，实现基本的操作
    /// 作者：钟文
    /// 创建日期：2016-4-12
    /// </summary>
    public class PDFHelper
    {
        private Document document;
        private PdfWriter pdfWriter;
        private PdfContentByte canvas;

        public Document Document
        {
            get
            {
                return this.document;
            }
            set
            {
                this.document = value;
            }
        }

        public PdfWriter PdfWriter
        {
            get
            {
                return this.pdfWriter;
            }
            set
            {
                this.pdfWriter = value;
            }
        }

        public PdfContentByte Canvas
        {
            get
            {
                return this.canvas;
            }
            private set
            {
                this.canvas = value;
            }
        }


        /// <summary>
        /// 构造函数，创建document、pdfWriter、canvas对象
        /// </summary>
        /// <param name="filePath">pdf文档保存路径</param>
        /// <param name="pageSize">文档大小</param>
        /// <param name="marginLeft">文档内容与左边距离</param>
        /// <param name="marginRight">文档内容与右边距离</param>
        /// <param name="marginTop">文档内容与上边距离</param>
        /// <param name="marginBottom">文档内容与下边距离</param>
        public PDFHelper(string filePath, Rectangle pageSize, float marginLeft, float marginRight, float marginTop, float marginBottom)
        {
            try
            {
                document = new Document(pageSize, marginLeft, marginRight, marginTop, marginBottom);
                if (File.Exists(filePath))
                    File.Delete(filePath);
                pdfWriter = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.CreateNew));
                document.Open();
                canvas = pdfWriter.DirectContent;
            }
            catch
            {
                throw new Exception("此文件已被打开！");
            }
        }

        /// <summary>
        /// 获取最上层PdfContentByte
        /// </summary>
        /// <returns></returns>
        public PdfContentByte GetPdfContentByte()
        {
            return pdfWriter.DirectContent;
        }

        /// <summary>
        /// 获取最下层PdfContentByte
        /// </summary>
        /// <returns></returns>
        public PdfContentByte GetPdfContentByteUnder()
        {
            return pdfWriter.DirectContentUnder;
        }

        /// <summary>
        /// 使用PdfContentByte和直线构建矩形
        /// 这种方式是利用直线来构建，
        /// 4（x,y+height）<------------3(x+width,y+height)
        ///  |                          ^
        ///  |                          |
        ///  |                          |
        ///  |                          |
        ///  |                          |
        /// 1 (x,y)-------------------->2(x+width,y)
        /// </summary>
        /// <param name="pdfContentByte">PdfContentByte</param>
        /// <param name="offsetLeft">X轴坐标</param>
        /// <param name="offsetBottom">Y轴坐标</param>
        /// <param name="height">矩形高度</param>
        /// <param name="width">矩形宽度</param>
        public void DrawRectangle(PdfContentByte pdfContentByte, float offsetLeft, float offsetBottom, float height, float width)
        {
            pdfContentByte.MoveTo(offsetLeft, offsetBottom);
            pdfContentByte.LineTo(offsetLeft + width, offsetBottom);
            pdfContentByte.LineTo(offsetLeft + width, offsetBottom + height);
            pdfContentByte.LineTo(offsetLeft, offsetBottom + height);
            pdfContentByte.ClosePath();
            pdfContentByte.Stroke();
        }

        /// <summary>
        /// 使用PdfContentByte和Rectangle构建
        /// </summary>
        /// <param name="pdfContentByte"></param>
        /// <param name="rectangle"></param>
        public void DrawRectangle(PdfContentByte pdfContentByte, Rectangle rectangle)
        {
            DrawRectangle(pdfContentByte, rectangle.Left, rectangle.Bottom, rectangle.Right, rectangle.Top);
        }

        /// <summary>
        /// 绘制一条水平线
        /// </summary>
        /// <param name="pdfContentByte">PdfContentByte</param>
        /// <param name="offsetLeft">X轴坐标</param>
        /// <param name="offsetBottom">Y轴坐标</param>
        /// <param name="width">宽度</param>
        public void DrawHorizontalLine(PdfContentByte pdfContentByte, float offsetLeft, float offsetBottom, float width)
        {
            pdfContentByte.MoveTo(offsetLeft, offsetBottom);
            pdfContentByte.LineTo(offsetLeft + width, offsetBottom);
            pdfContentByte.Stroke();
        }

        /// <summary>
        /// 绘制一条竖直线
        /// </summary>
        /// <param name="pdfContentByte">PdfContentByte</param>
        /// <param name="offsetLeft"></param>
        /// <param name="offsetBottom"></param>
        /// <param name="height"></param>
        public void DrawVerticalLine(PdfContentByte pdfContentByte, float offsetLeft, float offsetBottom, float height)
        {
            pdfContentByte.MoveTo(offsetLeft, offsetBottom);
            pdfContentByte.LineTo(offsetLeft, offsetBottom + height);
            pdfContentByte.Stroke();
        }

        /// <summary>
        /// 任意绘制一条直线
        /// </summary>
        /// <param name="pdfContentByte">PdfContentByte</param>
        /// <param name="startOffsetLeft">起点X轴坐标</param>
        /// <param name="startOffsetBottom">起点Y轴坐标</param>
        /// <param name="stopOffsetLeft">终点X轴坐标</param>
        /// <param name="stopOffsetBottom">终点Y轴坐标</param>
        public void DrawLine(PdfContentByte pdfContentByte, int startOffsetLeft, int startOffsetBottom, int stopOffsetLeft, int stopOffsetBottom)
        {
            pdfContentByte.MoveTo(startOffsetLeft, startOffsetBottom);
            pdfContentByte.LineTo(stopOffsetLeft, stopOffsetBottom);
            pdfContentByte.Stroke();
        }

        /// <summary>
        /// 通过BaseFont类的实例计算字符的长度（pt）
        /// iText中默认的字体为Helvetica，字体大小12pt
        /// 1inch = 25.4mm = 72 user units ≈ 72pt
        /// </summary>
        /// <param name="baseFont">BaseFont类的实例</param>
        /// <param name="soureString">需要测试的字符串</param>
        /// <param name="fontSize">默认字体大小</param>
        /// <returns></returns>
        public float GetWidthPoint(BaseFont baseFont, string soureString, int fontSize = 12)
        {
            return baseFont.GetWidthPoint(soureString, fontSize);
        }

        /// <summary>
        /// 获取字符位于基准线之上的距离
        /// </summary>
        /// <param name="baseFont">BaseFont类的实例</param>
        /// <param name="soureString">需要测试的字符串</param>
        /// <param name="fontSize">默认字体大小</param>
        /// <returns></returns>
        public float GetAscentPoint(BaseFont baseFont, string soureString, int fontSize = 12)
        {
            return baseFont.GetAscentPoint(soureString, fontSize);
        }

        /// <summary>
        /// 获取字符位于基准线之下的距离
        /// </summary>
        /// <param name="baseFont">BaseFont类的实例</param>
        /// <param name="soureString">需要测试的字符串</param>
        /// <param name="fontSize">默认字体大小</param>
        /// <returns></returns>
        public float GetDescentPoint(BaseFont baseFont, string soureString, int fontSize = 12)
        {
            return baseFont.GetDescentPoint(soureString, fontSize);
        }

        /// <summary>
        /// 获取字符串高度
        /// 基准线之上距离 + 基准线之下的距离(负数)
        /// </summary>
        /// <param name="baseFont"></param>
        /// <param name="soureString"></param>
        /// <param name="fontSize"></param>
        /// <returns></returns>
        public float GetHeigthPoint(BaseFont baseFont, string soureString, int fontSize = 12)
        {
            return GetAscentPoint(baseFont, soureString) - GetDescentPoint(baseFont, soureString);
        }

        /// <summary>
        /// 定位字符串
        /// </summary>
        /// <param name="canvas">PdfContentByte</param>
        /// <param name="alignment">对齐方式</param>
        /// <param name="phrase">Phrase</param>
        /// <param name="offsetLeft">X轴开始或结束坐标 1.Element.ALIGN_LEFT:开始坐标；2.Element.ALIGN_RIGHT：结束坐标</param>
        /// <param name="offsetBottom">Y轴基准线坐标</param>
        /// <param name="rotation">旋转角</param>
        public void ShowTextAligned(PdfContentByte canvas, int alignment, Phrase phrase, float offsetLeft, float offsetBottom, float rotation)
        {
            ColumnText.ShowTextAligned(canvas, alignment, phrase, offsetLeft, offsetBottom, rotation);
        }

        /// <summary>
        /// 创建PdfPTable表格，使用可用WidthPercentage宽度百分比来创建
        /// 表格的宽度是可用空间的80%，例如A4纸的尺寸是：297mm×210mm，宽度为595pt
        /// 则实际宽度为：595-36*2 = 523pt(36是左右页边距的宽度)
        /// 可用宽度为： 523*80% = 418.4pt
        /// </summary>
        /// <param name="numColumns">表格列数</param>
        /// <param name="relativeWidths">表格相对宽度（例如：new int[] {2, 1, 1}，将表格分成2+1+1=4等份：第一列占有2等份，其它的都占有1等份）</param>
        /// <param name="tableWidth">表格宽度（pt）</param>
        /// <returns></returns>
        public PdfPTable CreateTable1(int numColumns, int[] relativeWidths, float tableWidth)
        {
            PdfPTable pdfTable = new PdfPTable(numColumns);
            pdfTable.WidthPercentage = tableWidth / 5.23f;
            pdfTable.SetWidths(relativeWidths);
            return pdfTable;
        }

        /// <summary>
        ///  创建PdfPTable表格，使用TotalWidth绝对宽度来创建
        /// </summary>
        /// <param name="numColumns">表格列数</param>
        /// <param name="relativeWidths">表格相对宽度（例如：new int[] {2, 1, 1}，将表格分成2+1+1=4等份：第一列占有2等份，其它的都占有1等份）</param>
        /// <param name="tableWidth">表格宽度（pt）</param>
        /// <returns></returns>
        public PdfPTable CreateTable2(int numColumns, float[] relativeWidths, float tableWidth)
        {
            PdfPTable pdfTable = new PdfPTable(numColumns);
            pdfTable.TotalWidth = tableWidth;
            pdfTable.LockedWidth = true;
            pdfTable.SetWidths(relativeWidths);
            return pdfTable;
        }

        /// <summary>
        /// 创建PdfPTable表格，使用Rectangle和realWidths实际宽度来创建
        /// </summary>
        /// <param name="numColumns">表格列数</param>
        /// <param name="realWidths">表格实际宽度（例如：new int[] {144, 72, 72}，将表格分成2+1+1=4等份：第一列占有144pt宽度，其它的都占有72pt宽度）</param>
        /// <param name="rectangle">Rectangle</param>
        /// <returns></returns>
        public PdfPTable CreateTable(int numColumns, float[] realWidths, Rectangle rectangle)
        {
            PdfPTable pdfTable = new PdfPTable(numColumns);
            pdfTable.SetWidthPercentage(realWidths, rectangle);
            return pdfTable;
        }

        /// <summary>
        /// 创建PdfPTable表格，使用realWidths实际宽度来创建
        /// </summary>
        /// <param name="numColumns">表格列数</param>
        /// <param name="realWidths">实际宽度，例如：new int[] {144, 72, 72}，将表格分成2+1+1=4等份：第一列占有144pt宽度，其它的都占有72pt宽度）</param>
        /// <returns></returns>
        public PdfPTable CreateTable(int numColumns, float[] realWidths)
        {
            PdfPTable pdfTable = new PdfPTable(numColumns);
            pdfTable.SetTotalWidth(realWidths);
            pdfTable.LockedWidth = true;
            return pdfTable;
        }

        /// <summary>
        /// 创建默认的PdfPCell
        /// </summary>
        /// <param name="element">PdfPTable/PdfPCell/Image/Phrase/</param>
        /// <returns></returns>
        public PdfPCell CreatePdfCell(IElement element)
        {
            PdfPCell cell = new PdfPCell();
            cell.AddElement(element);
            return cell;
        }

        /// <summary>
        /// 创建PdfPCell
        /// </summary>
        /// <param name="phrase"></param>
        /// <param name="fixedLeading"></param>
        /// <param name="multipliedLeading"></param>
        /// <returns></returns>
        public PdfPCell CreatePdfCell(Phrase phrase, float fixedLeading, float multipliedLeading)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.SetLeading(fixedLeading, multipliedLeading);
            return cell;
        }

        /// <summary>
        /// 创建PdfPCell
        /// </summary>
        /// <param name="phrase"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public PdfPCell CreatePdfCell(Phrase phrase, float padding)
        {
            PdfPCell cell = new PdfPCell();
            cell.AddElement(phrase);
            cell.Padding = padding;
            return cell;
        }

        /// <summary>
        /// 创建文档标题（带下划线）
        /// </summary>
        /// <param name="title">文档标题</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="spacingBefore">与前面控件距离</param>
        /// <param name="spacingAfter">与后面控件距离</param>
        /// <returns></returns>
        public bool CreatePdfTitle(string title, float fontSize, float spacingBefore, float spacingAfter)
        {
            try
            {
                Paragraph paragraph = new Paragraph();
                Phrase phrase = new Phrase(title, Fonts.BOLD(fontSize));
                paragraph.Add(phrase);
                paragraph.Alignment = Element.ALIGN_CENTER;
                LineSeparator line = new LineSeparator(1, 100, null, Element.ALIGN_CENTER, -15);
                paragraph.Add(line);
                paragraph.SpacingBefore = spacingBefore;
                paragraph.SpacingAfter = spacingAfter;
                document.Add(paragraph);
            }
            catch (Exception ex)
            {
                LogHelper.GetLogger<PDFHelper>().Error(ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 创建文档标题（带下划线）
        /// </summary>
        /// <param name="title">文档标题</param>
        /// <returns></returns>
        public bool CreatePdfTitle(string title)
        {
            try
            {
                Paragraph paragraph = new Paragraph();

                Phrase phrase = new Phrase(title, Fonts.BOLD());
                paragraph.Add(phrase);
                paragraph.Alignment = Rectangle.ALIGN_CENTER;

                LineSeparator line = new LineSeparator(1, 100, null, Element.ALIGN_CENTER, -15);
                paragraph.Add(line);
                paragraph.SpacingAfter = 10;
                document.Add(paragraph);

            }
            catch (Exception ex)
            {
                LogHelper.GetLogger<PDFHelper>().Error(ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 创建PdfPTable对象
        /// </summary>
        /// <param name="numColumns">列数</param>
        /// <param name="widthPercentage">表格相对宽度</param>
        /// <param name="spacingBefore">与前面控件间距</param>
        /// <param name="spacingAfter">与后面控件间距</param>
        /// <returns></returns>
        public PdfPTable CreatePdfTable(int numColumns, float widthPercentage, float spacingBefore, float spacingAfter)
        {
            PdfPTable table = new PdfPTable(numColumns);
            table.WidthPercentage = widthPercentage;
            table.SpacingBefore = spacingBefore;
            table.SpacingAfter = spacingAfter;
            return table;
        }

        /// <summary>
        /// 创建CreatePdfCell
        /// </summary>
        /// <param name="words">相关信息</param>
        /// <param name="alignment">对齐方式</param>
        /// <param name="colSpan">占用列数</param>
        /// <param name="fontSize">字体大小</param>
        /// <returns></returns>
        public PdfPCell CreatePdfCell(String words, int alignment, int colSpan, float fontSize)
        {
            PdfPCell cell = new PdfPCell(new Phrase(words, Fonts.BOLD(fontSize)));
            cell.HorizontalAlignment = alignment;
            cell.Colspan = colSpan;
            return cell;
        }

        /// <summary>
        /// 创建CreatePdfCell 关键字：相关信息
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="words">相关信息</param>
        /// <param name="alignment">对齐方式</param>
        /// <param name="colSpan">占用列数</param>
        /// <param name="padding">填充属性</param>
        /// <returns></returns>
        public PdfPCell CreatePdfCell(string key, string words, int alignment, int colSpan, int padding)
        {
            PdfPCell cell = new PdfPCell(new Phrase(string.Format("{0}: {1}", key, words)));
            cell.HorizontalAlignment = alignment;
            cell.Colspan = colSpan;
            cell.Padding = padding;
            return cell;
        }

        /// <summary>
        /// 创建CreatePdfCell 关键字：相关信息
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="words">相关信息</param>
        /// <param name="alignment">对齐方式</param>
        /// <param name="colSpan">占用列数</param>
        /// <param name="border">边界属性</param>
        /// <param name="padding">填充属性</param>
        /// <returns></returns>
        public PdfPCell CreatePdfCell(string key, string words, int alignment, int colSpan, int border, int padding)
        {
            PdfPCell cell = new PdfPCell(new Phrase(string.Format("{0}: {1}", key, words)));
            cell.HorizontalAlignment = alignment;
            cell.Colspan = colSpan;
            cell.Border = border;
            cell.Padding = padding;
            return cell;
        }

        /// <summary>
        /// 创建PdfPCell
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <returns></returns>
        public PdfPCell CreatePdfCell(string imagePath)
        {
            Image image = Image.GetInstance(imagePath);
            PdfPCell cell = new PdfPCell(image, true);
            return cell;
        }

        /// <summary>
        /// 创建Logo图标
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="scalePercent"></param>
        /// <param name="absoluteX"></param>
        /// <param name="offsetY"></param>
        public void CreateLogo(string imagePath, float scalePercent, float absoluteX, int offsetY)
        {
            canvas.SaveState();
            Image image = Image.GetInstance(imagePath);
            image.ScalePercent(scalePercent);
            image.SetAbsolutePosition(absoluteX, PageSize.A4.Height - image.Height - offsetY);
            canvas.AddImage(image);
            canvas.RestoreState();
        }

        /// <summary>
        /// 创建左上方Logo图标
        /// </summary>
        /// <param name="imagePath">左上方Logo路径</param>
        /// <param name="scalePercent">缩放比例</param>
        public void CreateLogoLeft(string imagePath, float scalePercent)
        {
            canvas.SaveState();
            Image image = Image.GetInstance(imagePath);
            image.ScalePercent(scalePercent);
            image.SetAbsolutePosition(36, PageSize.A4.Height - image.Height - 5);
            canvas.AddImage(image);
            canvas.RestoreState();
        }

        /// <summary>
        /// 创建右上方Logo图标
        /// </summary>
        /// <param name="imagePath">右上方Logo路径</param>
        /// <param name="scalePercent">缩放比例</param>
        public void CreateLogoRight(string imagePath, float scalePercent)
        {
            canvas.SaveState();
            Image image = Image.GetInstance(imagePath);
            image.ScalePercent(scalePercent);
            image.SetAbsolutePosition(PageSize.A4.Width - image.Width, PageSize.A4.Height - image.Height);
            canvas.AddImage(image);
            canvas.RestoreState();
        }

        /// <summary>
        /// 保存文档
        /// </summary>
        public void SaveToFile()
        {
            document.Close();
        }

        /// <summary>
        /// 保存文档到指定路径
        /// </summary>
        /// <param name="sourceFileName">源文件路径</param>
        /// <param name="destFileName">指定文件路径</param>
        public void SaveToFile(string sourceFileName, string destFileName)
        {
            document.Close();
            File.Copy(sourceFileName, destFileName, true);
        }
    }
}
