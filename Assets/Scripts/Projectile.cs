using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 10.0f;
    private Transform player;

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        transform.position += transform.up * (projectileSpeed * Time.deltaTime);

        if (player != null && Vector3.Distance(transform.position, player.position) > 20.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            HitEnemy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HitEnemy(collision.gameObject);
        }
    }

    private void HitEnemy(GameObject enemyObject)
    {
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeHit();
        }

        Destroy(gameObject);
    }
}