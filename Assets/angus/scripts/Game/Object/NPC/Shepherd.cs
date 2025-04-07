using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shepherd : InteractableNPC, IDataPersistence
{
    public string jungleScene;
    public string doorScene;

    public bool _shepherdAlreadyKnowSheepColored;

    public DialogueObject doorSceneNormalDialogue;
    public DialogueObject doorSceneFindSheepDialogue;

    public DialogueObject jungleSceneDialogue;

    void Start()
    {

    }
    public bool CheckSheepColorData(GameData data)
    {
        foreach (bool isColored in data.sheepGotColored.Values)
        {
            if (isColored)
            {
                return true;
            }
        }
        return false;
    }

    public override DialogueObject GetDialogue()
    {
        GameData data = DataPersistenceManager.Instance.gameData;
        if(SceneManager.GetActiveScene().name == doorScene)
        {
            if(!CheckSheepColorData(data))
            {
                return doorSceneNormalDialogue;
            }
            else
            {
                return doorSceneFindSheepDialogue;
            }
        }
        else
        {
            return jungleSceneDialogue;
        }
    }

    public void LoadData(GameData data)
    {
        data.shepherdAlreadyKnowSheepColored = _shepherdAlreadyKnowSheepColored;
        if (_shepherdAlreadyKnowSheepColored)
        {
            if (SceneManager.GetActiveScene().name == jungleScene)
            {
                gameObject.SetActive(true);
            }
            else
            {
                if (SceneManager.GetActiveScene().name == doorScene)
                {
                    gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (SceneManager.GetActiveScene().name == jungleScene)
            {
                gameObject.SetActive(false);
            }
            else
            {
                if (SceneManager.GetActiveScene().name == doorScene)
                {
                    gameObject.SetActive(true);
                }
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.shepherdAlreadyKnowSheepColored = _shepherdAlreadyKnowSheepColored;
    }
}
