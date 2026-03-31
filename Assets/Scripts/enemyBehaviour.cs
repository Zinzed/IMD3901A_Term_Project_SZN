using UnityEngine;
using UnityEngine.Rendering;

public class enemyBehaviour : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float wanderRadius = 10.0f;
    public float chaseRadius = 12.0f;
    public float stopDistance = 2.5f;
    public float minHeight = 0.5f;
    public float maxHeight = 1.5f;
    public float hoverHeight = 1.0f;

    public bool isChasing;

    public float separationRadius = 1.5f;
    public float separationStrength = 1.5f;

    public float damageCooldown = 1.0f;
    private float lastDamageTime;

    public bool isDead = false;

    private Vector3 target;
    private float changeTarget = 1.5f;
    private Transform playerTransform;

    //public float damageCooldown = 1f;
    //private float lastDamageTime;

    private Rigidbody rigidBody;

    private Vector3 lastPosition;
    private float stuckTimer = 0.0f;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        pickTarget();
        playerTransform = GameObject.FindWithTag("Player").transform;

        Vector3 pos = transform.position;
        pos.y = hoverHeight;
        transform.position = pos;
    }

    private void FixedUpdate()
    {
        if (playerTransform != null)
        {
            float distanceFromPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceFromPlayer <= chaseRadius)
            {
                isChasing = true;

                Vector3 dirToPlayer = (transform.position - playerTransform.position).normalized;

                if (distanceFromPlayer > stopDistance)
                {
                    target = playerTransform.position + dirToPlayer * stopDistance;
                }
                else
                {
                    //Vector3 side = Vector3.Cross(dirToPlayer, Vector3.up);
                    //target = transform.position + side * 2f;
                    target = transform.position;
                }
            }
            else
            {
                isChasing = false;

                if (Vector3.Distance(transform.position, target) < changeTarget)
                {
                    pickTarget();
                }
            }
        }

        MoveEnemy();
    }
    
    void MoveEnemy()
    {
        Vector3 direction = target - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance < 0.5f)
        {
            pickTarget();
            return;
        }

        direction = direction.normalized;

        //Make sure enemies don't stack
        Vector3 separation = Vector3.zero;

        Collider[] nearby = Physics.OverlapSphere(transform.position, separationRadius);

        foreach (Collider col in nearby)
        {
            if (col.gameObject != gameObject && col.CompareTag("Enemy"))
            {
                Vector3 pushDir = transform.position - col.transform.position;
                pushDir.y = 0f;
                separation += pushDir.normalized;
            }
        }

        Vector3 finalMove = direction + separation * separationStrength;


        if (finalMove.sqrMagnitude < 0.01f)
        {
            finalMove = direction;
        }
            
        finalMove = finalMove.normalized;

        Vector3 newPos = rigidBody.position + finalMove * moveSpeed * Time.fixedDeltaTime;
        newPos.y = hoverHeight;
        rigidBody.MovePosition(newPos);

        //Makes sure enemies don't get stuck if something is in the way
        //Pick a new target if stuck for too long
        float movedDistance = Vector3.Distance(transform.position, lastPosition);

        if (movedDistance < 0.01f)
        {
            stuckTimer += Time.fixedDeltaTime;

            if (stuckTimer > 1.0f)
            {
                pickTarget();
                stuckTimer = 0.0f;
            }
        }
        else
        {
            stuckTimer = 0.0f;
        }

        lastPosition = transform.position;

        //StayOnGround();
    }

    void pickTarget()
    {
        Vector2 randomXZ = Random.insideUnitCircle * wanderRadius;

        target = new Vector3(
            transform.position.x + randomXZ.x,
            transform.position.y,
            transform.position.z + randomXZ.y
        );
    }

    //private void OnCollisionStay(Collision other)
    //{
    //    if (other.gameObject.CompareTag("Player") && Time.time > lastDamageTime + damageCooldown)
    //    {
    //        health playerHealth = other.gameObject.GetComponent<health>();

    //        if (playerHealth != null )
    //        {
    //            playerHealth.UpdateHealth(-1);
    //            lastDamageTime = Time.time;
    //        }
    //    }
    //}

    //void StayOnGround()
    //{
    //    RaycastHit hit;

    //    if (Physics.Raycast(rigidBody.position + Vector3.up * 5f, Vector3.down, out hit, 20f))
    //    {
    //        Vector3 pos = rigidBody.position;
    //        pos.y = hit.point.y + hoverHeight;
    //        rigidBody.position = pos;
    //    }
    //}

    void Update()
    {
        //if (playerTransform != null)
        //{
        //    float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        //    // Prioritize chasing if player is within range
        //    if (distanceToPlayer <= chaseRadius)
        //    {
        //        target = playerTransform.position;
        //    }
        //    else if (Vector3.Distance(transform.position, target) < changeTarget)
        //    {
        //        // Pick new target only when not chasing
        //        pickTarget();
        //    }
        //}

        //// Move towards the current target
        //transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

   

    //void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log($"Triggered with: {other.gameObject.name}"); // Debug to confirm trigger

    //    if (other.CompareTag("Player"))
    //    {
    //        Debug.Log("Player triggered!"); // Confirm player detection
    //        health playerHealth = other.GetComponent<health>();
    //        if (playerHealth != null)
    //        {
    //            playerHealth.UpdateHealth(-1); // Deduct health
    //        }
    //        else
    //        {
    //            Debug.LogError("Player has no health script!"); // Debug missing component
    //        }
    //    }
    //}
}