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

    [SerializeField] private AudioSource[] attackSounds;

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
            if (attack.isPlaying)
            {
                attack.Stop();
            }
        }
    }
}
