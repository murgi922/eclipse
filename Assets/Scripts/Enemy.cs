using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    public float speed = 10f;
    private Universal universalScript;
    GameObject playerObject;
    Player playerScript;
    [Header("Player Damage System")]
    public float timeDelayBetwnDamage = 1.0f;
    private float elapsedTime = 0.0f;
    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<Transform>();
            
        }
        else Debug.LogError("No player was found");
        playerScript = GameObject.FindWithTag("PlayerChild").GetComponent<Player>();

        GameObject universalObject = GameObject.FindWithTag("Universal");
        universalScript = universalObject.GetComponent<Universal>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        speed = universalScript.enemySpeed;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerChild"))
        {
            playerScript.TakeDamage(10);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerChild"))
        {
            elapsedTime += Time.fixedDeltaTime;
            if (elapsedTime > timeDelayBetwnDamage)
            {
                playerScript.TakeDamage(10);
                elapsedTime = 0.0f;
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerChild")) elapsedTime = 0.0f;
    }
}
