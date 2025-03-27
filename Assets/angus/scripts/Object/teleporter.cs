using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class teleporter : MonoBehaviour
{
    public bool canTeleport = true;
    public string targetSpawnPointID;
    public string targetScene;

    public void TeleportPlayer()
    {
        SpawnManager.spawnPointID = targetSpawnPointID;
        GameManager gameManager = GameManager.Instance;
        gameManager.StartCoroutine(gameManager.LoadNextScene(targetScene));

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (canTeleport)
            {
                DataPersistenceManager.Instance.SaveGameData();
                TeleportPlayer();
            }
        }
    }
}
