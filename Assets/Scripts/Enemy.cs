using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    public float speed = 10f;
    private Universal universalScript;
    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null) player = playerObject.GetComponent<Transform>();
        else Debug.LogError("No player was found");
        GameObject universalObject = GameObject.FindWithTag("Universal");
        universalScript = universalObject.GetComponent<Universal>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        speed = universalScript.enemySpeed;
    }
}
