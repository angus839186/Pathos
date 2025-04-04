using UnityEngine;
using UnityEngine.Events;
using System;

[System.Serializable]
public class ResponseEvent
{
    [HideInInspector] public string name;
    [SerializeField] private UnityEvent onPickedResponse;
    [SerializeField] private ResponseEventType responseCategory;

    public UnityEvent OnPickedResponse => onPickedResponse;

    public ResponseEventType ResponseCategory => responseCategory;
}