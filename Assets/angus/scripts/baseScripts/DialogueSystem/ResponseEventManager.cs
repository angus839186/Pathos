using System;
using System.Collections.Generic;
using UnityEngine;

public enum ResponseEventType
{
    基本,
    拐杖,
    樂譜,
    樂手表演,
    音樂花,
    失蹤村民,
    
}

public class ResponseEventManager : MonoBehaviour, IDataPersistence
{
    private Dictionary<ResponseEventType, bool> responseTypeStates = new Dictionary<ResponseEventType, bool>();

    private void Awake()
    {
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

    public void LoadData(GameData data)
    {
        // 遍歷所有 enum 值，並嘗試從 GameData 裡面讀取對應狀態
        foreach (ResponseEventType type in Enum.GetValues(typeof(ResponseEventType)))
        {
            string key = type.ToString();
            if (data.ResponseEventStates.ContainsKey(key))
            {
                responseTypeStates[type] = data.ResponseEventStates[key];
            }
            else
            {
                // 若沒有找到就設定為預設值
                responseTypeStates[type] = false;
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        // 清空原本的狀態字典，避免重複存儲
        data.ResponseEventStates.Clear();
        // 將每個 ResponseEventType 的狀態以字串作為鍵存入 GameData
        foreach (var kvp in responseTypeStates)
        {
            string key = kvp.Key.ToString();
            data.ResponseEventStates[key] = kvp.Value;
        }
    }
}
