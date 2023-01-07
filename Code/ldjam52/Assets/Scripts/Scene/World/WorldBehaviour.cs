using Assets.Scripts.Base;
using Assets.Scripts.Constants;
using Assets.Scripts.Model;

using UnityEngine;

public class WorldBehaviour : MonoBehaviour
{
    private GameObject tileContainer;
    private GameObject templateContainer;

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

            var gameState = Assets.Scripts.Base.Core.Game.State;

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

            tileBehaviour.Tile = tile;

            if ((tileGameObject.transform.position.x != tile.Position.X) || (tileGameObject.transform.position.z != tile.Position.Z))
            {
                tileGameObject.transform.position = new Vector3(tile.Position.X, this.transform.position.y, tile.Position.Z);
            }
        }
    }

    private void Update()
    {
        Assets.Scripts.Base.Core.Game.State.ElapsedTime += Time.deltaTime;
    }
}
