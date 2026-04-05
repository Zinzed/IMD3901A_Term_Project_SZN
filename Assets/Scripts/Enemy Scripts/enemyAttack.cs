using System.Collections;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class enemyAttack : MonoBehaviour
{
    public ParticleSystem attack;
    public float fireRate = 1.5f;

    public float damageRange = 6.0f;
    public int damageAmount = 1;
    public float damageCooldown = 1.0f;

    private float nextFireTime = 0.0f;
    private float lastDamageTime = 0.0f;

    private enemyBehaviour enemy;
    private Transform player;
    private health playerHealth;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] attackSounds;

    private bool isAttacking = false;

    [SerializeField] private float fadeDuration = 0.5f;
    private Coroutine fadeCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GetComponent<enemyBehaviour>();

        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<health>();
        }

        if (attack != null)
        {
            attack.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null || attack == null || player == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (enemy.isChasing)
        {
            if (!isAttacking)
            {
                isAttacking = true;

                if (fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                }

                audioSource.volume = 0.5f;

                if(attackSounds.Length > 0)
                {
                    int index = Random.Range(0, attackSounds.Length);
                    audioSource.clip = attackSounds[index];
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
            if (Time.time >= nextFireTime)
            {
                attack.transform.LookAt(GameObject.FindWithTag("Player").transform);
                attack.Play();
                nextFireTime = Time.time + fireRate;
            }
            if (attack.isPlaying && distance <= damageRange && Time.time >= lastDamageTime + damageCooldown)
            {
                if (playerHealth != null)
                {
                    playerHealth.UpdateHealth(-damageAmount);
                    lastDamageTime = Time.time;

                    Debug.Log("Player attacked!");
                }
            }
        }
        else
        {
            StopAttacking();
        }

        if (enemy.isDead)
        {
            StopAttacking();
        }
    }
    //stp
    void StopAttacking()
    {
        if (isAttacking)
        {
            isAttacking = false;
            
            if (attack.isPlaying)
            {
                attack.Stop();
            }
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = StartCoroutine(FadeOutAudio());
        }
    }

    IEnumerator FadeOutAudio()
    {
        float startVolume = audioSource.volume;

        float time = 0.0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0.0f, time / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
