using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generalTeleporter : MonoBehaviour, ITeleport
{
    public string targetSpawnPointID;
    public string targetScene;

    public string GetTargetSceneName()
    {
        return targetScene;
    }

    public void TeleportPlayer()
    {
        SpawnManager.spawnPointID = targetSpawnPointID;
        GameManager gameManager = GameManager.Instance;
        gameManager.StartCoroutine(gameManager.LoadNextScene(GetTargetSceneName()));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DataPersistenceManager.Instance.SaveGameData();
            TeleportPlayer();
        }
    }
}
