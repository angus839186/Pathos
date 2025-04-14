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

    public Sprite DefaultSprite;

    public GameData gameData;

    public int index;

    public string GetProfileId()
    {
        return this.profileId;
    }


    public void SetData(GameData data)
    {
        if (data == null)
        {
            this.sceneImage.sprite = DefaultSprite;
            gameData = null;
            this.sceneName = "場景: 空";
            this.gameTime = "遊玩時間: 0秒";
            return;
        }
        gameData = data;
        this.sceneName = "場景: " + data.currentScene;
        this.gameTime = "遊玩時間: " + CalculateGameTime(data.gameTime);
        if(SaveFileMenu.Instance.sceneSprites.ContainsKey(data.currentScene))
        {
            this.sceneImage.sprite = SaveFileMenu.Instance.sceneSprites[data.currentScene];
        }
        else
        {
            this.sceneImage.sprite = DefaultSprite;
        }
    }

    public string CalculateGameTime(float gameTime)
    {
        // 將累計的時間轉換成整數秒數
        int totalSeconds = Mathf.FloorToInt(gameTime);

        // 計算時、分、秒
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;


        return string.Format("{0:D2}時:{1:D2}分:{2:D2}秒", hours, minutes, seconds);
    }
}
