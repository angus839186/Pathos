using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public Transform pointA;  // 起始點
    public Transform pointB;  // 目標點
    public float flightSpeed = 5f; // 飛行速度
    public float landDistance = 0.5f; // 進入 Land 動畫的距離

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        transform.position = pointA.position; // 確保鳥物件起始於A點
    }

    public void FlyToShelf()
    {
        StartCoroutine(FlyRoutine());
    }

    private IEnumerator FlyRoutine()
    {

        // 1️⃣ 播放 Takeoff 動畫
        animator.Play("takeoff");
        yield return new WaitForSeconds(0.5f); // 假設起飛動畫 0.5 秒

        // 2️⃣ 切換 Fly 動畫
        animator.Play("fly");

        // 3️⃣ 平滑移動
        while (Vector2.Distance(transform.position, pointB.position) > landDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, pointB.position, flightSpeed * Time.deltaTime);
            yield return null; // 等待下一幀
        }

        // 4️⃣ 進入 Land 動畫
        animator.Play("land");
        yield return new WaitForSeconds(0.5f); // 假設降落動畫 0.5 秒

        // 5️⃣ 停止移動，確保鳥物件停在 B 點
        transform.position = pointB.position;
    }
}
