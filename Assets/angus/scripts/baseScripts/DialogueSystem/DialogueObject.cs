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
    private void OnValidate()
    {
        if (dialogue != null)
        {
            // 如果 playertalk 為 null 或其長度不等於 dialogue 長度，則重置 playertalk 陣列
            if (playertalk == null || playertalk.Length != dialogue.Length)
            {
                bool[] newPlayerTalk = new bool[dialogue.Length];
                if (playertalk != null)
                {
                    // 儘可能保留原有值（只保留對應範圍內的數據）
                    for (int i = 0; i < Mathf.Min(dialogue.Length, playertalk.Length); i++)
                    {
                        newPlayerTalk[i] = playertalk[i];
                    }
                }
                playertalk = newPlayerTalk;
            }
        }
    }
}