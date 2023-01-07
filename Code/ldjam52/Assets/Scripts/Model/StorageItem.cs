using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Assets.Scripts.Model
{
    public class StorageItem
    {
        public Guid? PlantId
        {
            get;
            set;
        }


        private Plant plant;

        [JsonIgnore]
        public Plant Plant
        {
            get
            {
                if (plant == default)
                {
                    if (PlantId.HasValue && Assets.Scripts.Base.Core.Game.State.KnownPlants.TryGetValue(PlantId.Value, out Plant planti))
                    {
                        plant = planti;
                    }
                }
                return plant;
            }
            set
            {
                this.plant = value;
                this.PlantId = value?.ID;
            }
        }
        public int StorageAmountPlants { get; set; }
        public int StorageAmountSeeds { get; set; }
    }
}
