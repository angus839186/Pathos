using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    private static PersistentObject Instance;

    void Awake() {
    if (Instance == null) {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    } else {
        Destroy(gameObject);
    }
}
}
