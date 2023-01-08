using System;

using Assets.Scripts.Model;

using GameFrame.Core.Extensions;

using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public Tile Tile;

    private GameObject floorGameObject;
    private Boolean isColorSet;
    private Boolean isTileOwned;

    private void Awake()
    {
        this.floorGameObject = transform.Find("Fliese").gameObject;
        

        if (this.Tile != default)
        {
            this.isTileOwned = this.Tile.IsOwned;

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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (this.Tile != default)
        {
            if (this.Tile.IsOwned && !isTileOwned)
            {
                transform.Find("Field").gameObject.SetActive(true);
                transform.Find("NatureArea").gameObject.SetActive(false);
            }
        }
        //if (this.Tile != default)
        //{
        //    if (!isColorSet && this.floorGameObject != null)
        //    {
        //        if (this.floorGameObject.TryGetComponent<MeshRenderer>(out var meshRenderer))
        //        {
        //            if (meshRenderer.material != null)
        //            {
        //                meshRenderer.material.color = this.Tile.Color.ToUnity();
        //                this.isColorSet = true;
        //            }
        //        }
        //    }
        //}
    }
}
