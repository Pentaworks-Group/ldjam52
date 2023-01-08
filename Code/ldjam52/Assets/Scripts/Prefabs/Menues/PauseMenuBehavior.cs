using System;
using System.Collections.Generic;

using Assets.Scripts.Base;
using Assets.Scripts.Constants;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseMenuBehavior : MonoBehaviour
{
    public UnityEvent<Boolean> PauseToggled = new UnityEvent<Boolean>();

    public List<GameObject> ObjectsToHide = new();

    private GameObject menuToggle;
    private GameObject pauseArea;
    private GameObject optionsArea;
    private GameObject saveGameArea;
    //public GameObject GUI;
    private Button backButton;
    private Button continueButton;

    //private Transform inputFieldLeft;
    //private Transform inputFieldRight;
    // Start is called before the first frame update

    private void Awake()
    {
        menuToggle = transform.Find("ToggleArea").gameObject;
        pauseArea = transform.Find("ToggleArea/ContentArea/PauseArea").gameObject;
        optionsArea = transform.Find("ToggleArea/ContentArea/OptionsArea").gameObject;
        saveGameArea = transform.Find("ToggleArea/ContentArea/SaveGameArea").gameObject;

        backButton = transform.Find("ToggleArea/Header/Back").GetComponent<Button>();
        continueButton = transform.Find("ToggleArea/Header/Continue").GetComponent<Button>();
    }

    void Start()
    {
        //inputFieldLeft = GUI.transform.Find("InputFieldLeft");
        //inputFieldRight = GUI.transform.Find("InputFieldRight");
        


        menuToggle.SetActive(false);
        //ReloadUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if (menuToggle.activeSelf == true)
        {
            if (this.optionsArea.activeSelf)
            {
                this.OnBackButtonClicked();
            }
            else
            {
                //Core.Game.PlayButtonSound();
                Hide();

                this.PauseToggled.Invoke(false);
                foreach (GameObject gameObject in ObjectsToHide)
                {
                    gameObject.SetActive(true);
                }
                //ReloadUI(); //TODO prevent double activating/deactivating
            }
        }
        else
        {
            //Core.Game.PlayButtonSound();

            this.PauseToggled.Invoke(true);
            foreach (GameObject gameObject in ObjectsToHide)
            {
                gameObject.SetActive(false);
            }
            Show();
        }
    }

    public void ShowSavedGames()
    {
        //Core.Game.PlayButtonSound();

        SetVisible(saveGame: true);
    }

    public void Hide()
    {
        menuToggle.SetActive(false);
        Assets.Scripts.Base.Core.Game.LockCameraMovement = false;

        Time.timeScale = 1;
    }

    public void Show()
    {
        Time.timeScale = 0;

        SetVisible(pauseMenu: true);

        menuToggle.SetActive(true);
        Assets.Scripts.Base.Core.Game.LockCameraMovement = true;
    }

    public void OnBackButtonClicked()
    {
        if (this.saveGameArea.activeSelf)
        {
            Core.Game.SaveOptions();
        }

        //Core.Game.PlayButtonSound();
        SetVisible(pauseMenu: true);
    }

    //private void ReloadUI()
    //{
    //    switch (Core.Game.Options.MobileInterface)
    //    {
    //        case "None":
    //            inputFieldLeft.gameObject.SetActive(false);
    //            inputFieldRight.gameObject.SetActive(false);
    //            break;
    //        case "Left":
    //            inputFieldLeft.gameObject.SetActive(true);
    //            inputFieldRight.gameObject.SetActive(false);
    //            break;
    //        case "Right":
    //            inputFieldLeft.gameObject.SetActive(false);
    //            inputFieldRight.gameObject.SetActive(true);
    //            break;
    //        default:
    //            inputFieldLeft.gameObject.SetActive(false);
    //            inputFieldRight.gameObject.SetActive(true);
    //            break;
    //    }
    //}

    public void ShowOptions()
    {
        //Core.Game.PlayButtonSound();
        this.SetVisible(options: true);
    }

    public void Quit()
    {
        Time.timeScale = 1;

        //Core.Game.PlayButtonSound();
        Core.Game.Stop();
        Core.Game.ChangeScene(SceneNames.MainMenu);
    }



    private void SetVisible(Boolean pauseMenu = false, Boolean options = false, Boolean saveGame = false)
    {
        this.pauseArea.SetActive(pauseMenu);
        this.optionsArea.SetActive(options);
        this.saveGameArea.SetActive(saveGame);

        this.continueButton.gameObject.SetActive(pauseMenu);
        this.backButton.gameObject.SetActive(!pauseMenu);
    }
}
