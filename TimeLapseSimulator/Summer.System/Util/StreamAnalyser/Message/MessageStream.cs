using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Summer.System.Util.StreamAnalyser.Protocal;
using Summer.System.Util.StreamAnalyser.Value;
using Summer.System.Util.StreamAnalyser.Interpreter;
using Summer.System.IO;

namespace Summer.System.Util.StreamAnalyser.Message
{
    /// <summary>
    /// 二进制数据流协议解析类，包含一个MessageTerm的List用来存放根据二进制数据流解析的数据
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-7-14
    /// </remark>
    public class MessageStream
    {
        /// <summary>
        /// 协议帧定义
        /// </summary>
        ProtocalDesigner PD;

        /// <summary>
        /// 根据ProtocalTerm生成MessageSteam
        /// </summary>
        public MessageStream(ProtocalDesigner pd)
        {
            PD = pd;
        }

        /// <summary>
        /// 解析指定的二进制数据流，将解析内容保存在MessageTermList中，
        /// </summary>
        /// <param name="message">需要解析的二进制数据</param>
        /// <returns>当成功时返回true，否则返回false</returns>
        public bool Interpret(byte[] message, string name, out MessageTermSlot mt)
        {
            mt = null;
            ProtocalTermComplex ptc = null;
            if (PD != null) 
            {
                ptc = PD.ProtocalTermList.Find(r => r.Name == name);
            }
            if (null == ptc)
            {
                return false;
            }
            try
            {
                InterpreterPosition pos = new InterpreterPosition();
                mt = ptc.Interpret(message, ref pos, new InterpretOpion());
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 解析指定的二进制数据流，将解析内容保存在MessageTermList中并根据指定结束项停止解析
        /// </summary>
        /// <param name="message">需要解析的二进制数据</param>
        /// <returns>当成功时返回true，否则返回false</returns>
        public bool Interpret(byte[] message, string name, out MessageTermSlot mt, ref InterpreterPosition pos, string endtermname = "")
        {
            mt = null;
            ProtocalTermComplex ptc = FindProtocalTerm(name.Split('.')) as ProtocalTermComplex;
            if (null == ptc)
            {
                return false;
            }
            try
            {
                List<MessageTermSlot> mtList = new List<MessageTermSlot>();
                foreach (ProtocalTerm pt in ptc.PTList.Value)
                {
                    MessageTermSlot mt2 = pt.Interpret(message, ref pos, new InterpretOpion());
                    if (mt2 != null)
                    {
                        mtList.Add(mt2);
                    }
                    if (!string.IsNullOrEmpty(endtermname) && pt.Name == endtermname)
                    {
                        break;
                    }
                }
                mt = ProtocalTerm.CreateMessageTerm(ptc, new TermValue(""), mtList);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool Transfer(XmlVisitor xmlVisitor, out MessageTermSlot mt)
        {
            mt = null;
            string name = xmlVisitor.GetAttribute("name");
            ProtocalTermComplex ptc = null;
            if (PD != null)
            {
                ptc = PD.ProtocalTermList.Find(r => r.Name == name);
            }
            if (null == ptc)
            {
                return false;
            }
            try
            {
                mt = ptc.Transfer(xmlVisitor) ;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool CreateBlank(string name, out MessageTermSlot mt, bool ingoreoccurs = false)
        {
            mt = null;
            ProtocalTerm pt = FindProtocalTerm(name.Split('.'));
            if (null == pt)
            {
                return false;
            }
            try
            {
                mt = pt.CreateBlank(ingoreoccurs);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool CreateTerm(string name, TermValue value, out MessageTermSlot mt)
        {
            return CreateTerm(name, new MessageTerm(null, value), out mt);
        }

        public bool CreateTerm(string name, MessageTermSlot value, out MessageTermSlot mt)
        {
            List<MessageTermSlot> valuelist = new List<MessageTermSlot>();
            if (value != null)
            {
                valuelist.Add(value);
            }
            return CreateTerm(name, valuelist, out mt);
        }

        public bool CreateTerm(string name, List<MessageTermSlot> valuelist, out MessageTermSlot mt)
        {
            mt = null;
            ProtocalTerm pt = FindProtocalTerm(name.Split('.'));
            if (null == pt)
            {
                return false;
            }
            try
            {
                mt = pt.CreateTerm(valuelist, new InterpretOpion());
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool UpdateTerm(MessageTermSlot mtmsg, MessageTermSlot mtitem)
        {
            if (mtitem.PT != null)
            {
                string name = mtitem.PT.FullName;
                string[] namelist = name.Split('.');
                if (namelist.Count() > 0)
                {
                    string msgname = mtmsg.PT.FullName;
                    string[] msgnamelist = msgname.Split('.');
                    int index = 0;
                    for (; index < msgnamelist.Count(); index++)
                    {
                        if (index >= namelist.Count())
                        {
                            return false;
                        }
                        if (namelist[index] != msgnamelist[index])
                        {
                            return false;
                        }
                    }
                    MessageTermSlot mtitem2 = mtmsg;
                    for (int i = index; i < namelist.Count(); i++)
                    {
                        MessageTermComplex mtc = mtitem2 as MessageTermComplex;
                        if (mtc != null)
                        {
                            mtitem2 = mtc.MessageTermList.Find(r => r.Name == namelist[i]);
                        }
                        else
                        {
                            mtitem2 = null;
                            break;
                        }
                    }
                    if (mtitem2 != null)
                    {
                        return mtitem2.UpdateTerm(mtitem, this, new InterpretOpion());
                    }
                }
            }
            return false;
        }

        private ProtocalTerm FindProtocalTerm(string[] namelist)
        {
            ProtocalTerm pt = null;
            if ((PD != null) && (namelist.Count() > 0))
            {
                pt = PD.ProtocalTermList.Find(r => r.Name == namelist[0]);
                for (int i = 1; i < namelist.Count(); i++)
                {
                    ProtocalTermComplex ptc = pt as ProtocalTermComplex;
                    if (ptc != null)
                    {
                        pt = ptc.PTList.Value.Find(r => r.Name == namelist[i]);
                    }
                    else
                    {
                        pt = null;
                        break;
                    }
                }
            }
            return pt;
        }
    }
}
