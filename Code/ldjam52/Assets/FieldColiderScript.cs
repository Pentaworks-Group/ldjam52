using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldColiderScript : MonoBehaviour
{
    FieldBehaviour fieldBehaviour;

    private void Awake()
    {
        fieldBehaviour = transform.GetComponentInParent<FieldBehaviour>();
    }


    private void OnMouseDown()
    {
        fieldBehaviour.SelectField();
    }
}
