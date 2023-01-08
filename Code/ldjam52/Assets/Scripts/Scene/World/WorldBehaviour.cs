using System;

using Assets.Scripts.Base;
using Assets.Scripts.Constants;
using Assets.Scripts.Core;
using Assets.Scripts.Model;
using Assets.Scripts.UI.TileView;

using UnityEngine;

public class WorldBehaviour : MonoBehaviour
{
    private GameObject tileContainer;
    private GameObject templateContainer;

    private Boolean isFarmSet = false;
    private GameState gameState;

    public TileViewBehaviour TileViewBehaviour;
    public FieldViewBehaviour FieldViewBehaviour;

    private void Awake()
    {
        if (Core.Game.AvailableGameModes.Count < 1)
        {
            Assets.Scripts.Base.Core.Game.ChangeScene(SceneNames.MainMenu);
        }
        else
        {
            this.tileContainer = transform.Find("TileContainer").gameObject;
            this.templateContainer = transform.Find("Templates").gameObject;

            gameState = Assets.Scripts.Base.Core.Game.State;

            if (gameState != default)
            {
                if (gameState.World.Tiles.Count > 0)
                {
                    RenderWorld(gameState.World);
                }
            }

        }
    }

    private void RenderWorld(World world)
    {
        var tileTemplate = templateContainer.GetComponentInChildren<TileBehaviour>().gameObject;

        foreach (var tile in world.Tiles)
        {
            var tileGameObject = Instantiate(tileTemplate, this.tileContainer.transform);

            var tileBehaviour = tileGameObject.GetComponent<TileBehaviour>();
            tileBehaviour.OnClick.AddListener(TileSelected);

            tileBehaviour.SetTile(tile);
            tileBehaviour.FieldViewBehaviour = this.FieldViewBehaviour;

            if ((tileGameObject.transform.position.x != tile.Position.X) || (tileGameObject.transform.position.z != tile.Position.Z))
            {
                tileGameObject.transform.position = new Vector3(tile.Position.X, this.transform.position.y, tile.Position.Z);
            }
        }

        if (world.Farm != default)
        {
            RenderFarm(world.Farm);
        }
    }

    private void TileSelected(TileBehaviour tileBehaviour)
    {
        if (tileBehaviour != null)
        {
            if (!tileBehaviour.Tile.IsOwned)
            {
                this.TileViewBehaviour.Show();
            }
            else
            {
                tileBehaviour.ShowFieldView();
            }
        }
    }

    private void Update()
    {
        Assets.Scripts.Base.Core.Game.State.ElapsedTime += Time.deltaTime;

        if (!isFarmSet && gameState.World.Farm != default)
        {
            RenderFarm(gameState.World.Farm);
        }
    }

    private void RenderFarm(Farm farm)
    {
        var farmTemplate = templateContainer.transform.Find("Farm").gameObject;

        var farmGameObject = Instantiate(farmTemplate, tileContainer.transform);

        if ((farmGameObject.transform.position.x != farm.Position.X) || (farmGameObject.transform.position.z != farm.Position.Z))
        {
            farmGameObject.transform.position = new Vector3(farm.Position.X, this.transform.position.y, farm.Position.Z);
        }

        isFarmSet = true;
    }
}
