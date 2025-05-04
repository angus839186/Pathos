using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterNPC : InteractableNPC
{
    public string targetSpawnPointID;
    public string targetScene;

    public void TeleportPlayer()
    {
        SpawnManager.spawnPointID = targetSpawnPointID;
        GameManager gameManager = GameManager.Instance;
        gameManager.StartCoroutine(gameManager.LoadNextScene(GetTargetSceneName()));
    }

    public string GetTargetSceneName()
    {
        return targetScene;
    }
}
