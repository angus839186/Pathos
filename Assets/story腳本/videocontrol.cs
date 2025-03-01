using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class videocontrol : MonoBehaviour
{
    public VideoPlayer vp;
    public VideoClip[] clips;
    private int currentVideo = 0;
    // Start is called before the first frame update
    void Start()
    {
        vp.clip = clips[currentVideo];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextVideo();
        }
    }

    public void PlayVideo()
    {
        vp.Play();
    }

    public void StopVideo()
    {
        vp.Stop();
    }
    public void NextVideo()
    {
        currentVideo++;
        if (currentVideo>11)
        {
            
        }
        vp.clip = clips[currentVideo];
        vp.Play();

    }

}
