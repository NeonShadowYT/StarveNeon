using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManagerm : MonoBehaviour
{
    public float _currentSensitivity;
    private int _currentResolutionIndex;
    public int multiplaer;
    public bool PC;
    public bool Menu;
    public Monetizationtest Monetizationtest;

    #region Настройки

    [Header("Настройки Меню")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private TMP_Dropdown _resolutionDropdown, _qualityDropdown;
    [SerializeField] private Toggle _toggle;
    private Resolution[] _resolutions;
    public GameObject EffectRunButtonOn, EffectRunButtonOff, TurnButtonOn, TurnButtonOff;

    [Header("Сложность")]
    public int Complicated = 1;

    [Header("Эфект бега")]
    private int noRunEffectNumbers;

    [Header("Переставление ног на месте")]
    private int TurnNumbers;
    #endregion

    // max x value -1.841574
    // min x value -0.136
    private void Awake()
    {
        if (Menu == true)
        {
            PopulateResolutionDropdown();
            LoadSettings(_currentResolutionIndex);
        }
        if (PC == false) Monetizationtest.enabled = true;
    }
    #region Settings Funcs
    private void PopulateResolutionDropdown()
    {
        if (Menu == true && PC == true)
        {
            _resolutionDropdown.ClearOptions();
            List<string> options = new List<string>();
            _resolutions = Screen.resolutions;
            _currentResolutionIndex = 0;
            for (int i = 0; i < _resolutions.Length; i++)
            {
                string option = _resolutions[i].width + "x" + _resolutions[i].height;
                options.Add(option);
                if (_resolutions[i].width == Screen.currentResolution.width &&
                    _resolutions[i].height == Screen.currentResolution.height)
                    _currentResolutionIndex = i;
            }
            _resolutionDropdown.AddOptions(options);
            _resolutionDropdown.RefreshShownValue();
        }
    }
    public void SetFullscreen(bool isFullscreen)
    {
        if (Menu == true) Screen.fullScreen = isFullscreen;
    }
    public void SetResolution(int resolutionIndex)
    {
        if (Menu == true && PC == true)
        {
            Resolution resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
    }
    public void SetQuality(int qualityIndex)
    {
        if (Menu == true) QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetSensitivity(float desiredSensitivity)
    {
        PlayerPrefs.SetFloat("Sensitivity", desiredSensitivity);
        _currentSensitivity = desiredSensitivity;
    }
    public void SetVolume(float volume)
    {
        if (Menu == true) _audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }
    public void Sound()
    {
        if (Menu == true) AudioListener.pause = !AudioListener.pause;
    }
    public void SaveSettings()
    {
        if (Menu == true)
        {
            PlayerPrefs.SetInt("QualitySettings", _qualityDropdown.value);
            if (PC == true) PlayerPrefs.SetInt("Resolution", _resolutionDropdown.value);
            PlayerPrefs.SetInt("Fullscreen", System.Convert.ToInt32(Screen.fullScreen));
            PlayerPrefs.SetFloat("Sensitivity", _currentSensitivity);
            PlayerPrefs.SetInt("Complicated", Complicated);
            PlayerPrefs.SetInt("RunEffectSettings", noRunEffectNumbers);
            PlayerPrefs.SetInt("TurnSettings", TurnNumbers);
        }
    }
    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("QualitySettings")) _qualityDropdown.value = PlayerPrefs.GetInt("QualitySettings"); else _qualityDropdown.value = 3;
        SetQuality(_qualityDropdown.value);

        if (PC == true)
        {
            if (PlayerPrefs.HasKey("Resolution")) _resolutionDropdown.value = PlayerPrefs.GetInt("Resolution"); else _resolutionDropdown.value = currentResolutionIndex;
            SetResolution(_resolutionDropdown.value);
        }

        if (PlayerPrefs.HasKey("Sensitivity")) _currentSensitivity = PlayerPrefs.GetFloat("Sensitivity"); else _currentSensitivity = 1f;
        SetSensitivity(_currentSensitivity);

        if (PlayerPrefs.HasKey("Fullscreen")) Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen")); else Screen.fullScreen = true;
        SetFullscreen(Screen.fullScreen);
        _toggle.isOn = Screen.fullScreen;

        if (PlayerPrefs.HasKey("RunEffectSettings")) noRunEffectNumbers = PlayerPrefs.GetInt("RunEffectSettings"); else noRunEffectNumbers = 0; // 1 всё не работает, 0 всё работает
        if (PlayerPrefs.HasKey("TurnSettings")) TurnNumbers = PlayerPrefs.GetInt("TurnSettings"); else TurnNumbers = 0; // 1 всё не работает, 0 всё работает
        if (Menu == true)
        {
            if (noRunEffectNumbers == 1) EffectRunButtonOn.SetActive(true); else EffectRunButtonOff.SetActive(true);
            if (TurnNumbers == 1) TurnButtonOn.SetActive(true); else TurnButtonOff.SetActive(true);
        }
    }
    #endregion
    public void Quit() => Application.Quit();
    public void ComplexityIZI() => PlayerPrefs.SetInt("Complicated", 0);
    public void ComplexityNormal() => PlayerPrefs.SetInt("Complicated", 1);
    public void ComplexityHard() => PlayerPrefs.SetInt("Complicated", 2);
    public void ComplexityUltraHard() => PlayerPrefs.SetInt("Complicated", 3);
    public void ComplexityRou() => PlayerPrefs.SetInt("Complicated", 4);
    public void ComplexityNeonStand() => PlayerPrefs.SetInt("Complicated", 5);
    public void noRunEffectNumbersOn() => PlayerPrefs.SetInt("RunEffectSettings", 0);
    public void noRunEffectNumbersOff() => PlayerPrefs.SetInt("RunEffectSettings", 1);
    public void turnControllerOn() => PlayerPrefs.SetInt("TurnSettings", 0);
    public void turnControllerOff() => PlayerPrefs.SetInt("TurnSettings", 1);
    public void multiplaerOn() => PlayerPrefs.SetInt("Multiplayer", 1);
    public void multiplaerOff() => PlayerPrefs.SetInt("Multiplayer", 0);
}
