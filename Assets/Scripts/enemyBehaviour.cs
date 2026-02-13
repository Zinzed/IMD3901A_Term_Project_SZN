using UnityEngine;

public class enemyBehaviour : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float wanderRadius = 10.0f;
    public float minHeight = 0.5f;
    public float maxHeight = 1.5f;

    private Vector3 target;
    private float changeTarget = 0.5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pickTarget();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target - transform.position;

        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < changeTarget)
        {
            pickTarget();
            return;
        }

        
    }

    void pickTarget()
    {
        Vector2 randomXZ = Random.insideUnitCircle * wanderRadius;
        float targetY = Random.Range(minHeight, maxHeight);

        target = new Vector3(transform.position.x + randomXZ.x, targetY, transform.position.z + randomXZ.y);


    }
}
