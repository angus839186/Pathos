using System;
using System.Collections.Generic;
using UnityEngine;

public enum ResponseEventType
{
    None,
    Crutch,     // 拐杖
    MusicScore, // 樂譜
    // 其他分類...
}

public class ResponseEventManager : MonoBehaviour
{
    // Dictionary 用來管理每個分類的開啟狀態
    private Dictionary<ResponseEventType, bool> responseTypeStates = new Dictionary<ResponseEventType, bool>();

    private void Awake()
    {
        // 初始化所有選項狀態，預設為 false
        foreach (ResponseEventType category in Enum.GetValues(typeof(ResponseEventType)))
        {
            responseTypeStates[category] = false;
        }
    }

    public bool IsResponseTypeEnabled(ResponseEventType category)
    {
        return responseTypeStates.ContainsKey(category) && responseTypeStates[category];
    }

    public void SetResponseTypeState(ResponseEventType category, bool state)
    {
        if (responseTypeStates.ContainsKey(category))
        {
            responseTypeStates[category] = state;
            Debug.Log(responseTypeStates[category]);
        }
        else
        {
            responseTypeStates.Add(category, state);
        }
    }
}
