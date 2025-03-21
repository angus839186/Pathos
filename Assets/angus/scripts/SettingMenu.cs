using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingMenu : MonoBehaviour
{
    public static SettingMenu Instance;
    [Header("UI 元件")]
    public Dropdown resolutionDropdown;
    public Toggle windowedToggle;
    public Slider brightnessSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Button applyButton;
    public Button cancelButton;

    [Header("其他參考")]
    public AudioMixer audioMixer;
    private Resolution[] resolutions = new Resolution[]
    {
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1366, height = 768 },
        new Resolution { width = 2560, height = 1440 },
        new Resolution { width = 1280, height = 720 }
    };

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    void OnDisable()
    {

    }

    void Start()
    {

        resolutionDropdown.ClearOptions();
        var options = new List<string>();
        foreach (var res in resolutions)
        {
            options.Add(res.width + "x" + res.height);
        }
        resolutionDropdown.AddOptions(options);


        applyButton.onClick.AddListener(OnApplyButtonClicked);
        cancelButton.onClick.AddListener(CancelSettings);
    }



    /// <summary>
    /// 按下套用按鈕後執行，更新遊戲設定
    /// </summary>
    void OnApplyButtonClicked()
    {

        SetResolution();
        SetMusicVolume();
        SetSFXVolume();

        gameObject.SetActive(false);
    }

    void SetResolution()
    {
        // 取得玩家選擇的解析度
        int selectedResIndex = resolutionDropdown.value;
        Resolution selectedRes = resolutions[selectedResIndex];

        // 取得視窗模式設定 (Toggle 為 true 表示視窗化)
        bool isWindowed = windowedToggle.isOn;
        // Screen.SetResolution 的第三個參數為 fullscreen，
        // 因此若 isWindowed 為 true 則 fullscreen 傳 false
        bool fullscreen = !isWindowed;

        // 設定解析度與視窗/全螢幕模式
        Screen.SetResolution(selectedRes.width, selectedRes.height, fullscreen);
    }

    /// <summary>
    /// 按下取消按鈕後執行，直接關閉設定 UI
    /// </summary>
    void CancelSettings()
    {
        gameObject.SetActive(false);
    }

    void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("musicVolume", volume);

    }
    void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("sfxVolume", volume);
    }
    void LoadVolume()
    {

    }


}
