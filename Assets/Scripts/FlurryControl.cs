using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FlurryControl : MonoBehaviour
{
    private CharacterController characterController;
    public float moveSpeed = 5f;
    public float turnSpeed = 2f;
    public int count;
    public TextMeshProUGUI counter;
    public TextMeshProUGUI question;
    public TextMeshProUGUI question2;
    public TextMeshProUGUI finalMessage;
    public Button[] buttons = new Button[3];
    public Button[] buttons2 = new Button[3];
    public int collected = 1;
    public int maxHealth = 10;
    public int currentHealth;
    public SnowmanScript snow;
    public Rigidbody ob, ob2;
    public CloudScript cloud;

    public HealthBar healthBar;

    public void SetCountText()
    {
        counter.text = "Mini-Crystals Collected: " + count.ToString() + "/60";
    }

    public void SetWinText()
    {
        counter.text = "Find the Master Crystal";
    }

    void SetButtons()
    {
        buttons[0].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Tree";
        buttons[1].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Gate";
        buttons[2].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Candle";
        buttons2[0].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Agree with them";
        buttons2[1].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Defend your friend";
        buttons2[2].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Don't say anything";
    }

    void Start()
    {
        SetCountText();
        characterController = GetComponent<CharacterController>();
        count = 0;
        for(int i = 0; i < 3; i++)
        {
            buttons[i].gameObject.SetActive(false);
            buttons2[i].gameObject.SetActive(false);
        }

        currentHealth = maxHealth;
        healthBar.slider.gameObject.SetActive(false);
        finalMessage.gameObject.SetActive(false);
    }

    void Update()
    {
        Vector3 move = -Input.GetAxis("Vertical") * transform.right;
        characterController.Move(move * moveSpeed * Time.deltaTime);
        transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed, 0);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            other.gameObject.SetActive(false);
            collected++;
            count++;
            if (collected < 26)
            {
                SetCountText();
            }
            else
            {
                SetWinText();
            }
        }
        
        else if (other.gameObject.CompareTag("Unicorn"))
        {
            question.text = "Test of Knowledge \n I'm tall when I'm young, and I'm short when I'm older. What am I?";
            SetButtons();

            question.gameObject.SetActive(true);
            for (int i = 0; i < 3; i++)
            {
                buttons[i].gameObject.SetActive(true);
                if (i == 0 || i == 1)
                {
                    buttons[i].onClick.AddListener(delegate { wrongAnswer(other); });
                }
                else if (i == 2)
                {
                    buttons[i].onClick.AddListener(delegate { rightAnswer(other); });
                }
            }
            
        }
        else if (other.gameObject.CompareTag("Duck"))
        {
            question.gameObject.SetActive(true);
            question.text = "Test of Empathy \n If your friend started saying bad things to you about your best friend in hopes that you'll join a group to hate on them, what should you do?";
            SetButtons();

            question.gameObject.SetActive(true);
            for (int i = 0; i < 3; i++)
            {
                buttons2[i].gameObject.SetActive(true);
                if (i == 0 || i == 2)
                {
                    buttons2[i].onClick.AddListener(delegate { wrongAnswer(other); });
                }
                else if (i == 1)
                {
                    buttons2[i].onClick.AddListener(delegate { rightAnswer(other); });
                }
            }
        }
        else if (other.gameObject.CompareTag("Snowman"))
        {
            collected++;
            snow.currentHealth = 10;
            ob.detectCollisions = false;
            currentHealth = maxHealth;
            question.gameObject.SetActive(true);
            question.text = "Test of Courage \n Defeat the snowman!";
            healthBar.SetActive(true);
            snow.gameObject.GetComponent<Collider>().isTrigger = false;
            StartCoroutine(battle());
        }
        else if (other.gameObject.CompareTag("hit"))
        {
            currentHealth = currentHealth -1;
            healthBar.SetHealth(currentHealth);
        }
        else if (other.gameObject.CompareTag("Cloud"))
        {
            collected++;
            cloud.currentHealth = 10;
            ob2.detectCollisions = false;
            currentHealth = maxHealth;
            question.gameObject.SetActive(true);
            question.text = "Test of Courage \n Defeat the rain cloud!";
            healthBar.SetActive(true);
            StartCoroutine(battle2());
        }
        else if (other.gameObject.CompareTag("Crystal"))
        {
            if (collected >= 26 && count >= 60)
            {
                finalMessage.gameObject.SetActive(true);
                finalMessage.text = "Unlocked \n Advanced to Freedom";
            }
            else if (collected >= 26 && count < 60)
            {
                finalMessage.gameObject.SetActive(true);
                finalMessage.text = "Locked \n Trapped In the Doom World forever";
            }
        }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1f);
        finalMessage.gameObject.SetActive(false);
    }
    IEnumerator battle2()
    {
        yield return new WaitForSeconds(1f);
        question.gameObject.SetActive(false);
        healthBar.SetActive(true);

        yield return new WaitForSeconds(2f);
        cloud.shoot = true;
    }
    IEnumerator battle()
    {
        yield return new WaitForSeconds(1f);
        question.gameObject.SetActive(false);
        healthBar.SetActive(true);

        yield return new WaitForSeconds(2f);
        snow.shoot = true;
    }
    void ifCorrect()
    {
        count += 10/2;
        if (collected < 26) SetCountText();
        else SetWinText();
    }
    void wrongAnswer(Collider other)
    {
        question.text = "You have chosen the wrong answer";
        StartCoroutine(waiting(other));
    }
    void rightAnswer(Collider other)
    {
        question.text = "You are correct";
        ifCorrect();
        StartCoroutine(waiting(other));
    }
    IEnumerator waiting(Collider other)
    {
        collected++;
        yield return new WaitForSeconds(1f);
        question.gameObject.SetActive(false);
        GameObject ob = other.gameObject;
        if (ob != null) ob.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            buttons[i].gameObject.SetActive(false);
            buttons2[i].gameObject.SetActive(false);
        }
        
    }
}
