using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;

    [Header("Movement")]
    [Tooltip("Movement speed assigned when spawned by Universal script")]
    public float speed = 1.5f;

    private Universal universalScript;
    private Player playerScript;

    [Header("Player Damage System")]
    public float timeDelayBetwnDamage = 1.0f;
    private float elapsedTime = 0.0f;

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            playerScript = playerObject.GetComponent<Player>();
        }

        GameObject universalObject = GameObject.FindWithTag("Universal");
        if (universalObject != null)
        {
            universalScript = universalObject.GetComponent<Universal>();
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Move toward player at constant assigned speed
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // Rotate to face player
            Vector3 direction = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DamagePlayerAndSelf();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > timeDelayBetwnDamage)
            {
                DamagePlayerAndSelf();
                elapsedTime = 0.0f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            elapsedTime = 0.0f;
        }
    }

    private void DamagePlayerAndSelf()
    {
        if (playerScript != null)
        {
            playerScript.TakeDamage(1);
        }

        if (universalScript != null)
        {
            universalScript.enemyCount--;
            universalScript.deSpawnedEnemy++;
        }

        Destroy(gameObject);
    }
}