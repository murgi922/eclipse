using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;

    [Header("Movement")]
    public float speed = 1.5f;

    private Universal universalScript;
    private Player playerScript;

    [Header("Player Damage System")]
    public float timeDelayBetwnDamage = 1.0f;
    private float elapsedTime = 0.0f;

    [Header("Polygon & Visual Settings")]
    [Tooltip("Ordered sprites: Index 0 = Triangle, 1 = Square, 2 = Pentagon, etc.")]
    public Sprite[] polygonSprites;

    [Tooltip("Base scale forced for all enemy shapes")]
    public float baseScaleSize = 0.1f;

    private SpriteRenderer spriteRenderer;

    // Current shape configuration (3 = Triangle, 4 = Square, etc.)
    private int currentSides = 3;
    private bool isHitAnimating = false;
    private Vector3 baseScaleVector;

    [Header("Hit Bounce & Flash Settings")]
    public float animDuration = 0.18f;
    public float bounceScale = 1.35f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseScaleVector = new Vector3(baseScaleSize, baseScaleSize, baseScaleSize);
        transform.localScale = baseScaleVector;
    }

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

    /// <summary>
    /// Set initial polygon sides on spawn (3 = Triangle, 4 = Square, etc.)
    /// </summary>
    public void SetSides(int sides)
    {
        currentSides = Mathf.Clamp(sides, 3, 8);
        UpdateSprite();
        transform.localScale = baseScaleVector;
    }

    private void UpdateSprite()
    {
        int spriteIndex = currentSides - 3;
        if (polygonSprites != null && spriteIndex >= 0 && spriteIndex < polygonSprites.Length)
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = polygonSprites[spriteIndex];
        }
    }

    /// <summary>
    /// Called when hit by a projectile. Handles bounce/flash animation and shape degradation.
    /// </summary>
    public void TakeHit()
    {
        if (currentSides > 3)
        {
            if (!isHitAnimating)
            {
                StartCoroutine(HitBounceFlashAndDegrade());
            }
        }
        else
        {
            // Triangle hit -> Destroy enemy
            DestroyEnemySelf();
        }
    }

    private IEnumerator HitBounceFlashAndDegrade()
    {
        isHitAnimating = true;

        Color originalColor = Color.white;
        // Pure high-intensity white flash
        Color flashWhite = new Color(2f, 2f, 2f, 1f);

        float timer = 0f;

        // 1. Flash white & Bounce Up
        while (timer < animDuration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / animDuration);

            // Sine wave for smooth bounce scaling
            float scaleFactor = Mathf.Sin(progress * Mathf.PI);
            transform.localScale = Vector3.Lerp(baseScaleVector, baseScaleVector * bounceScale, scaleFactor);

            // Flash white during peak bounce
            spriteRenderer.color = (progress < 0.6f) ? flashWhite : originalColor;

            yield return null;
        }

        // 2. Degrade to lower polygon sprite after flash completes
        currentSides--;
        UpdateSprite();

        // 3. Reset transform & color
        transform.localScale = baseScaleVector;
        spriteRenderer.color = originalColor;
        isHitAnimating = false;
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

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

        DestroyEnemySelf();
    }

    private void DestroyEnemySelf()
    {
        if (universalScript != null)
        {
            universalScript.enemyCount--;
            universalScript.deSpawnedEnemy++;
        }

        Destroy(gameObject);
    }
}