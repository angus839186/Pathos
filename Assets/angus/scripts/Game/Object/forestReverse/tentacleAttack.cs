using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tentacleAttack : attackDisplay
{

    public override void Attack()
    {
        base.Attack();
        if (fireCoroutine != null)
            StopCoroutine(fireCoroutine);
        fireCoroutine = StartCoroutine(tentacleRoutine());
    }

    public IEnumerator tentacleRoutine()
    {
        while (true)
        {
            if(attackSound != null)
            {
                AudioManager.instance.PlaySound(attackSound);
            }
            yield return new WaitForSeconds(fireInterval);
        }
    }
}
