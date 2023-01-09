
using Assets.Scripts;
using Assets.Scripts.Base;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuBehaviour : MonoBehaviour
{
    private Slider effectsVolumeSlider;
    private Slider ambienceVolumeSlider;
    private Slider backgroundVolumeSlider;
    private Slider moveSensivitySlider;
    private Slider zoomSensivitySlider;
    private Toggle animationEnabledToggle;
    private Toggle sideScrollingEnabledToggle;
    private ToggleGroup mobileInterface;

    private void Awake()
    {
        effectsVolumeSlider = transform.Find("OptionContainer/EffectsVolume/Right/ForegroundSlider").GetComponent<Slider>();
        ambienceVolumeSlider = transform.Find("OptionContainer/AmbienceVolume/Right/AmbieceSlider").GetComponent<Slider>();
        backgroundVolumeSlider = transform.Find("OptionContainer/BackgroundVolume/Right/BackgroundSlider").GetComponent<Slider>();
        moveSensivitySlider = transform.Find("OptionContainer/MoveSensivity/Right/Slider").GetComponent<Slider>();
        zoomSensivitySlider = transform.Find("OptionContainer/ZoomSensivity/Right/Slider").GetComponent<Slider>();

        sideScrollingEnabledToggle = transform.Find("OptionContainer/EnableSideScroll/Right/Toggle").GetComponent<Toggle>();
        animationEnabledToggle = transform.Find("OptionContainer/EnableAnimations/Right/Toggle").GetComponent<Toggle>();
        mobileInterface = transform.Find("OptionContainer/MobileInterface/Right").GetComponent<ToggleGroup>();
    }

    private void Start()
    {
        this.UpdateValues();
        this.SetMobileInterfaceToggles();
    }

    private void SetMobileInterfaceToggles()
    {
        //switch (Core.Game.Options.MobileInterface)
        //{
        //    case "None":
        //        MobileInterface.transform.Find("ToggleNone").GetComponent<Toggle>().isOn = true;
        //        break;
        //    case "Left":
        //        MobileInterface.transform.Find("ToggleLeft").GetComponent<Toggle>().isOn = true;
        //        break;
        //    case "Right":
        //        MobileInterface.transform.Find("ToggleRight").GetComponent<Toggle>().isOn = true;
        //        break;
        //    default:
        //        MobileInterface.transform.Find("ToggleRight").GetComponent<Toggle>().isOn = true;
        //        break;
        //}
    }

    private void FixedUpdate()
    {
        this.UpdateValues();
    }

    public void OnForegroundSliderChanged()
    {
        Core.Game.EffectsAudioManager.Volume = effectsVolumeSlider.value;
        Core.Game.Options.EffectsVolume = effectsVolumeSlider.value;
    }

    public void OnAmbienceSliderChanged()
    {
        Core.Game.AmbienceAudioManager.Volume = ambienceVolumeSlider.value;
        Core.Game.Options.AmbienceVolume = ambienceVolumeSlider.value;
    }

    public void OnBackgroundSliderChanged()
    {
        Core.Game.BackgroundAudioManager.Volume = backgroundVolumeSlider.value;
        Core.Game.Options.BackgroundVolume = backgroundVolumeSlider.value;
    }

    public void OnMoveSensivitySliderChanged()
    {
        Core.Game.Options.MoveSensivity = moveSensivitySlider.value;
    }

    public void OnZoomSensivitySliderChanged()
    {
        Core.Game.Options.ZoomSensivity = zoomSensivitySlider.value;
    }

    public void OnAnimationEnabledToggleValueChanged()
    {
        Core.Game.Options.AreAnimationsEnabled = this.animationEnabledToggle.isOn;
    }

    public void OnSideScrollEnabledToggleValueChanged()
    {
        Core.Game.Options.IsMouseScreenEdgeScrollingEnabled = this.sideScrollingEnabledToggle.isOn;
    }

    public void OnMobileInterfaceValueChanged(Toggle t)
    {
        if (t.isOn)
        {
            TextMeshProUGUI text = t.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            //Core.Game.Options.MobileInterface = text.text;
        }
    }

    public void OnRestoreDefaultsClick()
    {
        Core.Game.PlayButtonSound();
        effectsVolumeSlider.value = 1f;
        ambienceVolumeSlider.value = 0.125f;
        backgroundVolumeSlider.value = 0.125f;
        moveSensivitySlider.value = 0.5f;
        zoomSensivitySlider.value = 0.5f;
        Core.Game.Options.AreAnimationsEnabled = true;
        Core.Game.Options.IsMouseScreenEdgeScrollingEnabled = true;
        //Core.Game.Options.MobileInterface = "Right";


        this.SetMobileInterfaceToggles();
    }

    public void SaveOptions()
    {
        Core.Game.SaveOptions();
    }

    private void UpdateValues()
    {
        if (Core.Game.Options != default)
        {
            if (this.effectsVolumeSlider.value != Core.Game.Options.EffectsVolume)
            {
                this.effectsVolumeSlider.value = Core.Game.Options.EffectsVolume;
            }

            if (this.ambienceVolumeSlider.value != Core.Game.Options.AmbienceVolume)
            {
                this.ambienceVolumeSlider.value = Core.Game.Options.AmbienceVolume;
            }

            if (this.backgroundVolumeSlider.value != Core.Game.Options.BackgroundVolume)
            {
                this.backgroundVolumeSlider.value = Core.Game.Options.BackgroundVolume;
            }

            if (this.moveSensivitySlider.value != Core.Game.Options.MoveSensivity)
            {
                this.moveSensivitySlider.value = Core.Game.Options.MoveSensivity;
            }

            if (this.zoomSensivitySlider.value != Core.Game.Options.ZoomSensivity)
            {
                this.zoomSensivitySlider.value = Core.Game.Options.ZoomSensivity;
            }

            if (this.animationEnabledToggle.isOn != Core.Game.Options.AreAnimationsEnabled)
            {
                this.animationEnabledToggle.isOn = Core.Game.Options.AreAnimationsEnabled;
            }

            if (this.sideScrollingEnabledToggle.isOn != Core.Game.Options.IsMouseScreenEdgeScrollingEnabled)
            {
                this.sideScrollingEnabledToggle.isOn = Core.Game.Options.IsMouseScreenEdgeScrollingEnabled;
            }
        }
    }
}
