using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{

    [SerializeField] private Sprite npcIcon;
    [SerializeField] [TextArea] private string[] dialogue;
    [SerializeField] private bool[] playertalk;
    [SerializeField] private Response[] responses;

    public Sprite NpcIcon => npcIcon;

    public string[] Dialogue => dialogue;

    public bool[] PlayerTalk => playertalk;

    public bool HasResponses => Responses != null && Responses.Length > 0;
    
    public Response[] Responses => responses;
}