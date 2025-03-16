using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ReversePlainBoss : MonoBehaviour, IInteractable
{
    [Header("火球參數")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float launchSpeed = 5f;
    public float downwardSpeed = 5f;

    [Header("phase1攻擊設定")]
    public float fireInterval = 0.75f;
    public int fireballCount = 5;
    public float attackDelay = 2f;

    public AudioClip sound;

    [Header("掉落範圍設定")]
    public float dropMinX;
    public float dropMaxX;
    public bool phase2;

    public Animator anime;

    [Header("擊飛")]
    public Transform knockbackTarget; // 指定擊飛的目標位置（在 Inspector 指定）
    public float knockbackDuration = 1f; // 飛行持續時間

    void Start()
    {
        anime = GetComponent<Animator>();
    }


    public void StartAttack()
    {
        StartCoroutine(BossAttack());
    }

    public void StopAttack()
    {
        Debug.Log("Boss stop attack");
        StopAllCoroutines();
    }

    public IEnumerator BossAttack()
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
            yield return new WaitUntil(() =>
            {
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

    public void Interact()
    {
        //Do nothing
    }

    public string GetDescription()
    {
        return "";
    }

    public string GetAnimationTrigger(Item heldItem)
    {
        return "confort";
    }

    public void InteractEvent(Item heldItem)
    {
        if (phase2 == false)
        {
            StopAttack();
            FireBall[] fireballs = FindObjectsOfType<FireBall>();
            foreach (FireBall fb in fireballs)
            {
                fb.IsExploded = true;
                Destroy(fb.gameObject);
            }
            NextPhase();
            phase2 = true;
            return;
        }
        else
        {

        }
    }
    public void NextPhase()
    {
        StartCoroutine(BossChnage());
    }

    IEnumerator BossChnage()
    {
        anime.Play("BOSS_change");
        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo stateInfo = anime.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName("BOSS_change") && stateInfo.normalizedTime >= 1f;
        });
        anime.Play("BOSS_hit");
        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo stateInfo = anime.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName("BOSS_hit") && stateInfo.normalizedTime >= 1f;
        });
        fireballCount = 8;
        attackDelay = 1f;
        anime.SetBool("phase2", true);
        StartAttack();
    }
    public void BossPlaySkill()
    {
        KnockBackPlayer();
    }
    public void KnockBackPlayer()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player == null)
            return;

        // 暫時禁用玩家移動
        player.canMove = false;

        Vector2 startPos = player.transform.position;
        Vector2 targetPos = knockbackTarget.position;
        float T = knockbackDuration;

        // 考慮玩家的 gravityScale
        float g = Mathf.Abs(Physics2D.gravity.y) * player.rb.gravityScale;

        // 計算初始速度向量
        Vector2 knockbackVelocity;
        knockbackVelocity.x = (targetPos.x - startPos.x) / T;
        knockbackVelocity.y = (targetPos.y - startPos.y + 0.5f * g * T * T) / T;

        // 將計算出的速度套用給玩家
        player.rb.velocity = knockbackVelocity;
        player._anime.SetTrigger("hurt");

        // 飛行結束後恢復玩家移動
        StartCoroutine(ReenablePlayerMovement(player, T));
    }

    private IEnumerator ReenablePlayerMovement(PlayerController player, float delay)
    {
        yield return new WaitForSeconds(delay);
        player.canMove = true;
    }
}
