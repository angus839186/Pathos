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



    void Start()
    {
        PlayerInputManager.Instance.OnContinueVideoEvent += ContinueVideo;
        GameManager.Instance.OnSceneLoaded += GetScenePreviewVideo;
        continueButton.onClick.AddListener(ContinueVideo);
        continueButton.gameObject.SetActive(false);

        ToggleVideoCanvas(false);
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
        if(video.isPlaying) return;
        currentPauseIndex++;
        isPausedBySystem = false;
        continueButton.gameObject.SetActive(false);
        video.Play();
    }

    void GetScenePreviewVideo()
    {
        VideoBase _Video = FindObjectOfType<VideoBase>();
        if (_Video == null) return;
        if (_Video.played)
        {
            OnVideoEnd.Invoke();
            return;
        }
        else
        {
            if (_Video.pausePoints != null && _Video.pausePoints.Count > 0)
            {
                SetPausePoint(_Video.pausePoints);
            }
            else
            {
                SetPausePoint(null);
            }
            PlayVideo(_Video.clip);
        }
    }
    public void GetVideo(VideoClip clip, List<double> points)
    {
        if (points != null && points.Count > 0)
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
        // 確保影片開始播放
        yield return new WaitUntil(() => video.isPlaying);
        Debug.Log(video.clip.length);

        // 若無暫停點，直接等待影片結束
        if (pausePoints == null || pausePoints.Count == 0)
        {
            Debug.Log(video.time);
            yield return new WaitUntil(() => video.time >= video.clip.length - 0.1f);
            ToggleVideoCanvas(false);
            OnVideoEnd?.Invoke();
            yield break;
        }

        // 有暫停點的情況
        while (isPlayingVideo)
        {
            if (currentPauseIndex < pausePoints.Count)
            {
                yield return new WaitUntil(() => video.time >= pausePoints[currentPauseIndex]);

                video.Pause();
                isPausedBySystem = true;
                continueButton.gameObject.SetActive(true);

                yield return new WaitUntil(() => !isPausedBySystem);
            }
            else
            {
                yield return new WaitUntil(() => video.time >= video.clip.length - 0.1f);
                ToggleVideoCanvas(false);
                OnVideoEnd?.Invoke();
                break;
            }

            yield return null;
        }
    }

}
