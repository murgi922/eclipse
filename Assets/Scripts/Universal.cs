using System.Runtime.CompilerServices;
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

    public int enemyCount = 0;

    void Start()
    {
        outerBoundary.x = innerBoundary.x + boundaryThickness;
        outerBoundary.y = innerBoundary.y + boundaryThickness;
    }

    void Update()
    {
        
        timeElapsed += Time.deltaTime;
        if (timeElapsed > timeDelay && enemyCount <= totalEnemy)
        {
            SpawnEnemy(enemyPrefab, innerBoundary, outerBoundary);
            enemyCount++;
            timeElapsed = 0.0f;
        }
    }
    void SpawnEnemy(GameObject enemy, Vector2 location)
    {
        Instantiate(enemy, new Vector3(location.x, location.y, 0), Quaternion.identity);
    }
    void SpawnEnemy(GameObject enemy, Vector2 innerBoundary, Vector2 outerBoundary)
    {
        innerBoundary = new Vector2(Mathf.Abs(innerBoundary.x),  Mathf.Abs(innerBoundary.y));
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
