using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReversePlainBoss : MonoBehaviour
{
    [Header("火球參數")]
    public GameObject fireballPrefab;      // 火球預製物
    public Transform firePoint;            // 火球生成位置
    public float launchSpeed = 5f;         // 火球上射速度
    public float downwardSpeed = 5f;       // 火球下墜速度

    [Header("攻擊設定")]
    public float fireInterval = 0.75f;     // 每顆火球發射間隔
    public int fireballCount = 5;          // 一次攻擊發射火球數量
    public float attackDelay = 2f;         // 火球全部爆炸後的延遲時間

    public AudioClip sound;

    [Header("掉落範圍設定")]
    public float dropMinX;  // 掉落區域最小 X 值
    public float dropMaxX;  // 掉落區域最大 X 值

    void Start()
    {
        // 開始 BOSS 攻擊循環
        StartCoroutine(BossAttack());
    }

    IEnumerator BossAttack()
    {
        while (true)
        {
            List<FireBall> fireballs = new List<FireBall>();
            AudioManager.instance.PlaySound(sound);

            // 發射 fireballCount 顆火球，每顆間隔 fireInterval 秒
            for (int i = 0; i < fireballCount; i++)
            {
                GameObject fbObj = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
                FireBall fb = fbObj.GetComponent<FireBall>();

                // 傳入火球需要的參數：發射與下墜速度、掉落範圍與地面位置
                fb.Initialize(launchSpeed, downwardSpeed, dropMinX, dropMaxX);

                fireballs.Add(fb);
                yield return new WaitForSeconds(fireInterval);
            }

            // 所有火球都發射完畢後，再通知它們開始下墜
            foreach (var fb in fireballs)
            {
                fb.StartFalling();
            }

            // 等待所有火球爆炸或被銷毀
            yield return new WaitUntil(() => {
                foreach (var fb in fireballs)
                {
                    if (!fb.IsExploded)
                        return false;
                }
                return true;
            });

            // 攻擊結束後延遲 attackDelay 秒，再開始下一輪攻擊
            yield return new WaitForSeconds(attackDelay);
        }
    }
}
