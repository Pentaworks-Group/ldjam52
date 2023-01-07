using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Model;

namespace Assets.Scripts.Constants
{
    public class GrowthStages
    {
        public static List<GrowthStage> stages = new List<GrowthStage>
        {
            new GrowthStage{
                ProgressStart = 0,
                ProgressEnd = 0.25,
                ModelName = "Flower0"
            },
            new GrowthStage{
                ProgressStart = 0.25,
                ProgressEnd = 0.5,
                ModelName = "Flower1"
            },
            new GrowthStage{
                ProgressStart = 0.5,
                ProgressEnd = 0.75,
                ModelName = "Flower2"
            },
            new GrowthStage{
                ProgressStart = 0.75,
                ProgressEnd = 1.0,
                ModelName = "Flower3"
            },
            new GrowthStage{
                ProgressStart = 1,
                ProgressEnd = 1.25,
                ModelName = "Flower4"
            },
        };
    }
}
