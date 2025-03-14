using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class teleporter : MonoBehaviour
{
    public bool canTeleport;
    public string targetSpawnPointID;
    public string targetScene;

    public void TeleportPlayer() {
        Debug.Log("Teleport");
        SpawnManager.spawnPointID = targetSpawnPointID;
        Debug.Log(SpawnManager.spawnPointID);

        GameManager gameManager = GameManager.Instance;
        gameManager.StartCoroutine(gameManager.LoadNextScene(targetScene));

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            DataPersistenceManager.Instance.SaveGameData();
            TeleportPlayer();
        }
    }
}
