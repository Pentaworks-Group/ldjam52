using System.Collections;
using System.Collections.Generic;


using UnityEngine;

public class SavedGamedPreviewImpl : SavedGamePreview<Assets.Scripts.Core.GameState>
{
    public string Name { get; set; }   
    public string CreatedOn { get; set; }
    public string SavedOn { get; set; }
    public string TimeElapsed { get; set; }
    public string Money { get; set; }

    public override void Init(Assets.Scripts.Core.GameState savedGame, string key)
    {
        base.Init(savedGame, key);

        CreatedOn = string.Format("{0:G}", savedGame.CreatedOn);
        Name = CreatedOn;
        SavedOn = string.Format("{0:G}", savedGame.SavedOn);
        TimeElapsed = string.Format("{0:F1}s", savedGame.ElapsedTime);
        Money = savedGame.FarmStorage.MoneyBalance.ToString();
    }
}
