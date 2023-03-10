using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Base;
using Assets.Scripts.Constants;
using Assets.Scripts.Core;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Buildings;
using Assets.Scripts.Scene.Shops;
using Assets.Scripts.Scene.World.Clickables;
using Assets.Scripts.UI.TileView;

using GameFrame.Core.Extensions;

using UnityEngine;

public class WorldBehaviour : MonoBehaviour
{
    private GameObject tileContainer;
    private GameObject buildingContainer;

    private readonly IDictionary<String, GameObject> tileTemplateCache = new Dictionary<String, GameObject>();
    private readonly IDictionary<String, GameObject> buildingTemplateCache = new Dictionary<String, GameObject>();
    private int escTimeout = 3;
    private Boolean isFarmSet = false;
    private GameState gameState;

    public TileViewBehaviour TileViewBehaviour;
    public FieldViewBehaviour FieldViewBehaviour;
    public LaboratoryBehaviour LaboratoryBehaviour;
    public SeedShopBehaviour SeedShopBehaviour;
    public CameraBehaviour CameraBehaviour;

    public PauseMenuBehavior PauseMenuBehaviour;

    //Random Ambient Sounds
    private float nextSoundEffectTime = 0;

    public void PressEsc()
    {
        escTimeout = 3;
    }

    public bool WasEscPressed()
    {
        return escTimeout > 0;
    }

    private void Awake()
    {
        if (Core.Game.AvailableGameModes.Count < 1)
        {
            Assets.Scripts.Base.Core.Game.ChangeScene(SceneNames.MainMenu);
        }
        else
        {
            this.tileContainer = transform.Find("TileContainer").gameObject;
            this.buildingContainer = transform.Find("BuildingContainer").gameObject;

            LoadTemplates();

            gameState = Assets.Scripts.Base.Core.Game.State;
            Assets.Scripts.Scene.World.UpdateManager.ResetArrays((gameState.World.Height * gameState.World.Width) * 2);

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

        this.PauseMenuBehaviour.PauseToggled.AddListener(OnPauseMenuToggled);

        Core.Game.LockCameraMovement = false;
        Time.timeScale = 1;
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

            PlayRandomEffectSound();

            if (escTimeout > 0)
            {
                escTimeout--;
            }

            CheckShortKeys();
        }

        Assets.Scripts.Scene.World.UpdateManager.UpdateBehaviours();
    }

    private void RenderWorld(World world)
    {
        var tileTemplate = this.tileTemplateCache.Values.FirstOrDefault();

        foreach (var tile in world.Tiles)
        {
            var tileGameObject = Instantiate(tileTemplate, this.tileContainer.transform);

            var tileBehaviour = tileGameObject.GetComponent<TileBehaviour>();

            tileBehaviour.SetTile(tile);

            var clickableTile = tileGameObject.GetComponent<ClickableTile>();
            clickableTile.item = tile;
            clickableTile.Clicked.AddListener(OnTileClicked);

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

    private void OnTileClicked(Tile tile)
    {
        if (!WasEscPressed())
        {
            var tileBehaviour = Core.Game.TileController.GetBehaviour(tile.ID);

            if (tileBehaviour != default)
            {
                if (tile.Building != default)
                {
                    OnBuildingClicked(tile.Building); // redirect due to clickable fence :)
                }
                else if (!tileBehaviour.Tile.IsOwned || (Core.Game.State.World.Farm == default))
                {
                    this.CameraBehaviour.HideToFarmButton();
                    this.TileViewBehaviour.Show(tileBehaviour);
                }
                else
                {
                    this.CameraBehaviour.HideToFarmButton();
                    this.FieldViewBehaviour.Show(tileBehaviour.FieldBehaviour);
                }
            }
        }
    }

    private void OnFieldViewRequested(TileBehaviour tileBehaviour)
    {
        this.CameraBehaviour.HideToFarmButton();
        this.FieldViewBehaviour.Show(tileBehaviour.FieldBehaviour);
    }

    private void RenderFarm(Farm farm)
    {
        var farmTemplate = GetBuildingTemplate("FarmPrefab");

        var farmGameObject = Instantiate(farmTemplate, buildingContainer.transform);

        var collider = farmGameObject.GetComponentInChildren<ClickableBuilding>();

        if (collider != null)
        {
            collider.item = farm;
            collider.Clicked.AddListener(OnBuildingClicked);
        }
        else
        {
            throw new Exception("Buildings need to have a BuildingColliderBehaviour!");
        }

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

                var buildingGameObject = Instantiate(buildingTemplate, buildingContainer.transform);

                var collider = buildingGameObject.GetComponentInChildren<ClickableBuilding>();

                if (collider != null)
                {
                    collider.item = building;
                    collider.Clicked.AddListener(OnBuildingClicked);
                }
                else
                {
                    Debug.LogError("Buildings need to have a BuildingColliderBehaviour!");
                    throw new Exception("Buildings need to have a BuildingColliderBehaviour!");
                }

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

    private void OnBuildingClicked(Building building)
    {
        if (building is Farm)
        {
            PauseMenuBehaviour.ToggleMenu();
        }
        else if (building is Shop)
        {
            this.CameraBehaviour.HideToFarmButton();
            this.SeedShopBehaviour.Show();
        }
        else if (building is Laboratory)
        {
            this.CameraBehaviour.HideToFarmButton();
            this.LaboratoryBehaviour.Show();
        }
    }

    private void PlayRandomEffectSound()
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
            LoadTemplates(this.tileTemplateCache, templateConatiner, "Tiles");
            LoadTemplates(this.buildingTemplateCache, templateConatiner, "Buildings");
        }
    }

    private void LoadTemplates<T>(IDictionary<String, T> cache, Transform rootTemplateContainer, String templateContainerName)
    {
        var buildingTemplates = rootTemplateContainer.transform.Find(templateContainerName).gameObject;

        if (buildingTemplates.transform.childCount > 0)
        {
            foreach (Transform buildingTemplate in buildingTemplates.transform)
            {
                if (buildingTemplate.gameObject is T castedObject)
                {
                    cache[buildingTemplate.name] = castedObject;
                }
            }
        }
        else
        {
            throw new Exception("Missing Templates!");
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
        this.CameraBehaviour.ShowToFarmButton();
    }

    private void OnTileViewHide()
    {
        PressEsc();
        this.CameraBehaviour.ShowToFarmButton();
    }

    private void OnShopViewHide()
    {
        PressEsc();
        this.CameraBehaviour.ShowToFarmButton();
    }

    private void OnLaboratiryHide()
    {
        PressEsc();
        this.CameraBehaviour.ShowToFarmButton();
    }

    private void CheckShortKeys()
    {
        if (!WasEscPressed())
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                this.CameraBehaviour.CenterFarm();
            }
        }
    }

    private void OnPauseMenuToggled(Boolean isPaused)
    {
        if (isPaused)
        {
            this.CameraBehaviour.HideToFarmButton();
        }
        else
        {
            this.CameraBehaviour.ShowToFarmButton();
        }
    }
}
