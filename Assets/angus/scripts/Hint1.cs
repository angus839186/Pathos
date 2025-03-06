using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint1 : MonoBehaviour, IDataPersistence
{
    bool passed = false;
    public GameObject hint;

    void Start()
    {
        hint.SetActive(true);
    }
    public void LoadData(GameData data)
    {
        passed = data.hint1;
        if(passed == false)
        {
            this.gameObject.SetActive(true);
            hint.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
            hint.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.hint1 = passed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            hint.SetActive(false);
            passed = true;
        }
    }
}
