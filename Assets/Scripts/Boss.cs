using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Boss : MonoBehaviour
{
    [SerializeField] float averageTimeToFire;
    [SerializeField] int averageShotCount;
    [SerializeField] int maxHealth = 1000;

    [SerializeField] private GameObject projectile;

    [SerializeField] private GameObject healthUI;

    private bool readyToFire;
    private Vector3 fireVector;
    private int health;

    public void ApplyDamage(float damage)
    {
        health -= (int)damage;
        healthUI.GetComponent<TextMeshProUGUI>().text = health.ToString() + " / " + maxHealth.ToString();
        if (health <= 0)
        {
            SceneManager.LoadScene("Victory");
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        fireVector = transform.forward;
        StartCoroutine(WaitForSecondsToFire(averageTimeToFire));
        health = maxHealth;
        healthUI.GetComponent<TextMeshProUGUI>().text = health.ToString() + " / " + maxHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToFire)
        {
            float factor = Random.Range(1.0f, 3.0f);
            float angle = 360.0f / (averageShotCount * factor);
            
            for (int i = 0; i < averageShotCount * factor; i++)
            {
                fireVector = Quaternion.AngleAxis(angle, Vector3.up) * fireVector;

                GameObject fired = Instantiate(projectile, Vector3.up, Quaternion.identity);
                fired.GetComponent<Projectile>().target = fireVector + Vector3.up;

            }

            StartCoroutine(WaitForSecondsToFire(averageTimeToFire * factor));
        }
    }

    IEnumerator WaitForSecondsToFire(float time)
    {
        readyToFire = false;
        // yield on a new YieldInstruction that waits for time seconds.
        yield return new WaitForSeconds(time);
        readyToFire = true;
    }
}
