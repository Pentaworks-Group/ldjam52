using System;
using System.Collections.Generic;

using Assets.Extensions;
using Assets.Scripts.Base;
using Assets.Scripts.Core;
using Assets.Scripts.Model;

using GameFrame.Core.Extensions;

using UnityEngine;
using UnityEngine.Events;

public class TileBehaviour : MonoBehaviour
{
    private readonly Dictionary<String, GameObject> borders = new Dictionary<String, GameObject>();

    private GameObject floorGameObject;
    private GameObject naturalAreaGameObject;

    private Boolean isTileOwned;

    public FieldBehaviour FieldBehaviour;

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
                        meshRenderer.material.color = tile.Biome.Type.Color.ToUnity();
                    }
                }
            }
        }
    }

    private void Awake()
    {
        this.floorGameObject = transform.Find("Surface").gameObject;
        this.FieldBehaviour = transform.Find("Field").GetComponent<FieldBehaviour>();
        this.naturalAreaGameObject = transform.Find("NatureArea").gameObject;

        var borderContainer = transform.Find("Border");

        if (borderContainer != null)
        {
            foreach (Transform borderTransform in borderContainer)
            {
                this.borders[borderTransform.name] = borderTransform.gameObject;
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
            if (this.Tile.IsOwned && !this.isTileOwned)
            {
                this.isTileOwned = this.Tile.IsOwned;

                this.FieldBehaviour.gameObject.SetActive(this.Tile.IsOwned);
                this.naturalAreaGameObject.SetActive(!this.Tile.IsOwned);

                RenderBorder();
            }
        }
    }

    private void RenderBorder()
    {
        UpdateTile(TileDirection.Top);
        UpdateTile(TileDirection.Left);
        UpdateTile(TileDirection.Bottom);
        UpdateTile(TileDirection.Right);
    }

    private void UpdateTile(TileDirection direction, Boolean isFromNeightbour = false)
    {
        if (borders.TryGetValue(direction.ToString(), out var border))
        {
            var isActive = true;

            var neighbourTileBehaviour = Core.Game.TileController.GetTileInDirection(this.Tile, direction);

            if ((neighbourTileBehaviour != null) && (neighbourTileBehaviour.Tile.IsOwned))
            {
                isActive = false;

                if (!isFromNeightbour)
                {
                    neighbourTileBehaviour.UpdateTile(direction.Invert(), true);
                }
            }

            border.SetActive(isActive);
        }
    }
}
