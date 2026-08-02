using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float speed = 10f;
    private Universal universalScript;
    GameObject playerObject;
    Player playerScript;
    [Header("Player Damage System")]
    public float timeDelayBetwnDamage = 1.0f;
    private float elapsedTime = 0.0f;
    //Enemy Type
    private int enemyIndex;
    private HealthSystem healthSystem;
    private Sprite[] enemies;
    
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
        healthSystem = new HealthSystem(enemyIndex + 1);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        speed = universalScript.enemySpeed;
        speed /= healthSystem.GetHealth();
        if (!healthSystem.IsAlive())
        {
            universalScript.enemyCount--;
            universalScript.deSpawnedEnemy++;
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerChild"))
        {
            playerScript.TakeDamage(10);
            TakeDamageSelf();
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
                TakeDamageSelf();
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerChild")) elapsedTime = 0.0f;
    }
    public void EnemyIndexSet(int index)
    {
        enemyIndex = index;
    }
    public int GetEnemyIndex()
    { return enemyIndex; }
    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
    public void changeSprite(int index)
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = enemies[index];
    }
    public void SetEnemyTypes(Sprite[] enemies)
    {
        this.enemies = enemies;
    }
    private void TakeDamageSelf()
    {
        healthSystem.TakeDamage(1);
        enemyIndex--;
        changeSprite(enemyIndex);

    }
}
