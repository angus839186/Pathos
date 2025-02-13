using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionText : MonoBehaviour
{
    public GameObject descriptionObject;
    public Text descriptionText;

    public IEnumerator showDescription(string description)
    {
        descriptionObject.SetActive(true);
        descriptionText.text = description;
        yield return new WaitForSeconds(1.5f);
        descriptionObject.SetActive(false);
    }
}
