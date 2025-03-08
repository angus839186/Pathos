using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class teleporter : MonoBehaviour
{
    public string nextSceneName;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager gameManager = GameManager.Instance;
            gameManager.StartCoroutine(gameManager.LoadGameScene(nextSceneName));
            Debug.Log("GoNextScene");
        }
    }
}
