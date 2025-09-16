using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public GameObject gameOverUI;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        if (gameOverUI) gameOverUI.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        UpdateHealthUI();
    }

    public void TakePercentageDamage(float percent)
    {
        int damage = Mathf.RoundToInt(maxHealth * percent);
        TakeDamage(damage);
    }

    public void HealPercentage(float percent)
    {
        int healAmount = Mathf.RoundToInt(maxHealth * percent);
        Heal(healAmount);
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    void Die()
    {
        if (gameOverUI) gameOverUI.SetActive(true);
        GetComponent<PlayerController2D>().enabled = false;
    }

    public bool CanHeal()
    {
        return currentHealth < maxHealth;
    }


    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}