using UnityEngine;
using UnityEngine.Events;

public class SoldierHealthSystem : MonoBehaviour, IHealth
{
    public float maxHealth = 1;
    private float currentHealth;
    public UnityEvent deathEvent,takeDamageEvent;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        { 
            takeDamageEvent.Invoke();
        }
    }

    private void Die()
    {
        Debug.Log("Soldier has died.");
        deathEvent.Invoke();
        Destroy(gameObject,1f);
    }
    public float getCurrentHealth()
    {
        return currentHealth;
    }
}
