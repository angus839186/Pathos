using UnityEngine;
using Cinemachine;

public class SmoothCameraOffset : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // 虛擬攝影機
    public Transform player;                       // 角色
    public float mapLeftBoundary = 0f;            // 地圖左邊界
    public float mapRightBoundary = 10f;          // 地圖右邊界
    public float smoothTime = 0.2f;               // 平滑過渡時間

    private Vector3 velocity = Vector3.zero;      // 用於平滑過渡計算
    private CinemachineFramingTransposer framingTransposer;

    private void Start()
    {
        // 獲取 Framing Transposer 組件
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void LateUpdate()
    {
        // 計算角色位置
        float playerX = player.position.x;

        // 設定目標偏移值
        Vector3 targetOffset = Vector3.zero;

        if (playerX <= mapLeftBoundary)
        {
            // 角色接近左邊界，攝影機偏右
            targetOffset = new Vector3(7f, framingTransposer.m_TrackedObjectOffset.y, 0f);
        }
        else if (playerX >= mapRightBoundary)
        {
            // 角色接近右邊界，攝影機偏左
            targetOffset = new Vector3(-7f, framingTransposer.m_TrackedObjectOffset.y, 0f);
        }
        else
        {
            // 角色在地圖中間，攝影機居中
            targetOffset = Vector3.zero;
        }

        // 平滑過渡到目標偏移
        framingTransposer.m_TrackedObjectOffset = Vector3.SmoothDamp(
            framingTransposer.m_TrackedObjectOffset,
            targetOffset,
            ref velocity,
            smoothTime
        );
    }
}
