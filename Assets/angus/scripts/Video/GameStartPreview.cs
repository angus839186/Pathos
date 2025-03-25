using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartPreview : VideoBase
{
    void Start()
    {
        VideoController.Instance.OnVideoEnd += VideoEnd;
    }

    public void VideoEnd()
    {
        played = true;
        PlayerInputManager.Instance.SwitchActionMap("MainMenu");
    }
}
