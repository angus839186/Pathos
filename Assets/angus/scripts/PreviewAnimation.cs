using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PreviewAnimation : MonoBehaviour
{
    public VideoPlayer video;
    public GameObject previewVideoCanvas;
    void Start()
    {
        StartCoroutine(PlayPreviewVideo());
    }

    IEnumerator PlayPreviewVideo()
    {
        video.Play();
        yield return new WaitForSeconds((float)video.clip.length);
        previewVideoCanvas.SetActive(false);
        ReversePlainBoss boss = FindObjectOfType<ReversePlainBoss>();
        boss.StartAttack();
    }
}
