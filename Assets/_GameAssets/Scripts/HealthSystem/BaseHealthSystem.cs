using UnityEngine;
using UnityEngine.UI;

public class BaseHealthSystem : MonoBehaviour,IHealth
{
    public float maxHealth = 20;
    private float currentHealth;
    public Slider healthBar;
    public bool isBase = true;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if(isBase)
        {
            GameManager.Instance.GameLose();
        }
        else
        {
            GameManager.Instance.GameWin();
        }
        Debug.Log("Base has been destroyed.");
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
