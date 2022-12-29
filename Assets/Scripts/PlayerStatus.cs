using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PlayerStats
{
    public static int Health { get; set; }
    public static int Damage { get; set; }
    public static float Speed { get; set; }
}

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] public int health = 20;
    [SerializeField] public int maxHealth = 20;
    [SerializeField] public float speed = 10.0f;
    [SerializeField] public int damage = 10;

    [SerializeField] float invincibilityTime = 1.0f;
    [SerializeField] private GameObject healthUI;
    [SerializeField] private GameObject currencyUI;

    [SerializeField] bool inBoss;

    private int currency;

    private bool vulnerable;

    public void Death()
    {
        Debug.Log("Player died!");
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("SampleScene"))
        {
            PlayerStats.Health = maxHealth;
            PlayerStats.Damage = damage;
            PlayerStats.Speed = speed;
            SceneManager.LoadScene("Boss");
        }
        else
        {
            SceneManager.LoadScene("GameOver");
        }
        return;
    }

    public void ApplyDamage(float damage)
    {
        if (vulnerable)
        {
            health -= (int)damage;
            if (health <= 0)
            {
                healthUI.GetComponent<TextMeshProUGUI>().text = "0" + " / " + maxHealth.ToString();
                Death();
                return;
            }
            healthUI.GetComponent<TextMeshProUGUI>().text = health.ToString() + " / " + maxHealth.ToString();
            Debug.Log("Player Hit!");
            StartCoroutine(Invincibility(invincibilityTime));
        }
    }

    public void UpgradeHealth()
    {
        if (currency >= 20)
        {
            health += 10;
            maxHealth += 10;
            currency -= 20;
            healthUI.GetComponent<TextMeshProUGUI>().text = health.ToString() + " / " + maxHealth.ToString();
            currencyUI.GetComponent<TextMeshProUGUI>().text = currency.ToString();
        }
    }

    public void UpgradeSkill()
    {
        if (currency >= 20)
        {
            speed += 0.7f;
            damage += 3;
            currency -= 20;
            currencyUI.GetComponent<TextMeshProUGUI>().text = currency.ToString();
        }
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
        currencyUI.GetComponent<TextMeshProUGUI>().text = currency.ToString();
    }

    public void RemoveCurrency(int amount)
    {
        currency -= amount;
        if (currency < 0)
        {
            currency = 0;
        }
        currencyUI.GetComponent<TextMeshProUGUI>().text = currency.ToString();
    }

    void Start()
    {
        if (inBoss)
        {
            maxHealth = PlayerStats.Health;
            health = PlayerStats.Health;
            damage = PlayerStats.Damage;
            speed = PlayerStats.Speed;
        }

        currency = 0;
        vulnerable = true;

        healthUI.GetComponent<TextMeshProUGUI>().text = health.ToString() + " / " + maxHealth.ToString();
        currencyUI.GetComponent<TextMeshProUGUI>().text = currency.ToString();
    }

    IEnumerator Invincibility(float time)
    {
        vulnerable = false;
        // yield on a new YieldInstruction that waits for time seconds.
        yield return new WaitForSeconds(time);
        vulnerable = true;
    }
}
