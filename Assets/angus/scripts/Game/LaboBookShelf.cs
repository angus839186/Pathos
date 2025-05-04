using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LaboBookShelf : InteractableNPC
{

    public Image photo;

    void OnEnable()
    {
        if(PlayerInputManager.Instance != null)
        {
            PlayerInputManager.Instance.ClosePhotoEvent += ClosePhoto;
            Debug.Log("AAA");
        }
    }

    void OnDisable()
    {
        if(PlayerInputManager.Instance != null)
        {
            PlayerInputManager.Instance.ClosePhotoEvent -= ClosePhoto;
            Debug.Log("BBB");
        }
        
    }

    public void OpenDisplayingPhoto(Sprite photoSprite)
    {
        StartCoroutine(DisplayingPhoto(photoSprite));
    }

    public IEnumerator DisplayingPhoto(Sprite photoSprite)
    {
        photo.gameObject.SetActive(true);
        photo.sprite = photoSprite;
        yield return new WaitForSeconds(0.5f);
        PlayerInputManager.Instance.SwitchActionMap("DisplayingPhoto");
    }

    public void ClosePhoto()
    {
        Debug.Log("Close photo");
        photo.sprite = null;
        photo.gameObject.SetActive(false);
        PlayerInputManager.Instance.SwitchActionMap("Player");
    }
}
