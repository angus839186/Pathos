using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReversePlainBossTeleporter : MonoBehaviour, ITeleport, IDataPersistence
{
    public string targetSpawnPointID;
    public string BossScene;
    public string PlainScene;

    private string nextScene;
    public string GetTargetSceneName()
    {
        return null;
    }

    public void LoadData(GameData data)
    {
        bool BossDied = data.reversePlainBossDied;
        if(BossDied)
        {
            nextScene = PlainScene;
        }
        else
        {
            nextScene = BossScene;
        }
    }

    public void SaveData(ref GameData data)
    {
        //Do nothing
    }

    public virtual void TeleportPlayer()
    {
        SpawnManager.spawnPointID = targetSpawnPointID;
        GameManager gameManager = GameManager.Instance;
        gameManager.StartCoroutine(gameManager.LoadNextScene(nextScene));
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
