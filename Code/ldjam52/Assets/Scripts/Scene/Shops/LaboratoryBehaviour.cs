using Assets.Scripts.Core.Inventory;
using Assets.Scripts.Model;
using Assets.Scripts.Prefabs.World;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scene.Shops
{
    public class LaboratoryBehaviour : ViewBaseBehaviour
    {
        private GameObject laboratoryViewToggle;

        public GameObject PlantAnalyzerPanel;
        public GameObject FieldAnalyzerPanel;

        public TMP_Text BalanceText;
                
        public override void Show()
        {
            base.Show();

            this.laboratoryViewToggle.SetActive(true);
            updateGUI();
        }

        public override void Hide()
        {
            base.Hide();

            this.laboratoryViewToggle.SetActive(false);
        }

        protected override void OnStart()
        {
            base.OnStart();

            this.laboratoryViewToggle = transform.Find("LaboratoryViewToggle").gameObject;

            updateGUI();
        }

        private void updateGUI()
        {
            BalanceText.text = FarmStorageController.GetStorageBalance().ToString();
            UpdateAnalyzerPanel(Base.Core.Game.State.PlantAnalyzer, PlantAnalyzerPanel);
            UpdateAnalyzerPanel(Base.Core.Game.State.FieldAnalyzer, FieldAnalyzerPanel);
        }

        private void UpdateAnalyzerPanel(Analyzer analyzer, GameObject panel)
        {
            panel.transform.Find("AnalyzerName").GetComponent<TMP_Text>().text = analyzer.Name;
            panel.transform.Find("AnalyzerDesc").GetComponent<TMP_Text>().text = analyzer.Description;

            panel.transform.Find("AnalyzerImage").GetComponent<Image>().sprite = GameFrame.Base.Resources.Manager.Sprites.Get(analyzer.ImgName);

            DevelopmentStage nextStage = getNextDevelopmentStage(analyzer);
            if (nextStage != null)
            {
                panel.transform.Find("NextStageDesc").GetComponent<TMP_Text>().text = "Next Stage: " + nextStage.Name;
                Image footer = panel.transform.Find("Footer").GetComponent<Image>();
                footer.transform.Find("CostText").GetComponent<TMP_Text>().text = nextStage.UpgradeCost.ToString();

                if (FarmStorageController.GetStorageBalance() < nextStage.UpgradeCost)
                {
                    Debug.Log("Not Enough Money to Upgrade");
                    Button upgradeButton = footer.transform.Find("ButtonUpgrade").GetComponent<Button>();
                    upgradeButton.enabled = false;
                    upgradeButton.transform.Find("Text").GetComponent<TMP_Text>().color = Color.gray;
                }
            }
            else
            {
                panel.transform.Find("ButtonUpgrade").gameObject.SetActive(false);
            }
        }

   
        public void UpgradePlantAnalyzer()
        {
            DevelopmentStage nextStage = getNextDevelopmentStage(Base.Core.Game.State.PlantAnalyzer);

            if (nextStage != null && FarmStorageController.GetStorageBalance() >= nextStage.UpgradeCost)
            {
                //Check Money Amount
                Base.Core.Game.State.PlantAnalyzer.CurrentDevelopmentStage = nextStage;
                FarmStorageController.TakeMoneyOfStorage(nextStage.UpgradeCost);
                updateGUI();
            }
        }

        public void UpgradeFieldAnalyzer()
        {
            DevelopmentStage nextStage = getNextDevelopmentStage(Base.Core.Game.State.FieldAnalyzer);

            if (nextStage != null && FarmStorageController.GetStorageBalance() >= nextStage.UpgradeCost)
            {
                //Check Money Amount
                Base.Core.Game.State.FieldAnalyzer.CurrentDevelopmentStage = nextStage;
                FarmStorageController.TakeMoneyOfStorage(nextStage.UpgradeCost);
                updateGUI();
            }
        }

        protected DevelopmentStage getNextDevelopmentStage(Analyzer analyzer)
        {
            int index = analyzer.DevelopmentStages.IndexOf(analyzer.CurrentDevelopmentStage);

            if (index < analyzer.DevelopmentStages.Count - 1)
            {
                return analyzer.DevelopmentStages[index + 1];
            }

            return null;
        }
    }
}