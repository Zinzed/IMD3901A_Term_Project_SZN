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

    public bool isDead { get; private set; } = false;
    private bool isAttacking = false;

    [SerializeField] private int minAttacks = 2;
    [SerializeField] private int maxAttacks = 5;
    [SerializeField] private float timeBetweenAttacks = 1.5f;
    private float attackDuration = 1.06f;

    [SerializeField] private float teleportCooldown = 10f;
    [SerializeField] private float safeTimeBeforeTeleport = 2.0f;
    private float lastTeleportTime;

    [SerializeField] public int damageAmount = 10;
    private Transform player;
    private health playerHealth;
    //[SerializeField] private AudioClip attackSound;

    [SerializeField] private float attackRange = 3.0f;
    [SerializeField] private float damageDelay = 0.3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetNextAttack();

        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<health>();
        }
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
            //sound
            StartCoroutine(DamageWithDelay());
            yield return new WaitForSeconds(attackDuration + timeBetweenAttacks);
        }

        isAttacking = false;
    }

    IEnumerator DamageWithDelay()
    {
        yield return new WaitForSeconds(damageDelay);

        if (player != null && playerHealth != null)
        {
            float distFromPlayer = Vector3.Distance(transform.position, player.position);

            if (distFromPlayer <= attackRange)
            {
                Debug.Log("Damaged Player!");
                playerHealth.UpdateHealth(-damageAmount);
            }
            else
            {
                Debug.Log("Player too far, no damage done!");
            }
        }
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

        Debug.Log("Die() called on boss");
        isDead = true;

        Rigidbody rb = GetComponentInParent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        animator.SetTrigger("isDead");
        Destroy(gameObject, 5.0f);
    }

    void SetNextAttack()
    {
        teleportsBeforeAttack = Random.Range(1, 4);
    }

    
}
