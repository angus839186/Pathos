using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthPoint : MonoBehaviour
{
    private Animator anime;
    private Image image;
    private Sprite defaultSprite;

    void Awake()
    {
        anime = GetComponent<Animator>();
        image = GetComponent<Image>();
    }

    public void Break()
    {
        anime.Play("HP_breakage");
    }
    public void Recover()
    {
        image.sprite = defaultSprite;
    }
}
