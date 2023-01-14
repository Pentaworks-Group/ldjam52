using System;
using System.Collections.Generic;

using Assets.Scripts.Base;
using Assets.Scripts.Constants;
using Assets.Scripts.Core;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Buildings;
using Assets.Scripts.Scene.Shops;
using Assets.Scripts.UI.TileView;

using GameFrame.Core.Extensions;

using UnityEngine;

public class WorldBehaviour : MonoBehaviour
{
    private GameObject tileContainer;
    private GameObject templateContainer;

    private readonly IDictionary<String, GameObject> buildingTemplateCache = new Dictionary<String, GameObject>();
    private int escTimeout = 3;
    private Boolean isFarmSet = false;
    private GameState gameState;

    public TileViewBehaviour TileViewBehaviour;
    public FieldViewBehaviour FieldViewBehaviour;
    public LaboratoryBehaviour LaboratoryBehaviour;
    public SeedShopBehaviour SeedShopBehaviour;

    public PauseMenuBehavior PauseMenuBehaviour;

    //Random Ambient Sounds
    private float nextSoundEffectTime = 0;

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

            LoadTemplates();

            gameState = Assets.Scripts.Base.Core.Game.State;

            if (gameState != default)
            {
                var tileController = new TileController();

                Assets.Scripts.Base.Core.Game.TileController = tileController;

                if (gameState.World.Tiles.Count > 0)
                {
                    RenderWorld(gameState.World);
                }
            }

            Core.Game.BackgroundAudioManager.Clips = Core.Game.AudioClipListGame;
            Core.Game.AmbienceAudioManager.Resume();
        }
    }

    private void Start()
    {
        this.TileViewBehaviour.OnHide.AddListener(OnTileViewHide);
        this.TileViewBehaviour.FieldViewRequested.AddListener(OnFieldViewRequested);

        this.FieldViewBehaviour.OnHide.AddListener(OnFieldViewHide);
        this.SeedShopBehaviour.OnHide.AddListener(OnShopViewHide);
        this.LaboratoryBehaviour.OnHide.AddListener(OnLaboratiryHide);

        Core.Game.LockCameraMovement = false;
    }

    private void Update()
    {
        if (Core.Game.State != default)
        {
            Core.Game.State.ElapsedTime += Time.deltaTime;

            if (!isFarmSet && gameState.World.Farm != default)
            {
                RenderFarm(gameState.World.Farm);
            }

            playRandomEffectSound();

            if (escTimeout > 0)
            {
                escTimeout--;
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

            Core.Game.TileController.AddTile(tileBehaviour);

            if ((tileGameObject.transform.position.x != tile.Position.X) || (tileGameObject.transform.position.z != tile.Position.Z))
            {
                tileGameObject.transform.position = new UnityEngine.Vector3(tile.Position.X, this.transform.position.y, tile.Position.Z);
            }
        }

        if (world.Farm != default)
        {
            RenderFarm(world.Farm);
        }

        if (world.Buildings?.Count > 0)
        {
            RenderBuildings(world.Buildings);
        }
    }

    private void TileSelected(TileBehaviour tileBehaviour)
    {
        if (tileBehaviour != null)
        {
            if (!WasEscPressed())
            {
                if (tileBehaviour.Tile.Building != default)
                {
                    if (tileBehaviour.Tile.Building is Farm)
                    {
                        PauseMenuBehaviour.Show();
                    }
                    else if (tileBehaviour.Tile.Building is Shop)
                    {
                        this.SeedShopBehaviour.Show();
                    }
                    else if (tileBehaviour.Tile.Building is Laboratory)
                    {
                        this.LaboratoryBehaviour.Show();
                    }
                }
                else if (!tileBehaviour.Tile.IsOwned || (Core.Game.State.World.Farm == default))
                {
                    this.TileViewBehaviour.Show(tileBehaviour);
                }
                else
                {
                    this.FieldViewBehaviour.Show(tileBehaviour.FieldBehaviour);
                }
            }
        }
    }

    private void OnFieldViewRequested(TileBehaviour tileBehaviour)
    {
        this.FieldViewBehaviour.Show(tileBehaviour.FieldBehaviour);
    }

    public void PressEsc()
    {
        escTimeout = 3;
    }

    public bool WasEscPressed()
    {
        return escTimeout > 0;
    }

    private void RenderFarm(Farm farm)
    {
        var farmTemplate = GetBuildingTemplate("FarmPrefab");

        var farmGameObject = Instantiate(farmTemplate, tileContainer.transform);

        if ((farmGameObject.transform.position.x != farm.Position.X) || (farmGameObject.transform.position.z != farm.Position.Z))
        {
            farmGameObject.transform.position = new UnityEngine.Vector3(farm.Position.X, this.transform.position.y, farm.Position.Z);
        }

        isFarmSet = true;
    }

    private void RenderBuildings(List<Building> buildings)
    {
        foreach (var building in buildings)
        {
            if (!String.IsNullOrEmpty(building.TemplateReference))
            {
                var buildingTemplate = GetBuildingTemplate(building.TemplateReference);

                var buildingGameObject = Instantiate(buildingTemplate, tileContainer.transform);

                if ((buildingGameObject.transform.position.x != building.Position.X) || (buildingGameObject.transform.position.z != building.Position.Z))
                {
                    buildingGameObject.transform.position = new UnityEngine.Vector3(building.Position.X, this.transform.position.y, building.Position.Z);
                }
            }
            else
            {
                throw new Exception($"Building of type '{building.GetType().FullName}' has no Template set!");
            }
        }
    }

    private void playRandomEffectSound()
    {
        if (Assets.Scripts.Base.Core.Game.State.ElapsedTime > nextSoundEffectTime && nextSoundEffectTime != 0)
        {
            Core.Game.EffectsAudioManager.Play(Core.Game.AmbientEffectsClipList.GetRandomEntry());
            double randomNumber = UnityEngine.Random.value;

            nextSoundEffectTime = (float)(randomNumber * 30.0 + 5.0 + Assets.Scripts.Base.Core.Game.State.ElapsedTime);
        }
        else if (nextSoundEffectTime == 0)
        {
            double randomNumber = UnityEngine.Random.value;

            nextSoundEffectTime = (float)(randomNumber * 30.0 + 5.0 + Assets.Scripts.Base.Core.Game.State.ElapsedTime);
        }
    }

    private void LoadTemplates()
    {
        var templateConatiner = transform.Find("Templates");

        if (templateConatiner != default)
        {
            var buildingTemplates = templateContainer.transform.Find("Buildings").gameObject;

            if (buildingTemplates.transform.childCount > 0)
            {
                foreach (Transform buildingTemplate in buildingTemplates.transform)
                {
                    this.buildingTemplateCache[buildingTemplate.name] = buildingTemplate.gameObject;
                }
            }
            else
            {
                throw new Exception("Missing Templates!");
            }
        }
    }

    private GameObject GetBuildingTemplate(String templateReference)
    {
        if (!String.IsNullOrEmpty(templateReference))
        {
            if (this.buildingTemplateCache.TryGetValue(templateReference, out var buildingTempalte))
            {
                return buildingTempalte;
            }
        }

        throw new Exception(String.Format("Building template '{0}' not found!", templateReference));
    }

    private void OnFieldViewHide()
    {
        PressEsc();
    }

    private void OnTileViewHide()
    {
        PressEsc();
    }

    private void OnShopViewHide()
    {
        PressEsc();
    }

    private void OnLaboratiryHide()
    {
        PressEsc();
    }
}
