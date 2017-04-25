using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Summer.System.IO
{
	/// <summary>
	/// 数据格式枚举
	/// </summary>
	public enum DataFormatType
	{
		TwoDigitsAfterDecimal,      //"1.20"
		RMBWithComma,               //"¥20,000"
		ScentificNum,               //"3.15E+00"
		Percent,                    //"99.33%"
		PhoneNum,                   //"021-65881234"
		ChineseCapitalizedCharacter,//壹贰叁
		ChineseDate,                //yyyy年m月d日
		EnglishDate                 //"m/d/yy h:mm"
	} ;

	/// <summary>
	/// 定义文件格式异常
	/// </summary>
	public class FileFormatException : Exception
	{
		public FileFormatException ( string message )
		{
			throw new Exception ( "文件格式错误：" + message );
		}
	}

	/// <summary>
	/// 写入Excel文件类
	/// 使用方法：先实例化ExcelFileWriter类，再调用方法编写Excel内容，最后调用Save2File方法保存为Excel文件
	/// </summary>
	/// <remarks>
	/// 公司：CASCO
	/// 作者：袁松
	/// 创建日期：2013-5-16
	/// </remarks>
	public class ExcelWriterHelper
	{
		private HSSFWorkbook mWorkbook;
		private List<ISheet> msheets;

		#region 构造函数
		/// <summary>
		/// 构造函数1，可动态构造多个sheet
		/// </summary>
		/// <param name="sheetNames">需要创建的每个sheet名称组成的数组</param>
		public ExcelWriterHelper ( params string[ ] sheetNames )
		{
			mWorkbook = new HSSFWorkbook ( );
			msheets = new List<ISheet> ( );
			foreach ( string sheetName in sheetNames )
			{
				msheets.Add ( mWorkbook.CreateSheet ( sheetName ) );
			}
		}

		/// <summary>
		/// 构造函数2，根据模板创建
		/// </summary>
		/// <param name="templePath">模板路径</param>
		public ExcelWriterHelper ( string templePath )
		{
			if ( string.IsNullOrEmpty ( templePath ) || !File.Exists ( templePath ) )
			{
				throw new FileNotFoundException ( templePath );
			}
			if ( Path.GetExtension ( templePath ) == ".xls" || Path.GetExtension ( templePath ) == ".xlsx" )
			{
				using ( FileStream file = new FileStream ( templePath, FileMode.Open, FileAccess.Read ) )
				{
					mWorkbook = new HSSFWorkbook ( file );
					msheets = new List<ISheet> ( );
					for ( int i = 0; i < mWorkbook.NumberOfSheets; i++ )
					{
						msheets.Add ( mWorkbook.GetSheetAt ( i ) );
					}
				}
			} else
			{
				throw new FileFormatException ( templePath );
			}

		}
		#endregion

		#region 页眉、页脚、文档信息
		/// <summary>
		/// 创建页眉,页眉左侧默认为页编号
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="centerText">页眉中间文本</param>
		/// <param name="rightText">页面右侧文本</param>
		public void CreateHeader ( string sheetName, string centerText, string rightText )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				sheet.Header.Left = HSSFHeader.Page;//页号
				sheet.Header.Center = centerText;
				sheet.Header.Right = rightText;
			}
		}

		/// <summary>
		/// 创建页脚
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="leftText">页脚左侧文本</param>
		/// <param name="centerText">页脚中间文本</param>
		/// <param name="rightText">页脚右侧文本</param>
		public void CreateFooter ( string sheetName, string leftText, string centerText, string rightText )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				sheet.Footer.Left = leftText;
				sheet.Footer.Center = centerText;
				sheet.Footer.Right = rightText;
			}

		}

		/// <summary>
		/// 创建文档概要信息
		/// </summary>
		/// <param name="companyName">公司名称</param>
		/// <param name="subjectName">文档主题</param>
		public void CreateDocumentSummary ( string companyName, string subjectName )
		{
			DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation ( );
			dsi.Company = companyName;
			mWorkbook.DocumentSummaryInformation = dsi;

			SummaryInformation si = PropertySetFactory.CreateSummaryInformation ( );
			si.Subject = subjectName;
			mWorkbook.SummaryInformation = si;
		}
		#endregion

		#region 设置列宽和行高
		/// <summary>
		/// 设置指定列的宽度
		/// </summary>
		/// <param name="sheetName">指定sheet的名称</param>
		/// <param name="columnIndex">列号</param>
		/// <param name="width">宽度值，以1/256表示一个字符的宽度，例如：需设置50个字符宽度，则应输入50*256</param>
		public void SetColumnWidth ( string sheetName, int columnIndex, int width )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				sheet.SetColumnWidth ( columnIndex, width );

			}

		}


		/// <summary>
		/// 设置指定行的高度
		/// </summary>
		/// <param name="sheetName">指定sheet的名称</param>
		/// <param name="rowIndex">行号</param>
		/// <param name="height">高度值，以1/20表示一个单位，例如：需设置10个单位的高度，则应输入10*20</param>
		public void SetRowHeight ( string sheetName, int rowIndex, int height )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				IRow row = GetRowFromSheet ( sheet, rowIndex );
				row.Height = height > short.MaxValue ? short.MaxValue : (short) height;
			}
		}
		#endregion

		#region 单元格赋值、为单元格设置注释
		/// <summary>
		/// 给指定单元格赋值(string)
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="rowIndex">行号</param>
		/// <param name="columnIndex">列号</param>
		/// <param name="value">实际值,string类型</param>
		/// <param name="isShrinkToFit">是否将单元格内容收缩到最小，该参数可选，默认为false</param>
		/// <param name="useNewLine">是否使用换行，用户可自行换行，比如“我来自CASCO”,若传入文本为“我\n来自CASCO”，则在单元格内会自动打印两行</param>
		public void SetCellValue ( string sheetName, int rowIndex, int columnIndex, string value, bool isShrinkToFit = false, bool useNewLine = true )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				IRow row = GetRowFromSheet ( sheet, rowIndex );
				ICell cell = GetCellFromRow ( row, columnIndex );
				cell.SetCellValue ( value );
				ICellStyle cellStyle = mWorkbook.CreateCellStyle ( );
				cellStyle.ShrinkToFit = isShrinkToFit;
				cellStyle.WrapText = useNewLine;
				cell.CellStyle = cellStyle;
			}
		}

		/// <summary>
		/// 给指定单元格赋值(DateTime)
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="rowIndex">行号</param>
		/// <param name="columnIndex">列号</param>
		/// <param name="time">实际值,DateTime类型</param>
		/// <param name="isShrinkToFit">是否将单元格内容收缩到最小，该参数可选，默认为false</param>
		/// <param name="formatType">设置日期格式，可选择DataFormatType.EnglishDate或DataFormatType.ChineseDate</param>
		public void SetCellValue ( string sheetName, int rowIndex, int columnIndex, DateTime time, bool isShrinkToFit = false, DataFormatType formatType = DataFormatType.EnglishDate )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				IRow row = GetRowFromSheet ( sheet, rowIndex );
				ICell cell = GetCellFromRow ( row, columnIndex );
				cell.SetCellValue ( time );
				ICellStyle cellStyle = mWorkbook.CreateCellStyle ( );
				cellStyle.DataFormat = GetDataFormat ( formatType );
				cellStyle.ShrinkToFit = isShrinkToFit;
				cell.CellStyle = cellStyle;
			}
		}

		/// <summary>
		/// 给指定单元格赋值(Bool)
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="rowIndex">行号</param>
		/// <param name="columnIndex">列号</param>
		/// <param name="value">实际值,Bool类型</param>
		/// <param name="isShrinkToFit">是否将单元格内容收缩到最小，该参数可选，默认为false</param>
		public void SetCellValue ( string sheetName, int rowIndex, int columnIndex, bool value, bool isShrinkToFit = false )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				IRow row = GetRowFromSheet ( sheet, rowIndex );
				ICell cell = GetCellFromRow ( row, columnIndex );
				cell.SetCellValue ( value );
				ICellStyle cellStyle = mWorkbook.CreateCellStyle ( );
				cellStyle.ShrinkToFit = isShrinkToFit;
				cell.CellStyle = cellStyle;
			}
		}
		public void SetCellValueNoStyle ( string sheetName, int rowIndex, int columnIndex, string value )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				IRow row = GetRowFromSheet ( sheet, rowIndex );
				ICell cell = GetCellFromRow ( row, columnIndex );
				cell.SetCellValue ( value );
			}
		}

		/// <summary>
		/// 给指定单元格赋值(Double)
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="rowIndex">行号</param>
		/// <param name="columnIndex">列号</param>
		/// <param name="value">实际值,Double类型</param>
		/// <param name="isShrinkToFit">是否将单元格内容收缩到最小，该参数可选，默认为false</param>
		/// <param name="formatType">设置数据格式，可选择TwoDigitsAfterDecimal,RMBWithComma,ScentificNum,Percent,phoneNum,ChineseCapitalizedCharacter</param>
		public void SetCellValue ( string sheetName, int rowIndex, int columnIndex, double value, bool isShrinkToFit = false, DataFormatType formatType = DataFormatType.TwoDigitsAfterDecimal )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				IRow row = GetRowFromSheet ( sheet, rowIndex );
				ICell cell = GetCellFromRow ( row, columnIndex );
				cell.SetCellValue ( value );
				ICellStyle cellStyle = mWorkbook.CreateCellStyle ( );
				cellStyle.DataFormat = GetDataFormat ( formatType );
				cellStyle.ShrinkToFit = isShrinkToFit;
				cell.CellStyle = cellStyle;
			}
		}

		/// <summary>
		/// 为指定单元格设置注释
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="rowIndex">单元格所在行号</param>
		/// <param name="colIndex">单元格所在列号</param>
		/// <param name="commentText">注释</param>
		/// <param name="author">作者</param>
		public void SetCellComment ( string sheetName, int rowIndex, int colIndex, string commentText, string author )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				IDrawing patr = sheet.CreateDrawingPatriarch ( );
				HSSFComment comment = (HSSFComment) patr.CreateCellComment ( new HSSFClientAnchor ( 0, 0, 0, 0, 4, 8, 6, 11 ) );
				//修改注释背景色
				comment.SetFillColor ( 204, 236, 255 );
				HSSFRichTextString str = new HSSFRichTextString ( commentText );
				//为注释设置字体格式
				IFont font = mWorkbook.CreateFont ( );
				font.FontName = ( "Arial" );
				font.FontHeightInPoints = 10;
				font.Boldweight = (short) FontBoldWeight.BOLD;
				font.Color = HSSFColor.RED.index;
				str.ApplyFont ( font );

				comment.String = str;
				comment.Visible = true; //默认情况下注释隐藏，此处定义注释可见
				comment.Author = author;
				comment.Row = rowIndex;
				comment.Column = colIndex;
			}

		}

		#endregion
		/// <summary>
		/// 取得Sheet的简单方式
		/// 作者：戴唯艺
		/// 日期：2013-08-27
		/// </summary>
		private ISheet GetSheet ( string sheetName )
		{
			return msheets.Find ( r => r.SheetName == sheetName );
		}


		#region 样式

		/// <summary>
		/// 设置某Sheet所有单元格样式，请先编写单元格，再应用此函数
		/// 注意：该函数使用反射机制，property请确保输入正确
		/// 作者：戴唯艺
		/// 日期：2013-08-27
		/// </summary>
		private void SetCellStyle ( string sheetName, string property, object value )
		{
			ISheet sheet = GetSheet ( sheetName );
			var rowEnum = sheet.GetRowEnumerator ( );
			while ( rowEnum.MoveNext ( ) )
			{
				IRow row = rowEnum.Current as IRow;
				var cellEnum = row.GetEnumerator ( );
				while ( cellEnum.MoveNext ( ) )
				{
					ICell cell = cellEnum.Current as ICell;


					var type = cell.CellStyle.GetType ( );
					var info = type.GetProperty ( property );
					info.SetValue ( cell.CellStyle, value, null );
				}
			}
		}
		/// <summary>
		/// 设置某Sheet所有单元格竖直对齐方式，请先编写单元格，再应用此函数
		/// 作者：戴唯艺
		/// 日期：2013-08-27
		/// </summary>
		public void SetCellVerticalSytle ( string sheetName, VerticalAlignment va )
		{
			SetCellStyle ( sheetName, "VerticalAlignment", va );
		}

		/// <summary>
		/// 设置指定单元格边框样式
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="rowIndex">单元格行号</param>
		/// <param name="colIndex">单元格列号</param>
		public void SetBorderStyle ( string sheetName, int rowIndex, int colIndex )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				IRow row = GetRowFromSheet ( sheet, rowIndex );
				ICell cell = GetCellFromRow ( row, colIndex );
				ICellStyle style = mWorkbook.CreateCellStyle ( );
				style.BorderBottom = BorderStyle.THIN;
				style.BottomBorderColor = HSSFColor.BLACK.index;
				style.BorderLeft = BorderStyle.DASH_DOT_DOT;
				style.LeftBorderColor = HSSFColor.GREEN.index;
				style.BorderRight = BorderStyle.HAIR;
				style.RightBorderColor = HSSFColor.BLUE.index;
				style.BorderTop = BorderStyle.MEDIUM_DASHED;
				style.TopBorderColor = HSSFColor.ORANGE.index;

				style.BorderDiagonal = BorderDiagonal.FORWARD;
				style.BorderDiagonalColor = HSSFColor.GOLD.index;
				style.BorderDiagonalLineStyle = BorderStyle.MEDIUM;
				cell.CellStyle = style;
			}

		}
		#endregion

		#region 创建带下拉列表框的单元格、将指定单元格的内容旋转指定角度
		/// <summary>
		/// 创建带下拉列表框的单元格
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="itemList">下拉列表项列表</param>
		/// <param name="colIndex">单元格所在列</param>
		/// <param name="fromRowIndex">下拉列表覆盖区域起始行号</param>
		/// <param name="toRowIndex">下拉列表覆盖区域终止行号</param>
		public void CreateDropDownListCell ( string sheetName, IEnumerable<string> itemList, int colIndex, int fromRowIndex, int toRowIndex )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				CellRangeAddressList rangeList = new CellRangeAddressList ( );
				//添加数据验证至指定列，从fromRowIndex到toRowIndex结束
				rangeList.AddCellRangeAddress ( new CellRangeAddress ( fromRowIndex, toRowIndex, colIndex, colIndex ) );
				DVConstraint dvconstraint = DVConstraint.CreateExplicitListConstraint ( itemList.ToArray<string> ( ) );
				HSSFDataValidation dataValidation = new
						HSSFDataValidation ( rangeList, dvconstraint );
				//添加数据验证至指定sheet
				( (HSSFSheet) sheet ).AddValidationData ( dataValidation );
			}

		}

		/// <summary>
		/// 将Excel中的文本旋转指定的角度
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="rowIndex">单元格所在行号</param>
		/// <param name="colIndex">单元格所在列号</param>
		/// <param name="rotation">旋转角度，取值为-90～90，例如如需将文本顺时针旋转60度，该值应赋-60</param>
		/// <param name="value">单元格内容</param>
		public void RotateTextInExcel ( string sheetName, int rowIndex, int colIndex, int rotation, string value )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				IRow row = GetRowFromSheet ( sheet, rowIndex );
				ICell cell = GetCellFromRow ( row, colIndex );
				cell.SetCellValue ( value );
				ICellStyle style = mWorkbook.CreateCellStyle ( );
				style.Rotation = (short) rotation;
				cell.CellStyle = style;
			}
		}
		#endregion

		#region 行内单元格拷贝、行间拷贝
		/// <summary>
		/// 将指定行的某个单元格拷贝到目标单元格
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="rowIndex">指定行号</param>
		/// <param name="fromCol">被拷贝单元格列号</param>
		/// <param name="toCol">目标单元格列号</param>
		public void CopyCell ( string sheetName, int rowIndex, int fromCol, int toCol )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				IRow row = GetRowFromSheet ( sheet, rowIndex );
				ICell cell = GetCellFromRow ( row, fromCol );
				cell.CopyCellTo ( toCol );
			}
		}

		/// <summary>
		/// 将指定行拷贝到目标行，原目标行自动下移
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="fromRow">被拷贝行号</param>
		/// <param name="toRow">目标行号</param>
		public void CopyRow ( string sheetName, int fromRow, int toRow )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				sheet.CopyRow ( fromRow, toRow );
			}
		}
		#endregion

		#region 合并单元格、隐藏指定行、隐藏指定列
		/// <summary>
		/// 合并单元格
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="firstRow">区域第一行</param>
		/// <param name="lastRow">区域最后一行</param>
		/// <param name="firstCol">区域第一列</param>
		/// <param name="lastCol">区域最后一列</param>
		/// <param name="isSetEnclosedBorder">是否为合并区域设置边框，默认true</param>
		public void MergeCells ( string sheetName, int firstRow, int lastRow, int firstCol, int lastCol, bool isSetEnclosedBorder = true )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				CellRangeAddress region = new CellRangeAddress ( firstRow, lastRow, firstCol, lastCol );
				sheet.AddMergedRegion ( region );
				//为合并区域设置边框
				if ( isSetEnclosedBorder )
				{
					( (HSSFSheet) sheet ).SetEnclosedBorderOfRegion ( region, BorderStyle.DOTTED, NPOI.HSSF.Util.HSSFColor.RED.index );
				}
			}
		}

		/// <summary>
		/// 隐藏指定行
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="rowIndex">指定行号</param>
		public void HideRow ( string sheetName, int rowIndex )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				IRow row = GetRowFromSheet ( sheet, rowIndex );
				row.ZeroHeight = true;
			}
		}

		/// <summary>
		/// 隐藏指定列
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="colIndex">指定列号</param>
		public void HideColumn ( string sheetName, int colIndex )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				sheet.SetColumnHidden ( colIndex, true );
			}
		}
		#endregion

		#region 从DataTable导入Excel
		/// <summary>
		/// 将DataTable插入指定位置
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="dt">目标Datatable</param>
		/// <param name="fromRowIndex">起始行号</param>
		/// <param name="fromColIndex">起始列号</param>
		public void InsertValueFromDataTable ( string sheetName, DataTable dt, int fromRowIndex, int fromColIndex )
		{
			for ( int i = 0; i < dt.Rows.Count; i++ )
			{
				for ( int j = 0; j < dt.Columns.Count; j++ )
				{
					SetCellValue ( sheetName, i + fromRowIndex, j + fromColIndex, dt.Rows[ i ][ j ].ToString ( ) );
				}
			}
		}
		#endregion

		#region 按行分组、按列分组、冻结指定区域、自动筛选
		/// <summary>
		/// 对行进行分组
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="fromRowIndex">起始行</param>
		/// <param name="toRowIndex">结束行</param>
		public void GroupRows ( string sheetName, int fromRowIndex, int toRowIndex )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				sheet.GroupRow ( fromRowIndex, toRowIndex );
			}

		}

		/// <summary>
		/// 对列进行分组
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="fromColmnIndex">起始列</param>
		/// <param name="toColumnIndex">结束列</param>
		public void GroupColumns ( string sheetName, int fromColmnIndex, int toColumnIndex )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				sheet.GroupColumn ( fromColmnIndex, toColumnIndex );
			}
		}

		/// <summary>
		/// 冻结指定区域
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="colSplit">列分隔线的水平位置</param>
		/// <param name="rowSplit">行分隔线的垂直位置</param>
		public void FreezePane ( string sheetName, int colSplit, int rowSplit )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				sheet.CreateFreezePane ( colSplit, rowSplit );
			}
		}

		/// <summary>
		/// 设置自动筛选
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="firstRow">第一行的行号</param>
		/// <param name="lastRow">最后一行的行号</param>
		/// <param name="firstCol">第一列的列号</param>
		/// <param name="lastCol">最后一列的列号</param>
		public void EnableAutoFilter ( string sheetName, int firstRow, int lastRow, int firstCol, int lastCol )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				sheet.SetAutoFilter ( new CellRangeAddress ( firstRow, lastRow, firstCol, lastCol ) );
			}
		}
		#endregion

		#region 插入图片

		/// <summary>
		/// 插入图片至指定Sheet的指定区域
		/// </summary>
		/// <param name="sheetName">sheet名称</param>
		/// <param name="path">图片路径</param>
		/// <param name="dx1">第一个单元格的x坐标</param>
		/// <param name="dy1">第一个单元格的y坐标</param>
		/// <param name="dx2">第二个单元格的x坐标</param>
		/// <param name="dy2">第二个单元格的y坐标</param>
		/// <param name="col1">第一个单元格的列号</param>
		/// <param name="row1">第一个单元格的行号</param>
		/// <param name="col2">第二个单元格的列号</param>
		/// <param name="row2">第二个单元格的行号</param>
		public void InsertPicturesToExcel ( string sheetName, string path, int dx1, int dy1, int dx2, int dy2, int col1, int row1, int col2, int row2 )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				HSSFPatriarch patriarch = (HSSFPatriarch) sheet.CreateDrawingPatriarch ( );
				//创建锚
				HSSFClientAnchor anchor;
				anchor = new HSSFClientAnchor ( dx1, dy1, dx2, dy2, col1, row1, col2, row2 );
				anchor.AnchorType = 2;
				//加载图片并获取图片在workBook中的索引
				HSSFPicture picture = (HSSFPicture) patriarch.CreatePicture ( anchor, LoadImage ( path, mWorkbook ) );
				//picture.Resize();   //Note: Resize will reset client anchor you set.
				picture.LineStyle = LineStyle.DashDotGel;
			}

		}
		/// <summary>
		/// 加载图片并获取图片在workBook中的索引
		/// </summary>
		/// <param name="path">图片路径</param>
		/// <param name="workBook">workBook</param>
		/// <returns>图片在workBook中的索引</returns>
		public static int LoadImage ( string path, HSSFWorkbook workBook )
		{
			using ( FileStream file = new FileStream ( path, FileMode.Open, FileAccess.Read ) )
			{
				byte[ ] buffer = new byte[ file.Length ];
				file.Read ( buffer, 0, (int) file.Length );
				return workBook.AddPicture ( buffer, PictureType.JPEG );
			}
		}
		#endregion

		#region 打印
		/// <summary>
		/// 设置打印区域
		/// </summary>
		/// <param name="sheetName">需打印sheet名称</param>
		/// <param name="startCol">开始列号</param>
		/// <param name="endCol">结束列号</param>
		/// <param name="startRow">开始行号</param>
		/// <param name="endRow">结束行号</param>
		public void SetPrintArea ( string sheetName, int startCol, int endCol, int startRow, int endRow )
		{
			int index = msheets.FindIndex ( r => r.SheetName == sheetName );
			if ( index >= 0 )
			{
				mWorkbook.SetPrintArea ( index, startCol, endCol, startRow, endRow );
			}

		}

		/// <summary>
		/// 设置打印参数
		/// </summary>
		/// <param name="sheetName">需设置的sheet名称</param>
		/// <param name="rightMargin">右侧边距</param>
		/// <param name="topMargin">上方边距</param>
		/// <param name="leftMargin">左侧边距</param>
		/// <param name="bottomMargin">下方边距</param>
		/// <param name="copies">打印份数</param>
		/// <param name="fitHeight">适应高度</param>
		/// <param name="fitWidth">适应宽度</param>
		/// <param name="noColor">非彩打</param>
		/// <param name="landScape">自动美化</param>
		/// <param name="fitToPage">适应页面</param>
		/// <param name="isPrintGridlines">是否打网格线</param>
		public void SetPrintSettings ( string sheetName, double rightMargin = 0.5, double topMargin = 0.6, double leftMargin = 0.4, double bottomMargin = 0.3,
									 short copies = 1, short fitHeight = 2, short fitWidth = 3, bool noColor = true, bool landScape = true,
									 bool fitToPage = true, bool isPrintGridlines = true )
		{
			ISheet sheet = msheets.Find ( r => r.SheetName == sheetName );
			if ( sheet != null )
			{
				sheet.SetMargin ( MarginType.RightMargin, rightMargin );
				sheet.SetMargin ( MarginType.TopMargin, topMargin );
				sheet.SetMargin ( MarginType.LeftMargin, leftMargin );
				sheet.SetMargin ( MarginType.BottomMargin, bottomMargin );

				sheet.PrintSetup.Copies = copies;
				sheet.PrintSetup.FitHeight = fitHeight;
				sheet.PrintSetup.FitWidth = fitWidth;
				sheet.PrintSetup.PaperSize = (short) PaperSize.A4;
				sheet.PrintSetup.NoColor = noColor;
				sheet.PrintSetup.Landscape = landScape;
				sheet.FitToPage = fitToPage;
				sheet.IsPrintGridlines = isPrintGridlines;
			}
		}

		#endregion

		#region 保存至Excel文件
		/// <summary>
		/// 保存到指定Excel文件
		/// </summary>
		/// <param name="path">Excel文件路径</param>
		public void Save2File ( string path )
		{

			using ( FileStream file = new FileStream ( path, FileMode.Create ) )
			{
				mWorkbook.Write ( file );
			}
		}
		#endregion

		#region 类私有通用方法
		/// <summary>
		/// 根据行号从指定sheet中获取Row
		/// </summary>
		/// <param name="sheet">指定sheet</param>
		/// <param name="rowIndex">行号</param>
		/// <returns>row实例</returns>
		private IRow GetRowFromSheet ( ISheet sheet, int rowIndex )
		{
			if ( sheet != null )
			{
				if ( sheet.GetRow ( rowIndex ) != null )
				{
					return sheet.GetRow ( rowIndex );
				} else
				{
					return sheet.CreateRow ( rowIndex );
				}
			}
			return null;
		}

		/// <summary>
		/// 根据列号从指定row中获取Cell
		/// </summary>
		/// <param name="row">指定Row</param>
		/// <param name="colIndex">列号</param>
		/// <returns>cell实例</returns>
		private ICell GetCellFromRow ( IRow row, int colIndex )
		{
			if ( row != null )
			{
				if ( row.GetCell ( colIndex ) != null )
				{
					return row.GetCell ( colIndex );
				} else
				{
					return row.CreateCell ( colIndex );
				}
			}
			return null;
		}

		/// <summary>
		/// 根据数据类型获取对应的格式
		/// </summary>
		/// <param name="dataType">数据类型</param>
		/// <returns>数据格式</returns>
		private short GetDataFormat ( DataFormatType dataType )
		{
			IDataFormat format = mWorkbook.CreateDataFormat ( );
			switch ( dataType )
			{
				case DataFormatType.ChineseCapitalizedCharacter:
					return format.GetFormat ( "[DbNum2][$-804]0 元" );
				case DataFormatType.ChineseDate:
					return format.GetFormat ( "yyyy年m月d日" );
				case DataFormatType.EnglishDate:
					return HSSFDataFormat.GetBuiltinFormat ( "m/d/yy h:mm" );
				case DataFormatType.Percent:
					return HSSFDataFormat.GetBuiltinFormat ( "0.00%" );
				case DataFormatType.PhoneNum:
					return format.GetFormat ( "000-0000000" );
				case DataFormatType.RMBWithComma:
					return format.GetFormat ( "¥#,##0" );
				case DataFormatType.ScentificNum:
					return HSSFDataFormat.GetBuiltinFormat ( "0.00E+00" );
				case DataFormatType.TwoDigitsAfterDecimal:
					return HSSFDataFormat.GetBuiltinFormat ( "0.00" );
				default:
					return short.MinValue;
			}
		}
		#endregion

		/// <summary>
		/// 取得Sheet的简单方式 
		/// 作者：戴唯艺
		/// 日期：2014-05-11
		/// </summary>
		public bool InsertBlankRow ( int sheet_index, int row_index, int count )
		{
			var sheet = mWorkbook.GetSheetAt ( sheet_index );
			if ( sheet == null )
				return false;

			sheet.ShiftRows ( row_index, sheet.LastRowNum, count );
			return true;
		}
	}
}
