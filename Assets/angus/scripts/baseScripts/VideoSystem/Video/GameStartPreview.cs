using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartPreview : VideoBase
{
    void Start()
    {
        VideoController.Instance.OnVideoEnd += VideoEnd;
    }

    void OnDisable()
    {
        VideoController videoController = VideoController.Instance;
        if(videoController != null)
        {
            videoController.OnVideoEnd -= VideoEnd;
        }
    }

    public void VideoEnd()
    {
        StartGame();
    }
    public void StartGame()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.LoadLevel(gameManager.defaultScene);
        DataPersistenceManager.Instance.NewGame();
    }
}
