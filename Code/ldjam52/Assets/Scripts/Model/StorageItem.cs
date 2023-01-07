using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class StorageItem
    {
        public Plant Plant { get; set; }
        public int StorageAmountPlants { get; set; }
        public int StorageAmountSeeds { get; set; }
    }
}
