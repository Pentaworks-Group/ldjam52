using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class FarmStorage
    {
        public int StorageSize { get; set; }
        public int MoneyBalance { get; set; }
        public List<StorageItem> StorageItems { get; set; }
    }
}
