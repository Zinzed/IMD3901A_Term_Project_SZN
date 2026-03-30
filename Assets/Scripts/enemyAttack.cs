using UnityEngine;

public class enemyAttack : MonoBehaviour
{
    public ParticleSystem attack;
    public float fireRate = 1.5f;

    private float nextFireTime = 0.0f;
    private enemyBehaviour enemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GetComponent<enemyBehaviour>();

        if (attack != null)
        {
            attack.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null || attack == null) return;

        if (enemy.isChasing)
        {
            if (Time.time >= nextFireTime)
            {
                attack.transform.LookAt(GameObject.FindWithTag("Player").transform);
                attack.Play();
                nextFireTime = Time.time + fireRate;
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
