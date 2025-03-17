using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthPoint : MonoBehaviour
{
    private Animator anime;
    private Image image;
    public Sprite defaultSprite;

    void Awake()
    {
        anime = GetComponent<Animator>();
        image = GetComponent<Image>();
    }

    public void Break()
    {
        anime.SetBool("break", true);
    }
    public void Recover()
    {
        anime.SetBool("break", false);
        image.sprite = defaultSprite;
    }
}
