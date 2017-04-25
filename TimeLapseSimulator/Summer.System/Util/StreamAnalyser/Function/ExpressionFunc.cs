using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Summer.System.IO;
using Summer.System.Util.StreamAnalyser.Value;

namespace Summer.System.Util.StreamAnalyser.Function
{
    /// <summary>
    /// 表达式函数基类，为虚基类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-7-14
    /// </remark>
    public abstract class ExpressionFunc
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        protected ExpressionFunc(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 计算虚函数，由子类实现
        /// </summary>
        /// <param name="tve"></param>
        /// <returns></returns>
        public abstract TermValue Calc(List<TermValue> paramlist);

        public abstract void InverseCalc(List<TermValue> paramlist, TermValue value);

        /// <summary>
        /// 根据XML片段生成表达式函数类，目前有算数型（加减乘除)和枚举映射函数
        /// </summary>
        /// <param name="xmlVisitor"></param>
        /// <returns></returns>
        public static ExpressionFunc Create(XmlVisitor xmlVisitor)
        {
            ExpressionFunc ef = null;
            string style = xmlVisitor.GetAttribute("style");
            string name = xmlVisitor.GetAttribute("name");
            switch (style)
            {
                case "arith":
                    {
                        switch (name)
                        {
                            case "add":
                                ef = new AddFunc(name);
                                break;
                            case "sub":
                                ef = new SubFunc(name);
                                break;
                            case "mul":
                                ef = new MulFunc(name);
                                break;
                            case "div":
                                ef = new DivFunc(name);
                                break;
                        }
                    }
                    break;
                case "enum":
                    {
                        Dictionary<string, TermValue> enumvalueList = new Dictionary<string, TermValue>();
                        string dftValue = string.Empty;
                        XmlVisitor dftXmlVisitor = xmlVisitor.FirstChild("default");
                        if (dftXmlVisitor != null)
                        {
                            dftValue = dftXmlVisitor.Value;
                        }
                        foreach (XmlVisitor xmlChild in xmlVisitor.FilterChildren("enum"))
                        {
                            string key = xmlChild.GetAttribute("value");
                            if(!string.IsNullOrEmpty(key))
                            {
                               TermValue value = new TermValue(xmlChild.Value);
                               enumvalueList[key] = value;
                            }
                        }
                        ef = new EnumFunc(name, enumvalueList, dftValue);
                    }
                    break;
                case "strfmt":
                    {
                        ef = new StringFormatFunc(name);
                    }
                    break;
            }
            return ef;
        }
    }
}
