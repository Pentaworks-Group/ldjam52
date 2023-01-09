using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Core.Inventory;

using TMPro;

using UnityEngine;

public class FreeStorageBehaviour : MonoBehaviour
{
    public TMP_Text freeStorageText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        freeStorageText.text = "Free Storage Space: "+FarmStorageController.GetFreeStorageSpace();
    }
}
