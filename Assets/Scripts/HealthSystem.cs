using UnityEngine;

public class HealthSystem
{
    private int health;
    private int maxHealth;

    public HealthSystem(int maxHealth)
    {
        this.maxHealth = maxHealth;
        this.health = maxHealth;
    }

    public int GetHealth() => health;
    public int GetMaxHealth() => maxHealth;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
    }

    public void Heal(int amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
    }

    public bool IsAlive() => health > 0;
    public bool IsDead() => health <= 0;
}