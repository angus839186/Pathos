using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{

    public static DialogueUI Instance;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;

    public bool IsOpen { get; private set; }
    private ResponseHandler responseHandler;

    private TypeWriterEffect typewriterEffect;
    private PlayerInputManager playerInput;

    public Image PlayerImage;

    public Image NpcImage;

    private bool Continue;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playerInput = PlayerInputManager.Instance;
        if (playerInput != null)
        {
            playerInput.OnNextDialogueEvent += TriggerNextDialogue;
        }
        typewriterEffect = GetComponent<TypeWriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();

        CloseDialogueBox();
    }

    void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.OnNextDialogueEvent -= TriggerNextDialogue;
        }
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        IsOpen = true;
        dialogueBox.SetActive(true);
        NpcImage.sprite = dialogueObject.NpcIcon;
        PlayerInputManager.Instance.SwitchActionMap("Dialogue");
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        responseHandler.AddResponseEvents(responseEvents);
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];
            bool playerTalk = dialogueObject.PlayerTalk[i];
            if (playerTalk)
            {
                SetTalkerImageAlpha(PlayerImage, 1f);
                SetTalkerImageAlpha(NpcImage, 0.5f);
            }
            else
            {
                SetTalkerImageAlpha(PlayerImage, 0.5f);
                SetTalkerImageAlpha(NpcImage, 1f);
            }
            yield return RunTypingEffect(dialogue);

            textLabel.text = dialogue;
            yield return null;
            yield return new WaitUntil(() => Continue);
        }
        Continue = false;
        if (dialogueObject.HasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            yield return new WaitUntil(() => Continue);
            CloseDialogueBox();
        }
        Continue = false;
    }

    private void SetTalkerImageAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        typewriterEffect.Run(dialogue, textLabel);
        while (typewriterEffect.IsRunning)
        {
            yield return null;
        }
    }
    public void CloseDialogueBox()
    {
        IsOpen = false;
        Continue = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
        PlayerInputManager.Instance.SwitchActionMap("Player");
    }

    public void TriggerNextDialogue()
    {
        Debug.Log("nextDialogue");
        Continue = true;
    }
}
