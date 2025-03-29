using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData 
{
    public string currentScene;
    public Vector3 playerPosition;
    public float gameTime;
    public SerializableDictionary<string, bool> treesFalled;

    public SerializableDictionary<string, bool> birdsFlied;

    public SerializableDictionary<string, bool> bridsOnFence;
    public List<string> inventoryItemNames;

    public bool windmillWorked;

    public bool hint1;
    public bool hint2;

    public GameData()
    {
        currentScene = "";
        playerPosition = Vector3.zero;
        gameTime = 0f;
        windmillWorked = false;
        hint1 = false;
        hint2 = false;
        treesFalled = new SerializableDictionary<string, bool>();
        birdsFlied = new SerializableDictionary<string, bool>();
        bridsOnFence = new SerializableDictionary<string, bool>();
        inventoryItemNames = new List<string>();
    }
}

