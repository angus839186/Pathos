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
        gameManager.StartCoroutine(gameManager.LoadGameScene(targetScene));

    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && canTeleport) {
            TeleportPlayer();
        }
    }
}
