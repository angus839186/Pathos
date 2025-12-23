using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Video;

public class ReversePlainBoss : InteractableObject, IDataPersistence
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

    public string nextScene;

    [Header("擊飛")]
    public Transform knockbackTarget;
    public float knockbackDuration = 2.5f; 

    [Header("死亡")]

    public bool die;
    public VideoClip dieClip;

    public List<double> pausePoints;

    public bool ToBeContinued;

    public GameObject ToBeContinueObject;

    public AudioSource BGM;


    void OnDisable()
    {
        VideoController video = VideoController.Instance;
        if (video != null)
        {
            VideoController.Instance.OnVideoEnd -= Die;
        }

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if(playerHealth != null)
        {
            playerHealth.OnPlayerDied += StopAttack;
        }

    }

    void Start()
    {
        anime = GetComponent<Animator>();
        PlayerHealth playerhealth = FindObjectOfType<PlayerHealth>();
        playerhealth.OnPlayerDied += StopAttack;
    }


    public override string GetDescription(Item heldItem)
    {
        return base.GetDescription(null);
    }

    public override string GetAnimationTrigger(Item heldItem)
    {
        StopAttack();
        return "confort";
    }
    public override void Interact()
    {
        //Do nothing
    }

    public override void InteractEvent(Item heldItem)
    {
        if (phase2 == false)
        {
            NextPhase();
        }
        else
        {
            BossDieAnimation();
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.enabled = false;
        }
    }

    public void StartAttack()
    {
        StartCoroutine(BossAttack());
    }

    public void StopAttack()
    {
        FireBall[] fireballs = FindObjectsOfType<FireBall>();
        foreach (FireBall fb in fireballs)
        {
            fb.IsExploded = true;
            Destroy(fb.gameObject);
        }
        StopAllCoroutines();
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
        PlayerController player = FindObjectOfType<PlayerController>();
        player.ToggleMove(false);
        anime.Play("BOSS_hit");
        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo stateInfo = anime.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName("BOSS_hit") && stateInfo.normalizedTime >= 1f;
        });
        collider.enabled = true;
        fireballCount = 10;
        anime.SetBool("phase2", true);
        phase2 = true;
        StartAttack();
    }

    void BossDieAnimation()
    {
        die = true;
        anime.SetBool("died", true);
        AudioManager.instance.PlaySound(dieSound);
    }

    public void DieVideo()
    {
        FindObjectOfType<PlayerHealth>().Recover();
        BGM.Pause();
        VideoController.Instance.OnVideoEnd += Die;
        VideoController.Instance.GetVideo(dieClip, pausePoints);
    }

    public void Die()
    {
        if (ToBeContinued)
        {
            PlayerInputManager.Instance.BackToMenuEvent += BackToMenu;
            PlayerInputManager.Instance.SwitchActionMap("ToBeContinued");
            ToBeContinueObject.SetActive(true);
        }
        else
        {
            DataPersistenceManager.Instance.SaveGameData();
            GameManager gameManager = GameManager.Instance;
            gameManager.StartCoroutine(gameManager.LoadNextScene(nextScene));
        }
    }

    public void BackToMenu()
    {
        GameManager.Instance.BackToMenu();
    }


    public void BossPlaySkill()
    {
        KnockBackPlayer();
    }
    public void KnockBackPlayer()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            Vector2 startPos = player.transform.position;
            Vector2 targetPos = knockbackTarget.position;
            float T = knockbackDuration;

            float g = Mathf.Abs(Physics2D.gravity.y) * player.rb.gravityScale;

            Vector2 knockbackVelocity;
            knockbackVelocity.x = (targetPos.x - startPos.x) / T;
            knockbackVelocity.y = (targetPos.y - startPos.y + 0.5f * g * T * T) / T;

            player.rb.linearVelocity = knockbackVelocity;
            player._anime.SetTrigger("hurt");

            StartCoroutine(ReenablePlayerMovement(player, T));
        }
    }

    private IEnumerator ReenablePlayerMovement(PlayerController player, float delay)
    {
        yield return new WaitForSeconds(delay);
        player.ToggleMove(true);
    }

    public void LoadData(GameData data)
    {
        //Do Nothing
    }

    public void SaveData(ref GameData data)
    {
        data.reversePlainBossDied = die;
    }
}
