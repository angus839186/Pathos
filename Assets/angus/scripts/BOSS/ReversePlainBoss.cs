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

    public AudioClip attackSound;
    public AudioClip dieSound;

    [Header("掉落範圍設定")]
    public float dropMinX;
    public float dropMaxX;
    public bool phase2;

    public Animator anime;

    [Header("擊飛")]
    public Transform knockbackTarget; // 指定擊飛的目標位置（在 Inspector 指定）
    public float knockbackDuration = 2.5f; // 飛行持續時間

    void OnEnable()
    {
        Tutorial _tutorial = FindObjectOfType<Tutorial>();
        if(_tutorial != null)
        {
            _tutorial.StartBossFight += ToggleTeleporter;
        }
    }

    void OnDisable()
    {
        Tutorial _tutorial = FindObjectOfType<Tutorial>();
        if(_tutorial != null)
        {
            _tutorial.StartBossFight -= ToggleTeleporter;
        }
    }

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
        FireBall[] fireballs = FindObjectsOfType<FireBall>();
        foreach (FireBall fb in fireballs)
        {
            fb.IsExploded = true;
            Destroy(fb.gameObject);
        }
        StopAllCoroutines();
    }

    public void ToggleTeleporter(bool toggle)
    {
        teleporter[] teleporters = FindObjectsOfType<teleporter>();
        foreach (var teleporter in teleporters)
        {
            teleporter.canTeleport = toggle;
        }
    }

    public IEnumerator BossAttack()
    {
        while (true)
        {
            List<FireBall> fireballs = new List<FireBall>();
            AudioManager.instance.PlaySound(attackSound);

            for (int i = 0; i < fireballCount; i++)
            {
                GameObject fbObj = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
                FireBall fb = fbObj.GetComponent<FireBall>();

                fb.Initialize(launchSpeed, downwardSpeed, dropMinX, dropMaxX);

                fireballs.Add(fb);
                yield return new WaitForSeconds(fireInterval);
            }

            foreach (var fb in fireballs)
            {
                fb.StartFalling();
            }

            yield return new WaitUntil(() =>
            {
                foreach (var fb in fireballs)
                {
                    if (!fb.IsExploded)
                        return false;
                }
                return true;
            });

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
            NextPhase();
        }
        else
        {
            StopAttack();
            BossDied();
        }
    }
    public void NextPhase()
    {
        StartCoroutine(BossChange());
    }

    IEnumerator BossChange()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;
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
        collider.enabled = true;
        fireballCount = 8;
        attackDelay = 1f;
        anime.SetBool("phase2", true);
        phase2 = true;
        StartAttack();
    }

    void BossDied()
    {
        Debug.Log("BossDied");
        anime.SetBool("died", true);
        AudioManager.instance.PlaySound(dieSound);
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

        player.canMove = false;

        Vector2 startPos = player.transform.position;
        Vector2 targetPos = knockbackTarget.position;
        float T = knockbackDuration;

        float g = Mathf.Abs(Physics2D.gravity.y) * player.rb.gravityScale;

        Vector2 knockbackVelocity;
        knockbackVelocity.x = (targetPos.x - startPos.x) / T;
        knockbackVelocity.y = (targetPos.y - startPos.y + 0.5f * g * T * T) / T;

        player.rb.velocity = knockbackVelocity;
        player._anime.SetTrigger("hurt");

        StartCoroutine(ReenablePlayerMovement(player, T));
    }

    private IEnumerator ReenablePlayerMovement(PlayerController player, float delay)
    {
        yield return new WaitForSeconds(delay);
        player.canMove = true;
    }
}
