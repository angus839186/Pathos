using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackDisplay : InteractableNPC
{
    public string targetSpawnPointID;
    public string targetScene;
    public attackDisplay[] attacks;

    public void OpenAttack(attackDisplay _attack)
    {
        _attack.Attack();
    }
    public void CloseAllAttacks()
    {
        for (int i = 0; i < attacks.Length; i++)
        {
            attacks[i].StopAttack();
        }
    }
    public void TeleportPlayer()
    {
        SpawnManager.spawnPointID = targetSpawnPointID;
        GameManager gameManager = GameManager.Instance;
        gameManager.StartCoroutine(gameManager.LoadNextScene(targetScene));
    }
}
