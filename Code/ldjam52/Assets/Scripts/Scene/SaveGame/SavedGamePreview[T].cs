using System.Collections;
using System.Collections.Generic;

using GameFrame.Core;

using UnityEngine;

public abstract class SavedGamePreview<TGameState> where TGameState : GameState
{
    public string key { get; private set; }


    public virtual void Init(TGameState gameState, string key) 
    {
        this.key = key;
    }
}
