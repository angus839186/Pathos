using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReversePlainPreview : VideoBase
{
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
        this.enabled = false;
    }
}
