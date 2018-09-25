using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DbOperationByDapper.AttributeHelper
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
    [ComVisible(true)]
    public class WhereAttribute : Attribute
    {
        /// <summary>
        /// 符号
        /// </summary>
        public string Symbol { get; set; }

        public WhereAttribute(string symbol)
        {
            this.Symbol = symbol;
        }

        public WhereAttribute()
        {
        }
    }
}

