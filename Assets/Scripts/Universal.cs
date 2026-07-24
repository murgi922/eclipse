using TMPro;
using UnityEngine;

public class Universal : MonoBehaviour
{
    [Header("Target & Prefabs")]
    public Transform player;
    public GameObject enemyPrefab;

    [Header("Boundaries")]
    public Vector2 innerBoundary = new Vector2(5, 5);
    public float boundaryThickness = 3.0f;
    private Vector2 outerBoundary;

    [Header("Enemy Speed Settings")]
    [Tooltip("Starting speed for enemies in Wave 1. CHANGE THIS IN INSPECTOR TO TWEAK SPEED!")]
    public float baseEnemySpeed = 1.5f; // Lowered default starting speed

    [Tooltip("How much faster enemies get with each new wave")]
    public float speedIncreasePerWave = 0.3f;

    [Header("Wave Timing & Scaling")]
    public float timeDelay = 1.0f;
    public int totalEnemy = 5;
    public int enemyIncreaseAfterEachWave = 2;
    public float waveDelay = 3.0f;

    // Runtime variables
    [HideInInspector] public float currentWaveSpeed;
    [HideInInspector] public int enemyCount = 0;
    [HideInInspector] public int deSpawnedEnemy = 0;

    private float timeElapsed = 0.0f;
    private float waveDelayTime;
    private int spawnedEnemy = 0;
    private int waveNumber = 1;

    [Header("UI")]
    public TextMeshProUGUI waveText;
    public Player playerScript;

    void Start()
    {
        outerBoundary.x = innerBoundary.x + boundaryThickness;
        outerBoundary.y = innerBoundary.y + boundaryThickness;

        currentWaveSpeed = baseEnemySpeed;
        waveDelayTime = waveDelay; // Start Wave 1 immediately

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        EnemyWave();
        UpdateUI();
    }

    void EnemyWave()
    {
        waveDelayTime += Time.deltaTime;

        if (waveDelayTime >= waveDelay)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= timeDelay && spawnedEnemy < totalEnemy)
            {
                SpawnEnemy(enemyPrefab, innerBoundary, outerBoundary);
                enemyCount++;
                spawnedEnemy++;
                timeElapsed = 0.0f;
            }

            if (deSpawnedEnemy >= totalEnemy)
            {
                AdvanceWaveDifficulty();
            }
        }
    }

    void AdvanceWaveDifficulty()
    {
        waveNumber++;
        deSpawnedEnemy = 0;
        spawnedEnemy = 0;
        enemyCount = 0;
        waveDelayTime = 0.0f;

        // Increase wave speed
        currentWaveSpeed += speedIncreasePerWave;
        totalEnemy += enemyIncreaseAfterEachWave;
    }

    void SpawnEnemy(GameObject enemy, Vector2 innerBoundary, Vector2 outerBoundary)
    {
        if (player == null) return;

        innerBoundary = new Vector2(Mathf.Abs(innerBoundary.x), Mathf.Abs(innerBoundary.y));
        outerBoundary = new Vector2(Mathf.Abs(outerBoundary.x), Mathf.Abs(outerBoundary.y));
        Vector2 location = RandomNumber(innerBoundary, outerBoundary);

        GameObject newEnemy = Instantiate(enemy, new Vector3(location.x + player.position.x, location.y + player.position.y, 0), Quaternion.identity);

        // Pass the manager's wave speed directly to the newly spawned enemy instance
        Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.speed = currentWaveSpeed;
        }
    }

    Vector2 RandomNumber(Vector2 innerBoundary, Vector2 outerBoundary)
    {
        Vector2 location;
        if (Random.value < 0.5f)
        {
            location.x = (Random.value > 0.5f) ? Random.Range(innerBoundary.x, outerBoundary.x) : Random.Range(-innerBoundary.x, -outerBoundary.x);
            location.y = Random.Range(-outerBoundary.y, outerBoundary.y);
        }
        else
        {
            location.y = (Random.value > 0.5f) ? Random.Range(innerBoundary.y, outerBoundary.y) : Random.Range(-innerBoundary.y, -outerBoundary.y);
            location.x = Random.Range(-outerBoundary.x, outerBoundary.x);
        }
        return location;
    }

    private void UpdateUI()
    {
        if (waveText != null && playerScript != null && playerScript.GetHealthSystem() != null)
        {
            waveText.text = $"Wave: {waveNumber}, Health: {playerScript.GetHealthSystem().GetHealth()}/{playerScript.GetHealthSystem().GetMaxHealth()}";
        }
    }
}