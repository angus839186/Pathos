using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public string currentScene;
    public Vector3 playerPosition;
    public float gameTime;

    public SerializableDictionary<string, bool> treesFalled;

    public GameData()
    {
        currentScene = "";
        playerPosition = Vector3.zero;
        gameTime = 0f;
        treesFalled = new SerializableDictionary<string, bool>();
    }
}

