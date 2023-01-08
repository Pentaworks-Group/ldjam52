using System;
using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    public class FarmStorage
    {
        public Int32 StorageSize { get; set; }
        public Int32 MoneyBalance { get; set; }
        public List<StorageItem> StorageItems { get; set; }
    }
}
