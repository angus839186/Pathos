using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionText : MonoBehaviour
{

    public static DescriptionText Instance { get; private set; }
    public Text descriptionText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public IEnumerator showDescription(string description)
    {
        this.gameObject.SetActive(true);
        descriptionText.text = description;
        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(false);
    }
}
