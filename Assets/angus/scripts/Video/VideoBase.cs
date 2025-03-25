using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoBase : MonoBehaviour
{
    [SerializeField]
    public VideoClip clip;


    [SerializeField]
    public List<double> pausePoints;

    [SerializeField]
    public bool played;
}
