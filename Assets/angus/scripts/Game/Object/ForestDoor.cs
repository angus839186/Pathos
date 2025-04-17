using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ForestDoor : InteractableObject, IDataPersistence
{
    public Animator anime;

    public AudioClip sound;

    public bool ForestDoorOpen;

    [Header("結束影片")]
    public AudioSource BGM;

    public VideoClip endClip;

    public List<double> pausePoints;

    public bool ToBeContinued;

    public GameObject ToBeContinueObject;

    void OnDisable()
    {
        VideoController video = VideoController.Instance;
        if (video != null)
        {
            VideoController.Instance.OnVideoEnd -= ForestDoorEnd;
        }
    }

    void Start()
    {
        anime = GetComponent<Animator>();
    }
    public override string GetAnimationTrigger(Item heldItem)
    {
        return base.GetAnimationTrigger(null);
    }

    public override string GetDescription(Item heldItem)
    {
        return base.GetDescription(null);
    }

    public override void Interact()
    {
        Debug.Log("Go To Forest");
        if(ForestDoorOpen)
        {
            PlayForestDoorEndClip();
        }

    }

    public void ForestDoorEnd()
    {
        if (ToBeContinued)
        {
            PlayerInputManager.Instance.BackToMenuEvent += BackToMenu;
            PlayerInputManager.Instance.SwitchActionMap("ToBeContinued");
            ToBeContinueObject.SetActive(true);
        }
        else
        {
            //Do Nothing
        }
    }

    public void BackToMenu()
    {
        GameManager.Instance.BackToMenu();
    }

    public void PlayForestDoorEndClip()
    {
        BGM.Pause();
        VideoController.Instance.OnVideoEnd += ForestDoorEnd;
        VideoController.Instance.GetVideo(endClip, pausePoints);
    }

    public override void InteractEvent(Item heldItem)
    {
        //Do Nothing
    }
    public void Open()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        anime.SetTrigger("OpenForestDoor");
        collider.enabled = true;
        ForestDoorOpen = true;
        AudioManager.instance.PlaySound(sound);
        
    }

    public void LoadData(GameData data)
    {
        ForestDoorOpen = data.ForestDoorOpen;
        if (ForestDoorOpen)
        {
            Open();
        }
        else
        {
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.enabled = false;
        }
    }

    public void SaveData(ref GameData data)
    {
        data.ForestDoorOpen = ForestDoorOpen;
    }
}
