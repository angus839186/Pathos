using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

public class VideoController : MonoBehaviour
{

    public static VideoController Instance;

    public VideoPlayer video;
    public Button continueButton;

    public List<double> pausePoints = new List<double>();

    public int currentPauseIndex = 0;
    public bool isPausedBySystem = false;

    public bool isPlayingVideo;

    public event Action OnVideoEnd;

    public CanvasGroup videoCanvas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        video = GetComponent<VideoPlayer>();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GetPreviewVideo();
    }

    void Start()
    {
        PlayerInputManager.Instance.OnContinueVideoEvent += ContinueVideo;
        SceneManager.sceneLoaded += OnSceneLoaded;
        continueButton.onClick.AddListener(ContinueVideo);
        continueButton.gameObject.SetActive(false);

        GetPreviewVideo();
    }

    public void PlayVideo(VideoClip _video)
    {
        video.clip = _video;
        video.frame = 0;
        video.Play();
        isPausedBySystem = false;
        ToggleVideoCanvas(true);
        PlayerInputManager.Instance.SwitchActionMap("PlayingVideo");

        StartCoroutine(PlayingVideo());
    }

    void ToggleVideoCanvas(bool toggle)
    {
        videoCanvas.alpha = toggle ? 1f : 0f;
        videoCanvas.blocksRaycasts = toggle;
        videoCanvas.interactable = toggle;
        isPlayingVideo = toggle;
    }

    void ContinueVideo()
    {
        currentPauseIndex++;
        isPausedBySystem = false;
        continueButton.gameObject.SetActive(false);
        video.Play();
    }

    void GetPreviewVideo()
    {
        VideoBase _Video = FindObjectOfType<VideoBase>();
        if (_Video == null) return;
        if (!_Video.played)
        {
            // 有設定暫停點就使用，否則清空暫停點
            if (_Video.pausePoints != null && _Video.pausePoints.Count > 0)
            {
                SetPausePoint(_Video.pausePoints);
            }
            else
            {
                SetPausePoint(null); // 清空暫停點
            }
            PlayVideo(_Video.clip);
        }
        else
        {
            return;
        }
    }
    public void GetVideo(VideoClip clip, List<double> points)
    {
        if(points != null && points.Count > 0)
        {
            SetPausePoint(points);
        }
        else
        {
            SetPausePoint(null);
        }
        PlayVideo(clip);
    }

    public void SetPausePoint(List<double> points)
    {
        if (points != null)
        {
            pausePoints = points;
        }
        else
        {
            pausePoints = new List<double>();
        }

        currentPauseIndex = 0;
    }

    public IEnumerator PlayingVideo()
{
    while (isPlayingVideo)
    {
        if (currentPauseIndex < pausePoints.Count)
        {
            // 等待直到影片時間達到下一個暫停點
            yield return new WaitUntil(() => video.time >= pausePoints[currentPauseIndex]);

            video.Pause();
            isPausedBySystem = true;
            continueButton.gameObject.SetActive(true);

            // ✅ 現在只等待使用者點按鈕（ContinueVideo 會把 isPausedBySystem 設回 false）
            yield return new WaitUntil(() => !isPausedBySystem);
        }
        else
        {
            // 沒有更多暫停點了，等待影片播放完
            yield return new WaitUntil(() => !video.isPlaying);

            ToggleVideoCanvas(false);
            OnVideoEnd?.Invoke();
        }

        yield return null;
    }
}

}
