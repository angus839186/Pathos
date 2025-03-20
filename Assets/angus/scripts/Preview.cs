using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Preview : MonoBehaviour
{
    public VideoPlayer video;
    public GameObject previewVideoCanvas;

    public GameObject tutorial;

    public bool played;

    void Awake()
    {
        if(!played)
        {
            StartCoroutine(PlayPrviewvideo());
            previewVideoCanvas.SetActive(true);
        }
        else
        {
            previewVideoCanvas.SetActive(false);
            StartTutorial();
        }
    }

    IEnumerator PlayPrviewvideo()
    {
        video.Play();
        PlayerInputManager.Instance.SwitchActionMap("Preview");
        yield return new WaitForSeconds((float)video.clip.length);
        previewVideoCanvas.SetActive(false);
        StartTutorial();
    }
    void StartTutorial()
    {
        Tutorial tutorial = FindObjectOfType<Tutorial>();
        tutorial.InitializeTutorial();
    }
}
