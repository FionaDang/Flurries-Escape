using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmanScript : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    public HealthBar healthBar;
    public bool shoot = false;
    public Rigidbody snowball;
    public FlurryControl flurry;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        shoot = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sparkle"))
        {
            currentHealth = currentHealth - 1;
            healthBar.SetHealth(currentHealth);
        }
    }
    public void Update()
    {
        if (shoot)
        {
            StartCoroutine(fire());
        }
    }
    IEnumerator fire()
    {
        shoot = false;
        Rigidbody newSnow = Instantiate(snowball) as Rigidbody;
        newSnow.AddForce(transform.forward * 500);
        if (flurry.currentHealth > 0 && currentHealth <= 0)
        {
            flurry.question.gameObject.SetActive(true);
            flurry.question.text = "Congrats, you have defeated the Cardboard Snowman";
            flurry.count += 10;
            yield return new WaitForSeconds(1f);
            end();
        }
        else if (currentHealth > 0 && flurry.currentHealth <= 0)
        {
            flurry.question.gameObject.SetActive(true);
            flurry.question.text = "You have been defeated by the Cardboard Snowman";
            yield return new WaitForSeconds(1f);
            end();
        }
        else if (currentHealth == 0 && flurry.currentHealth == 0)
        {
            flurry.question.gameObject.SetActive(true);
            flurry.question.text = "It was a bloody battle, but you died together.";
            flurry.count += 5;
            yield return new WaitForSeconds(1f);
            end();
        }
        else
        {
            yield return new WaitForSeconds(3f);
            shoot = true;
        }
    }
    private void end()
    {
        if (flurry.collected < 26) flurry.SetCountText();
        else flurry.SetWinText();

        gameObject.SetActive(false);
        gameObject.transform.parent.gameObject.SetActive(false);
        healthBar.SetActive(false);
        flurry.healthBar.SetActive(false);
        flurry.question.gameObject.SetActive(false);
    }
}
