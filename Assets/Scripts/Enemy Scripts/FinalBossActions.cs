using System.Collections;
using UnityEngine;

public class FinalBossActions : MonoBehaviour
{
    public Animator animator;

    public ParticleSystem teleportEffect;
    public ParticleSystem attackEffect;

    public int teleportsBeforeAttack;
    private int teleportCount = 0;

    private bool isDead = false;
    private bool isAttacking = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetNextAttack();
    }

    public void OnTeleport()
    {
        if (isDead)
        {
            return;
        }

        if (teleportEffect != null)
        {
            //teleportEffect.transform.position = transform.position;
            teleportEffect.Play();
        }

        if (!isAttacking)
        {
            teleportCount++;

            if (teleportCount >= teleportsBeforeAttack)
            {
                if (!isAttacking)
                {
                    teleportCount = 0;
                    TriggerAttack();
                    SetNextAttack();
                }
            }
        }   
    }

    void TriggerAttack()
    {
        if (isDead || isAttacking)
        {
            return; 
        }

        isAttacking = true;
        animator.SetTrigger("isAttacking");

        StartCoroutine(AttackFxRoutine());
        StartCoroutine(ResetAttack());
    }

    IEnumerator AttackFxRoutine()
    {
        yield return new WaitForSeconds(0.1f);

        if (attackEffect != null)
        {
            attackEffect.Play();
        }
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1.06f);
        isAttacking = false;
    }

    public void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        animator.SetTrigger("isDead");
        Destroy(gameObject, 5.0f);
    }

    void SetNextAttack()
    {
        teleportsBeforeAttack = Random.Range(1, 4);
    }

    
}
