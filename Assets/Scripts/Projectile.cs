using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform player;
    public float projectileSpeed = 10.0f;
    private Vector3 initialDirection;
    private Universal universalScript;

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Orientation");
        GameObject universalObject = GameObject.FindWithTag("Universal");
        universalScript = universalObject.GetComponent<Universal>();
        player = playerObject.GetComponent<Transform>();
        initialDirection = player.up;
    }
    void Update()
    {
        transform.position += Time.deltaTime * projectileSpeed * 10f * initialDirection;
        if (Vector3.Distance(transform.position, player.position) > 20.0f) Destroy(this.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
        universalScript.enemyCount--;
        universalScript.deSpawnedEnemy++;
        Destroy(this.gameObject);
    }
}
