using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistenceCamera : MonoBehaviour
{
    public static PersistenceCamera Instance;
    void Awake()
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
        GameManager.Instance.OnSceneLoaded += GetCameraBorder;
    }
    void GetCameraBorder()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        CinemachineConfiner2D cinemachineConfiner2D = GetComponentInChildren<CinemachineConfiner2D>();
        CinemachineVirtualCamera cam = FindObjectOfType<CinemachineVirtualCamera>();
        GameObject CameraBorder = GameObject.Find("CameraBorder");
        cam.Follow = player.transform;
        if (cinemachineConfiner2D != null)
        {
            cinemachineConfiner2D.m_BoundingShape2D = CameraBorder.GetComponent<PolygonCollider2D>();
            cinemachineConfiner2D.InvalidateCache();
        }
    }
}
