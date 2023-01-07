using Assets.Scripts.Model;

using UnityEngine;

public class WorldBehaviour : MonoBehaviour
{
    private GameObject tileContainer;
    private GameObject templateContainer;

    // Start is called before the first frame update
    private void Start()
    {
        this.tileContainer = transform.Find("TileContainer").gameObject;
        this.templateContainer = transform.Find("TemplateContainer").gameObject;

        var gameState = Assets.Scripts.Base.Core.Game.State;

        if (gameState != default)
        {
            if (gameState.World.Tiles.Count > 0)
            {
                RenderWorld(gameState.World);
            }
        }
    }

    private void RenderWorld(World world)
    {
        var tileTemplate = default(GameObject);

        foreach (var tile in world.Tiles)
        {
            var tileGameObject = Instantiate(tileTemplate, this.tileContainer.transform);

            var tileBehaviour = tileGameObject.GetComponent<TileBehaviour>();

            tileBehaviour.Tile = tile;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        Assets.Scripts.Base.Core.Game.State.ElapsedTime += Time.deltaTime;
    }
}
