using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class deathUI : MonoBehaviour
{
    public string deathDialogue;
    public TextMeshProUGUI deathText;
    public GameObject options;

    public float textSpeed;

    public Animator anime;

    public CanvasGroup deathCanvas;

    public static deathUI Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        deathCanvas = GetComponent<CanvasGroup>();
        ToggleCanvasGroup(false);
    }
    public void PlayerDeath()
    {
        anime.SetTrigger("die");
        ToggleCanvasGroup(true);
        StartCoroutine(showDialogueAndOption());
    }

    public void OnClickYes()
    {
        GameManager.Instance.BackToMenu();
        ToggleCanvasGroup(false);
    }
    public void OnClickNo()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        DataPersistenceManager.Instance.SaveGameData();
        GameManager.Instance.StartCoroutine(GameManager.Instance.ReloadScene(sceneName));
        ToggleCanvasGroup(false);
    }

    public void ToggleCanvasGroup(bool Toggle)
    {
        // 設定透明度，顯示時為 1，隱藏時為 0
        deathCanvas.alpha = Toggle ? 1f : 0f;
        // 控制是否可以互動
        deathCanvas.interactable = Toggle;
        // 控制是否阻擋射線（用於 UI 點擊）
        deathCanvas.blocksRaycasts = Toggle;
    }

    IEnumerator showDialogueAndOption()
    {
        deathText.text = "";
        foreach (char letter in deathDialogue.ToCharArray())
        {
            deathText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        yield return null;
        options.SetActive(true);
    }
}
