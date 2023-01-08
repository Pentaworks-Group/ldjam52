using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class DevelopmentStage
    {
        public int StageId { get; set; }
        public String Name { get; set; }
        //Defines how many Values gets visible in one analyze run
        public int ValueVisibleCount { get; set; }
        public int UpgradeCost { get; set; }
        //How Much one Analytics Run costs in this stage
        public int AnalyticsCost { get; set; }
        //How Much one Analytics Run costs (Plants) in this stage
        public int AnalyticsPlantCost { get; set; }
    }
}
