using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint2 : MonoBehaviour, IDataPersistence
{
    bool passed = false;
    public GameObject hint;

    public Item item;

    void Start()
    {
        hint.SetActive(false);
    }

    void OnEnable()
    {
        InventoryManager.Instance.OnInventoryChanged += ShowHint;
    }

    void OnDisable()
    {
        InventoryManager.Instance.OnInventoryChanged -= ShowHint;
    }

    void ShowHint()
    {
        if (!passed && InventoryManager.Instance.items.Exists(x => x.item.itemName == item.itemName))
        {
            StartCoroutine(HideHintAfterTime());
            passed = true;
        }
    }

    IEnumerator HideHintAfterTime()
    {
        hint.SetActive(true);
        yield return new WaitForSeconds(5f);
        hint.SetActive(false);
    }
    public void LoadData(GameData data)
    {
        passed = data.hint2;
        if(passed == false)
        {
            this.enabled = true;
            hint.SetActive(false);
            return;
        }
        else
        {
            hint.SetActive(false);
            this.enabled = false;
        }

    }

    public void SaveData(ref GameData data)
    {
        data.hint2 = passed;
    }
}
