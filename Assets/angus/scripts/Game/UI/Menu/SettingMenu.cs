using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingMenu : Menu
{

    #region 參數
    public static SettingMenu Instance;
    public CanvasGroup settingCanvas;
    [Header("UI 元件")]
    public Dropdown resolutionDropdown;
    public Toggle windowedToggle;

    public Slider musicSlider;

    public Button ToggleMusicButton;
    public Text musicVolumeText;

    public Slider sfxSlider;

    public Button ToggleSFXButton;
    public Text sfxVolumeText;

    public Button applyButton;
    public Button cancelButton;

    public Sprite muteSprite;
    public Sprite volumeSprite;

    [Header("其他參考")]
    public AudioMixer audioMixer;

    public bool musicMuted = false;

    private bool sfxMuted = false;
    public float previousMusicVolume;
    private float previousSfxVolume;

    public RenderTexture VideoRenderer;

    #endregion
    private Resolution[] resolutions = new Resolution[]
    {
        new Resolution { width = 1280, height = 720 },
        new Resolution { width = 1366, height = 768 },
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 2560, height = 1440 }
    };

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        canvasGroup = this.GetComponent<CanvasGroup>();
        DontDestroyOnLoad(gameObject);
    }

    public CanvasGroup SettingMenuCanvasGroup()
    {
        return settingCanvas;
    }

    void OnEnable()
    {
        PlayerInputManager.Instance.OnToggleSettingMenuEvent += ToggleSettingMenu;
    }


    void OnDisable()
    {
        PlayerInputManager.Instance.OnToggleSettingMenuEvent -= ToggleSettingMenu;
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

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }

    }

    public void ToggleSettingMenu(bool toggle)
    {
        if (toggle == false)
        {
            CloseSettingMenu();
        }
        else
        {
            OpenSettingMenu();
        }
    }
    public void OnApplyButtonClicked()
    {

        SetResolution();
        SetMusicVolume();
        SetSFXVolume();
        ToggleSettingMenu(false);
    }

    public void OnCancelButtonClicked()
    {
        ToggleSettingMenu(false);
    }
    public void CloseSettingMenu()
    {
        if (SceneManager.GetActiveScene().name == GameManager.Instance.menuScene)
        {
            Transition(mainMenuCanva);
            PlayerInputManager.Instance.SwitchActionMap("MainMenu");
        }
        else
        {
            settingCanvas.alpha = false ? 1f : 0f;
            settingCanvas.interactable = false;
            settingCanvas.blocksRaycasts = false;
            PlayerInputManager.Instance.SwitchActionMap("PauseMenu");
        }
    }

    public void OpenSettingMenu()
    {
        if (SceneManager.GetActiveScene().name == GameManager.Instance.menuScene)
        {
            Transition(mainMenuCanva);
        }
        else
        {
            settingCanvas.alpha = 1f;
            settingCanvas.interactable = true;
            settingCanvas.blocksRaycasts = true;
        }
        PlayerInputManager.Instance.SwitchActionMap("SettingMenu");
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

        VideoRenderer.Release();
        VideoRenderer.width = selectedRes.width;
        VideoRenderer.height = selectedRes.height;
        VideoRenderer.Create();
    }
    public void SetMusicText()
    {
        float value = musicSlider.value;
        // 將 value 從 0.0001~1 轉換到 0~1 的範圍
        float normalizedValue = Mathf.InverseLerp(0.0001f, 1f, value);
        int percent = Mathf.RoundToInt(normalizedValue * 100);
        musicVolumeText.text = percent.ToString() + "%";
        if(value > 0.0001f)
        {
            ToggleMusicButton.image.sprite = volumeSprite;
        }
        else
        {
            ToggleMusicButton.image.sprite = muteSprite;
        }
    }

    public void SetSFXText()
    {
        float value = sfxSlider.value;
        // 將 value 從 0.0001~1 轉換到 0~1 的範圍
        float normalizedValue = Mathf.InverseLerp(0.0001f, 1f, value);
        int percent = Mathf.RoundToInt(normalizedValue * 100);
        sfxVolumeText.text = percent.ToString() + "%";
        if(value > 0.0001f)
        {
            ToggleSFXButton.image.sprite = volumeSprite;
        }
        else
        {
            ToggleSFXButton.image.sprite = muteSprite;
        }
    }

    void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }
    void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetMusicVolume();
        SetSFXVolume();

    }

    public void ToggleMusicVolume()
    {
        musicMuted = !musicMuted;
        if (musicMuted)
        {
            previousMusicVolume = musicSlider.value;
            musicSlider.value = 0.0001f;
        }
        else
        {
            musicSlider.value = previousMusicVolume;
        }
    }

    public void ToggleSFXVolume()
    {
        sfxMuted = !sfxMuted;
        if (sfxMuted)
        {
            previousSfxVolume = sfxSlider.value;
            sfxSlider.value = 0.0001f;
        }
        else
        {
            sfxSlider.value = previousSfxVolume;
        }
    }


}
