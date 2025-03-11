using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PreviewAnimation : MonoBehaviour
{
    public VideoPlayer video;
    public GameObject previewVideoCanvas;

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
        Debug.Log("Completed");
        StartCoroutine(PlayPreviewVideo());
    }

    IEnumerator PlayPreviewVideo()
    {
        yield return new WaitForSeconds((float)video.clip.length);
        previewVideoCanvas.SetActive(false);
        ReversePlainBoss boss = FindObjectOfType<ReversePlainBoss>();
        boss.StartAttack();
    }
}
