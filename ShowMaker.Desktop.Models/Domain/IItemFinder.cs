﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowMaker.Desktop.Models.Domain
{
    public interface IItemFinder<T,K>
    {
        T GetItemByKey(K key);
    }
}
