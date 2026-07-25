using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour
{
    [Header("Heart UI Elements")]
    public Image heart1;
    public Image heart2;
    public Image heart3;

    [Header("Animation Settings")]
    [Tooltip("Duration of the flash and bounce before the heart disappears")]
    public float animDuration = 0.45f;

    [Tooltip("How much larger the heart scales up during the bounce")]
    public float bounceScale = 1.4f;

    private int lastHealth = 3;

    public void UpdateHearts(int currentHealth)
    {
        if (currentHealth < lastHealth)
        {
            Image damagedHeart = GetHeartImageForHealthIndex(lastHealth);

            if (damagedHeart != null && damagedHeart.enabled)
            {
                StartCoroutine(AnimateHeartLoss(damagedHeart, currentHealth));
            }
            else
            {
                SetHeartsInstant(currentHealth);
            }
        }
        else
        {
            SetHeartsInstant(currentHealth);
        }

        lastHealth = currentHealth;
    }

    private Image GetHeartImageForHealthIndex(int healthIndex)
    {
        switch (healthIndex)
        {
            case 3: return heart3;
            case 2: return heart2;
            case 1: return heart1;
            default: return null;
        }
    }

    private void SetHeartsInstant(int health)
    {
        if (heart1 != null) heart1.enabled = health >= 1;
        if (heart2 != null) heart2.enabled = health >= 2;
        if (heart3 != null) heart3.enabled = health >= 3;
    }

    private IEnumerator AnimateHeartLoss(Image heart, int targetHealth)
    {
        Vector3 originalScale = Vector3.one;
        Color originalColor = heart.color;
        // Pure high-intensity white flash
        Color solidWhite = new Color(2f, 2f, 2f, 1f);

        float timer = 0f;
        float blinkInterval = 0.06f; // Time in seconds per blink state
        float blinkTimer = 0f;
        bool isFlashingWhite = false;

        while (timer < animDuration)
        {
            float dt = Time.unscaledDeltaTime;
            timer += dt;
            blinkTimer += dt;

            // 1. Smooth Bounce Scale (Sine Wave Curve)
            float progress = timer / animDuration;
            float scaleFactor = Mathf.Sin(progress * Mathf.PI);
            heart.transform.localScale = Vector3.Lerp(originalScale, originalScale * bounceScale, scaleFactor);

            // 2. High-Contrast Toggle Blink (Snaps cleanly between white and original color)
            if (blinkTimer >= blinkInterval)
            {
                isFlashingWhite = !isFlashingWhite;
                blinkTimer = 0f;
            }

            heart.color = isFlashingWhite ? solidWhite : originalColor;

            yield return null;
        }

        // Reset transform & color
        heart.transform.localScale = originalScale;
        heart.color = originalColor;

        // Hide damaged heart
        heart.enabled = false;

        // Ensure UI state matches total remaining health
        SetHeartsInstant(targetHealth);
    }
}