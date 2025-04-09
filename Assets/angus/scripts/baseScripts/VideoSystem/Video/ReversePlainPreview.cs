using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ReversePlainPreview : VideoBase, IDataPersistence
{
    public AudioSource audioSource;
    void Start()
    {
        VideoController video = VideoController.Instance;
        if (video != null)
        {
            VideoController.Instance.OnVideoEnd += VideoEnd;
        }
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
        played = true;
        audioSource.Play();
        VideoController video = VideoController.Instance;
        if (video != null)
        {
            VideoController.Instance.OnVideoEnd -= VideoEnd;
        }
    }

    public void LoadData(GameData data)
    {
        played = data.reversePlainPreviewVideoPlayed;
    }

    public void SaveData(ref GameData data)
    {
        data.reversePlainPreviewVideoPlayed = played;
    }
}
