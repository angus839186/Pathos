using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public string lastScene;

    public Vector3 playerPosition;

    public GameData()
    {
        this.lastScene = "";
        playerPosition = Vector3.zero;
    }
}
