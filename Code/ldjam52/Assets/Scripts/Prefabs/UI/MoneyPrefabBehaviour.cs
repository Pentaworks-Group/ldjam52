using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Base;

using TMPro;

using UnityEngine;

public class MoneyPrefabBehaviour : MonoBehaviour
{
    public TMP_Text balanceText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        balanceText.text = Core.Game.State.FarmStorage.MoneyBalance.ToString();
    }

}
