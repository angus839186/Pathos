using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTeleporter : InteractableTeleporter
{
    public string targetSpawnPointID;
    public string targetScene;
    public override void Interact()
    {
        DataPersistenceManager.Instance.SaveGameData();
        TeleportPlayer();
    }

    public override void InteractEvent(Item heldItem)
    {
        //Do nothing
    }

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
