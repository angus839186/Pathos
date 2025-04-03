using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenceObject : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
