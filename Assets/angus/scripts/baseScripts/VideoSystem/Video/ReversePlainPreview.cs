using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ReversePlainPreview : VideoBase
{
    public AudioSource audioSource;
    void Start()
    {
        VideoController.Instance.OnVideoEnd += VideoEnd;
    }

    void OnDisable()
    {
        VideoController video = VideoController.Instance;
        if (video != null)
        {
            VideoController.Instance.OnVideoEnd -= VideoEnd;
        }
    }

    public void VideoEnd()
    {
        BossTutorial tutorial = FindObjectOfType<BossTutorial>();
        tutorial.InitializeTutorial();
        audioSource.Play();
    }
}
