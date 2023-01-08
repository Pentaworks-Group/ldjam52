using System;

using Assets.Scripts.Model;

using GameFrame.Core.Extensions;

using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public Tile Tile;

    private GameObject floorGameObject;
    private Boolean isColorSet;

    // Start is called before the first frame update
    void Start()
    {
        this.floorGameObject = transform.Find("Fliese").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.Tile != default)
        {
            if (!isColorSet && this.floorGameObject != null)
            {
                if (this.floorGameObject.TryGetComponent<MeshRenderer>(out var meshRenderer))
                {
                    if (meshRenderer.material != null)
                    {
                        meshRenderer.material.color = this.Tile.Color.ToUnity();
                        this.isColorSet = true;
                    }
                }
            }
        }
    }
}
