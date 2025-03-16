using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Preview : MonoBehaviour
{
    public VideoPlayer video;
    public GameObject previewVideoCanvas;

    public GameObject tutorial;

    void Awake()
    {
        video.Play();
    }
    void OnEnable()
    {
        video.prepareCompleted += OnPreviewVideoPrepared;
    }

    void OnDisable()
    {
        video.prepareCompleted -= OnPreviewVideoPrepared;
    }

    public void OnPreviewVideoPrepared(VideoPlayer source)
    {
        StartCoroutine(PlayPreviewVideo());
    }

    IEnumerator PlayPreviewVideo()
    {
        PlayerInputManager.Instance.SwitchActionMap("Preview");
        yield return new WaitForSeconds((float)video.clip.length);
        PlayerInputManager.Instance.SwitchActionMap("Tutorial");

        previewVideoCanvas.SetActive(false);
        Tutorial tutorial = FindObjectOfType<Tutorial>();
        tutorial.InitializeTutorial();
    }
}
