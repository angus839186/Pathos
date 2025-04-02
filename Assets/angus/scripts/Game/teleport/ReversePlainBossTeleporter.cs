using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReversePlainBossTeleporter : MonoBehaviour, ITeleport
{
    public string targetSpawnPointID;
    public string BossScene;
    public string PlainScene;
    public string GetTargetSceneName()
    {
        string newScene = GameManager.Instance.reversePlainBossWin? PlainScene: BossScene;
        return newScene;
    }
    public virtual void TeleportPlayer()
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
