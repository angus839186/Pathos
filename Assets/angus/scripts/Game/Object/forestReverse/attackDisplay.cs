using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class attackDisplay : MonoBehaviour
{
    public float fireInterval = 3f;
    public Coroutine fireCoroutine;

    public AudioClip attackSound;
    public virtual void Attack()
    {
        gameObject.SetActive(true);
    }
    public virtual void StopAttack()
    {
        gameObject.SetActive(false);
        if (fireCoroutine != null)
            StopCoroutine(fireCoroutine);
        fireCoroutine = null;
    }
}
