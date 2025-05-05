using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserAttack : attackDisplay
{
    [SerializeField] private Animator eyeAnimator;
    [SerializeField] private Animator laserAnimator;

    public override void Attack()
    {
        base.Attack();  // 啟用 GameObject
        // 如果正在執行，就先停掉
        if (fireCoroutine != null)
            StopCoroutine(fireCoroutine);
        // 開始每隔 fireInterval 秒挑角度並發射
        fireCoroutine = StartCoroutine(laserRoutine());
    }

    private IEnumerator laserRoutine()
    {
        while (true)
        {
            float angle = Random.Range(0f, 360f);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            eyeAnimator.Play("eye_ub");
            yield return new WaitUntil(() =>
            {
                AnimatorStateInfo stateInfo = eyeAnimator.GetCurrentAnimatorStateInfo(0);
                return stateInfo.IsName("eye_ub") && stateInfo.normalizedTime >= 1f;
            });
            eyeAnimator.Play("eye_attack");
            laserAnimator.SetTrigger("laserTrigger");
            if(attackSound != null)
            {
                AudioManager.instance.PlaySound(attackSound);
            }
            yield return new WaitUntil(() =>
            {
                AnimatorStateInfo stateInfo = eyeAnimator.GetCurrentAnimatorStateInfo(0);
                return stateInfo.IsName("eye_attack") && stateInfo.normalizedTime >= 1f;
            });

            yield return new WaitForSeconds(fireInterval);
        }
    }
}
