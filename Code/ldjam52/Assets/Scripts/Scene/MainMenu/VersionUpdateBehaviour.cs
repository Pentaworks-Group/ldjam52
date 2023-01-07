using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersionUpdateBehaviour : MonoBehaviour
{
    void Awake()
    {
        Text text = transform.GetComponent<Text>();
        text.text = "Version: " + Application.version;
    }
}
