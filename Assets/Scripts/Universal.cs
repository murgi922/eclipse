using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class Universal : MonoBehaviour
{
    [Header("Enemy Control")]
    public Transform player;
    public GameObject enemyPrefab;
    public Vector2 innerBoundary;
    public float boundaryThickness = 3.0f;
    private Vector2 outerBoundary;
    public float timeDelay = 1.0f;
    private float timeElapsed = 0.0f;
    public int totalEnemy = 10;
    public float waveDelay = 5.0f;
    private float waveDelayTime = 0.0f;
    [HideInInspector]
    public int enemyCount = 0;
    [HideInInspector]
    public int deSpawnedEnemy = 0;
    [HideInInspector]
    public float enemySpeed;
    public float enemySpeedIncrease = 0.7f;
    public int enemyIncreaseAfterEachWave = 5;
    private int spawnedEnemy = 0;

    [Header("UI")]
    public TextMeshProUGUI waveText;
    private int waveNumber = 1;
    void Start()
    {
        outerBoundary.x = innerBoundary.x + boundaryThickness;
        outerBoundary.y = innerBoundary.y + boundaryThickness;
        enemySpeed = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        EnemyWave();
        waveText.text = "Wave: " + waveNumber;
    }
    void EnemyWave()
    {
        waveDelayTime += Time.deltaTime;
        if (waveDelayTime > waveDelay)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed > timeDelay && spawnedEnemy < totalEnemy)
            {
                SpawnEnemy(enemyPrefab, innerBoundary, outerBoundary);
                enemyCount++;
                spawnedEnemy++;
                timeElapsed = 0.0f;
            }
            if (deSpawnedEnemy == totalEnemy)
            {
                deSpawnedEnemy = 0;
                totalEnemy += enemyIncreaseAfterEachWave;
                spawnedEnemy = 0;
                enemyCount = 0;
                waveDelayTime = 0.0f;
                enemySpeed += enemySpeedIncrease;
                waveNumber++;
            }
        }
    }
    void SpawnEnemy(GameObject enemy, Vector2 location)
    {
        Instantiate(enemy, new Vector3(location.x, location.y, 0), Quaternion.identity);
    }
    void SpawnEnemy(GameObject enemy, Vector2 innerBoundary, Vector2 outerBoundary)
    {
        innerBoundary = new Vector2(Mathf.Abs(innerBoundary.x), Mathf.Abs(innerBoundary.y));
        outerBoundary = new Vector2(Mathf.Abs(outerBoundary.x), Mathf.Abs(outerBoundary.y));
        Vector2 location = RandomNumber(innerBoundary, outerBoundary);
        Instantiate(enemy, new Vector3(location.x + player.position.x, location.y + player.position.y, 0), Quaternion.identity);
    }
    Vector2 RandomNumber(Vector2 innerBoundary, Vector2 outerBoundary)
    {
        Vector2 location;
        if (Random.value < 0.5f)
        {
            if (Random.value > 0.5f) location.x = Random.Range(innerBoundary.x, outerBoundary.x);
            else location.x = Random.Range(-innerBoundary.x, -outerBoundary.x);
            location.y = Random.Range(-outerBoundary.y, outerBoundary.y);
        }
        else
        {
            if (Random.value > 0.5f) location.y = Random.Range(innerBoundary.y, outerBoundary.y);
            else location.y = Random.Range(-outerBoundary.y, -innerBoundary.y);
            location.x = Random.Range(-outerBoundary.x, outerBoundary.x);
        }

            return location;
    }
}
