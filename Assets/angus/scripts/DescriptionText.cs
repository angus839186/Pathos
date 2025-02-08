using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionText : MonoBehaviour
{
    public Text descriptionText;
    public void showDescription(string description)
    {
        descriptionText.text = description;
    }
}
