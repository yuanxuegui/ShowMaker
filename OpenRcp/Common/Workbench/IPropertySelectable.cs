using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRcp
{
    public interface IPropertySelectable
    {
        /// <summary>
        /// 返回选择的属性对象
        /// </summary>
        /// <returns></returns>
        object SelectProperty();
    }
}
