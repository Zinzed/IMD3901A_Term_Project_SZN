using System.Collections;
using Unity.VisualScripting;
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

    [SerializeField] private int minAttacks = 2;
    [SerializeField] private int maxAttacks = 5;
    [SerializeField] private float timeBetweenAttacks = 1.5f;
    private float attackDuration = 1.06f;

    [SerializeField] private float teleportCooldown = 10f;
    [SerializeField] private float safeTimeBeforeTeleport = 2.0f;
    private float lastTeleportTime;

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

        lastTeleportTime = Time.time;

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
                    teleportCount = 0;
                    TriggerAttack();
                    SetNextAttack();                
            }
        }   
    }

    void TriggerAttack()
    {
        if (isDead || isAttacking)
        {
            return; 
        }

        StartCoroutine(AttackSequence());       
    }

    IEnumerator AttackSequence()
    {
        isAttacking = true;

        int attackCount = Random.Range(minAttacks, maxAttacks);

        

        for (int i = 0; i < attackCount; i++)
        {
            if (Time.time - lastTeleportTime > teleportCooldown - safeTimeBeforeTeleport)
            {
                break;
            }

            animator.ResetTrigger("isAttacking");
            animator.SetTrigger("isAttacking");
            StartCoroutine(AttackFxRoutine());

            yield return new WaitForSeconds(attackDuration + timeBetweenAttacks);
        }

        isAttacking = false;
    }

    IEnumerator AttackFxRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        if (attackEffect != null)
        {
            attackEffect.Play();
        }
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
