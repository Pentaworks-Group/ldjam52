using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Model;

namespace Assets.Scripts.Constants
{
    public class DevelopmentStages
    {
        public static List<DevelopmentStage> PlantAnalyticsStages = new List<DevelopmentStage>
        {
            new DevelopmentStage
            {
                StageId = 0,
                Name = "Discover one Chromosome in an analysis",
                ValueVisibleCount = 1,
                UpgradeCost = 0,
                AnalyticsCost = 50,
                AnalyticsPlantCost = 1
            },
            new DevelopmentStage
            {
                StageId = 0,
                Name = "Discover two Chromosomes in an analysis",
                ValueVisibleCount = 2,
                UpgradeCost = 10000,
                AnalyticsCost = 75,
                AnalyticsPlantCost = 2
            },
            new DevelopmentStage
            {
                StageId = 0,
                Name = "Discover three Chromosomes in an analysis",
                ValueVisibleCount = 3,
                UpgradeCost = 7500,
                AnalyticsCost = 100,
                AnalyticsPlantCost = 3
            },
            new DevelopmentStage
            {
                StageId = 0,
                Name = "Discover four Chromosomes in an analysis",
                ValueVisibleCount = 4,
                UpgradeCost = 10000,
                AnalyticsCost = 125,
                AnalyticsPlantCost = 4
            },
            new DevelopmentStage
            {
                StageId = 0,
                Name = "Discover five Chromosomes in an analysis",
                ValueVisibleCount = 5,
                UpgradeCost = 12500,
                AnalyticsCost = 150,
                AnalyticsPlantCost = 5
            },
            new DevelopmentStage
            {
                StageId = 0,
                Name = "Discover six Chromosomes in an analysis",
                ValueVisibleCount = 6,
                UpgradeCost = 15000,
                AnalyticsCost = 175,
                AnalyticsPlantCost = 6
            }
        };

        public static List<DevelopmentStage> FieldAnalyticsStages = new List<DevelopmentStage>
        {
            new DevelopmentStage
            {
                StageId = 0,
                Name = "Discover one Property in a Scan",
                ValueVisibleCount = 1,
                UpgradeCost = 0,
                AnalyticsCost = 50,
                AnalyticsPlantCost = 0
            },
            new DevelopmentStage
            {
                StageId = 0,
                Name = "Discover two Properties in a Scan",
                ValueVisibleCount = 2,
                UpgradeCost = 5000,
                AnalyticsCost = 75,
                AnalyticsPlantCost = 0
            },
            new DevelopmentStage
            {
                StageId = 0,
                Name = "Discover one Properties in a Scan",
                ValueVisibleCount = 3,
                UpgradeCost = 7500,
                AnalyticsCost = 100,
                AnalyticsPlantCost = 0
            },
            new DevelopmentStage
            {
                StageId = 0,
                Name = "Discover one Properties in a Scan",
                ValueVisibleCount = 4,
                UpgradeCost = 10000,
                AnalyticsCost = 125,
                AnalyticsPlantCost = 0
            },
        };
    }
}
