using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    public string profileId = "";

    public string sceneName = "";

    public string gameTime = "";
    public Image sceneImage;

    public string GetProfileId()
    {
        return this.profileId;
    }


    public void SetData(GameData data)
    {
        if (data == null)
        {
            this.sceneName = "場景: 空";
            this.gameTime = "遊玩時間: 0秒";
            return;
        }
        this.sceneName = "場景: " + data.currentScene;
        this.gameTime = "遊玩時間: " + data.gameTime.ToString() + "秒";
    }
}
