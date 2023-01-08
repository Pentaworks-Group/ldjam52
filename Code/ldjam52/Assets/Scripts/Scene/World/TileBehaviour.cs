using System;

using Assets.Scripts.Model;

using GameFrame.Core.Extensions;

using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    private GameObject floorGameObject;
    private GameObject naturalAreaGameObject;

    private Boolean isTileOwned;

    public FieldBehaviour FieldBehaviour;
    public TileViewBehaviour TileViewBehaviour;

    public Tile Tile { get; private set; }

    public void SetTile(Tile tile)
    {
        this.Tile = tile;

        if (this.FieldBehaviour != null)
        {
            this.FieldBehaviour.SetField(tile?.Field);
        }

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

    public void ShowTileView()
    {
        if (this.TileViewBehaviour != null)
        {
            this.TileViewBehaviour.ViewField(this.FieldBehaviour);
        }
    }

    private void Awake()
    {
        this.floorGameObject = transform.Find("Surface").gameObject;
        this.FieldBehaviour = transform.Find("Field").GetComponent<FieldBehaviour>();
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

                this.FieldBehaviour.gameObject.SetActive(this.Tile.IsOwned);
                this.naturalAreaGameObject.SetActive(!this.Tile.IsOwned);
            }
        }
    }
}
