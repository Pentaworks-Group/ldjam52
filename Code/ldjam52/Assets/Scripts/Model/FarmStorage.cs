using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class FarmStorage
    {
        public Double StorageSize { get; set; }
        public Double MoneyBalance { get; set; }
        public List<StorageItem> StorageItems { get; set; }
    }
}
