using TMPro;
using UnityEngine;

public class Universal : MonoBehaviour
{
    [Header("Target & Prefabs")]
    public Transform player;
    public GameObject enemyPrefab;

    [Header("Boundaries (Spawn Distance)")]
    public Vector2 innerBoundary = new Vector2(2.5f, 2.5f);
    public float boundaryThickness = 1.5f;
    private Vector2 outerBoundary;

    [Header("Base Speed Settings")]
    [Tooltip("Base movement speed for a Triangle (3-sided) enemy")]
    public float baseTriangleSpeed = 2.5f;

    [Tooltip("How much speed enemies lose per extra polygon side (Higher side = Slower)")]
    public float speedPenaltyPerSide = 0.45f;

    [Tooltip("Minimum movement speed cap so octagons don't freeze completely")]
    public float minEnemySpeed = 0.5f;

    [Header("Wave Timing & Scaling")]
    public float timeDelay = 0.35f;
    public int totalEnemy = 5;
    public int enemyIncreaseAfterEachWave = 3;
    public float waveDelay = 1.0f;

    // Runtime variables
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

        waveDelayTime = waveDelay;

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

        // Increase total enemies per wave
        totalEnemy += enemyIncreaseAfterEachWave;
    }

    void SpawnEnemy(GameObject enemy, Vector2 innerBoundary, Vector2 outerBoundary)
    {
        if (player == null) return;

        innerBoundary = new Vector2(Mathf.Abs(innerBoundary.x), Mathf.Abs(innerBoundary.y));
        outerBoundary = new Vector2(Mathf.Abs(outerBoundary.x), Mathf.Abs(outerBoundary.y));
        Vector2 location = RandomNumber(innerBoundary, outerBoundary);

        GameObject newEnemy = Instantiate(enemy, new Vector3(location.x + player.position.x, location.y + player.position.y, 0), Quaternion.identity);

        Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            // 1. Calculate side count based on wave difficulty progression algorithm
            int sides = CalculateEnemySides(waveNumber);
            enemyComponent.SetSides(sides);

            // 2. Inverse Speed Algorithm: Higher polygon sides = Slower movement speed
            // Formula: BaseSpeed - ((Sides - 3) * Penalty) + Slight Wave Acceleration
            float waveSpeedBonus = (waveNumber - 1) * 0.08f;
            float calculatedSpeed = baseTriangleSpeed - ((sides - 3) * speedPenaltyPerSide) + waveSpeedBonus;

            // Enforce minimum speed limit
            enemyComponent.speed = Mathf.Max(calculatedSpeed, minEnemySpeed);
        }
    }

    /// <summary>
    /// Weighted Random Pool Algorithm for selecting Polygon Side counts.
    /// Unlocks higher shapes as waves progress and ramps up their spawn chances.
    /// </summary>
    private int CalculateEnemySides(int wave)
    {
        // Unlocks a new higher polygon shape every 2 waves (Cap at 8 = Octagon)
        int maxUnlockedSides = Mathf.Min(3 + (wave / 2), 8);

        // Calculate probability weights for shapes 3 through maxUnlockedSides
        float[] weights = new float[maxUnlockedSides - 2];
        float totalWeight = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            int sideCount = i + 3; // 3 = Triangle, 4 = Square, etc.

            // Weight formula: Higher shapes get higher spawn probability as wave numbers rise
            float weight = Mathf.Pow(wave, (sideCount - 3) * 0.4f);
            weights[i] = weight;
            totalWeight += weight;
        }

        // Weighted Random Pick
        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue <= cumulativeWeight)
            {
                return i + 3; // Returns side count (3 to 8)
            }
        }

        return 3; // Fallback to Triangle
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