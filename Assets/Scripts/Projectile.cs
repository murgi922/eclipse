using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 10.0f;
    private Universal universalScript;
    private Transform player;

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        GameObject universalObject = GameObject.FindWithTag("Universal");
        if (universalObject != null)
        {
            universalScript = universalObject.GetComponent<Universal>();
        }
    }

    void Update()
    {
        // Move upward relative to projectile's orientation
        transform.position += transform.up * (projectileSpeed * Time.deltaTime);

        // Despawn if out of bounds relative to player
        if (player != null && Vector3.Distance(transform.position, player.position) > 20.0f)
        {
            Destroy(gameObject);
        }
    }

    // Handles trigger collisions (if Enemy colliders are set to Trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            DestroyEnemy(collision.gameObject);
        }
    }

    // Handles standard collisions (if Enemy colliders are not Triggers)
    // Handles standard collisions (if Enemy colliders are NOT triggers)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // FIX: Add .gameObject before .CompareTag
        if (collision.gameObject.CompareTag("Enemy"))
        {
            DestroyEnemy(collision.gameObject);
        }
    }

    private void DestroyEnemy(GameObject enemy)
    {
        if (universalScript != null)
        {
            universalScript.enemyCount--;
            universalScript.deSpawnedEnemy++;
        }

        Destroy(enemy);
        Destroy(gameObject);
    }
}