using System;

using Assets.Scripts.Model;

using GameFrame.Core.Extensions;

using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    private GameObject floorGameObject;
    private GameObject fieldGameObject;
    private GameObject naturalAreaGameObject;

    private Boolean isTileOwned;

    public Tile Tile { get; private set; }

    public void SetTile(Tile tile)
    {
        this.Tile = tile;

        if (tile != default)
        {
            this.isTileOwned = tile.IsOwned;

            if (this.floorGameObject != null)
            {
                if (this.floorGameObject.TryGetComponent<MeshRenderer>(out var meshRenderer))
                {
                    if (meshRenderer.material != null)
                    {
                        meshRenderer.material.color = tile.Color.ToUnity();
                    }
                }
            }
        }
    }

    private void Awake()
    {
        this.floorGameObject = transform.Find("Surface").gameObject;
        this.fieldGameObject = transform.Find("Field").gameObject;
        this.naturalAreaGameObject = transform.Find("NatureArea").gameObject;
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
                isTileOwned = this.Tile.IsOwned;

                this.fieldGameObject.SetActive(this.Tile.IsOwned);
                this.naturalAreaGameObject.SetActive(!this.Tile.IsOwned);
            }
        }
    }
}
