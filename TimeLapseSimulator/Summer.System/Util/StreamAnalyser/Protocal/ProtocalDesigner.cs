using System.Collections.Generic;
using System.Linq;
using Summer.System.IO;
using Summer.System.Util.StreamAnalyser.Function;

namespace Summer.System.Util.StreamAnalyser.Protocal
{
    /// <summary>
    /// 协议定义类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-7-14
    /// </remark>
    public class ProtocalDesigner
    {
        /// <summary>
        /// 协议列表
        /// </summary>
        public List<ProtocalTermFrame> ProtocalTermList;

        /// <summary>
        /// 表达式列表
        /// </summary>
        public List<ExpressionFunc> ExpressionFuncList;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProtocalDesigner(string dir, string filename):this()
        {
         List<string> filenameList = new List<string> ( );
            Load(dir, filename, ProtocalTermList, ExpressionFuncList, filenameList);
            Intergation();
        }

        protected void Load(string dir, string filename, List<ProtocalTermFrame> protocalTermList, List<ExpressionFunc> expressionFuncList, List<string> filenameList)
        {
            if (filenameList.Contains(filename))
            {
                return;
            }
            filenameList.Add(filename);
			//打开一个xml文件
            XmlFileHelper xmlFileHelper = XmlFileHelper.CreateFromFile(dir + filename);
            if (null == xmlFileHelper)
            {
                return;
            }
            XmlVisitor xmlRoot = xmlFileHelper.GetRoot();

			//针对一个文件，首先找到其alias标签
            List<XmlVisitor> xmlAliasList = xmlRoot.FilterChildren("alias").ToList();

            foreach (XmlVisitor xmlChild in xmlRoot.FilterChildren())
            {//对每一个节点开始解析
                switch (xmlChild.Name)
                {
	                case "import":
		                Load(dir, xmlChild.GetAttribute("src"), protocalTermList, expressionFuncList, filenameList);
		                break;
	                case "frame":
		                foreach (ProtocalTerm pt in ProtocalTerm.Create(xmlChild, xmlAliasList))
		                {
			                if ((pt is ProtocalTermFrame) && protocalTermList.Find(r => r.Name == pt.Name) == null)
			                {//如果是Frame类型则，添加
				                protocalTermList.Add(pt as ProtocalTermFrame);
			                }
		                }
		                break;
	                case "func":
		                {
			                ExpressionFunc ef = ExpressionFunc.Create(xmlChild);
			                if ((ef != null) && expressionFuncList.Find(r => r.Name == ef.Name) == null)
			                {
				                expressionFuncList.Add(ef);
			                }
		                }
		                break;
                }
            }  
        }

        protected void Intergation()
        {
            foreach (ProtocalTermFrame ptf in ProtocalTermList)
            {
                ptf.Intergation(this);
            }
        }

	    public ProtocalDesigner()
	    {
			ProtocalTermList = new List<ProtocalTermFrame> ( );
			ExpressionFuncList = new List<ExpressionFunc> ( );
	    }

	

    }
}
