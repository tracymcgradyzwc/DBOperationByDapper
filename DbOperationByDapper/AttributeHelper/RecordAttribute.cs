using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DbOperationByDapper.AttributeHelper
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
    [ComVisible(true)]
    public class RecordAttribute : Attribute
    {
        private string recordType;      //记录类型：更新/创建
        private string author;          //作者
        private DateTime date;          //更新/创建日期
        private string memo;            //备注

        //构造函数，构造函数的参数在特性中也成为“位置参数”
        public RecordAttribute(string recordType, string author, string date)
        {
            //特性类的构造函数的参数有一些限制：必须为敞亮、Type类型，或者是常量数组
            //因此不能直接传递DateTime类型，只能传递String类型，然后在构造函数内进行一个强制类型转换
            this.recordType = recordType;
            this.author = author;
            this.date = Convert.ToDateTime(date);
        }

        //对于位置参数，通常只提供Get访问器
        public string RecordType { get { return recordType; } }
        public string Author { get { return author; } }
        public DateTime Date { get { return date; } }
        public string Memo { get { return memo; } }

        //构建一个属性，在特性中也叫“命名参数”
        //public string Memo
        //{
        //    get;
        //    set;
        //}
    }
}
