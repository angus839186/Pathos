using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public Transform pointA;  // 起始點
    public Transform pointB;  // 目標點
    public float flightSpeed; // 飛行速度
    public float landDistance = 0.5f; // 進入 Land 動畫的距離

    private Animator animator;

    private bool isFlying;

    void Start()
    {
        animator = GetComponent<Animator>();
        transform.position = pointA.position; // 確保鳥物件起始於A點
    }

    public void FlyToNextPos()
    {
        StartCoroutine(FlyRoutine());
    }
    private IEnumerator FlyRoutine()
{
    isFlying = true;

    // 1️⃣ 播放起飛動畫
    animator.Play("bird_takesoff");
    yield return new WaitForSeconds(0.5f); // 等待動畫播放

    // 2️⃣ 切換為飛行動畫
    animator.Play("bird_fly");

    // 3️⃣ 平滑移動
    while (Vector2.Distance(transform.position, pointB.position) > landDistance)
    {
        transform.position = Vector2.MoveTowards(transform.position, pointB.position, flightSpeed * Time.deltaTime);
        yield return null; // 等待下一幀
    }

    // 4️⃣ 降落動畫
    animator.Play("bird_landing");
    yield return new WaitForSeconds(0.5f);

    // 5️⃣ 停止移動
    animator.Play("bird_wait");
    transform.position = pointB.position;
    isFlying = false;
}


}
