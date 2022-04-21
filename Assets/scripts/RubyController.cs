using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 5;
    public int score = 0;
    public int cogs;
    public GameObject winText;
    public GameObject loseText;
    public Text value;
    public Text Cogs;
    public GameObject projectilePrefab;
    public GameObject healthIncrease;
    public GameObject healthDecrease;
    public bool gameOver;
    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip winSong;
    public AudioClip loseSong;
    public AudioClip moveSound;
    public AudioClip wallThud;
    public float timeRemaining = 30;
    public bool timerIsRunning = false;
    public Text timeText;


    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        value.text = score.ToString();
        winText.SetActive(false);
        loseText.SetActive(false);
        gameOver = false;
        cogs = 4;
        Cogs.text = cogs.ToString();
        if (PlayerPrefs.GetInt("timeAttack", 0) == 1)
        {
            timerIsRunning = true;
        }
        else
        {
            timeText.text = " ";
        }
        Debug.Log(timerIsRunning);


    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }


        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            PlaySound(moveSound);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlaySound(moveSound);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlaySound(moveSound);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            PlaySound(moveSound);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    if (gameOver == false)
                    {
                        character.DisplayDialog();
                    }
                    else if (gameOver == true)
                    {
                        SceneManager.LoadScene("Level2");
                        timerIsRunning = true;
                    }


                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (gameOver == true)
            {
                if (score < 6 && SceneManager.GetActiveScene().name == "Level2")
                {
                    SceneManager.LoadScene("Level2");
                }
                else
                {
                    SceneManager.LoadScene("Main");
                }


            }

        }
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                loseText.SetActive(true);
                Destroy(rigidbody2d);
                gameOver = true;
                PlaySound(loseSong);

            }

        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
       
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            GameObject healthObject = Instantiate(healthDecrease, rigidbody2d.position, Quaternion.identity);
            PlaySound(hitSound);
            animator.SetTrigger("Hit");
        }
        else if (amount > 0)
        {
            GameObject hurtObject = Instantiate(healthIncrease, rigidbody2d.position, Quaternion.identity);
        }


        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

        if (currentHealth <= 0)
        {
            loseText.SetActive(true);
            Destroy(rigidbody2d);
            gameOver = true;
            PlaySound(loseSong);
        }
    }

    public void ChangeScore(int amount)
    {
        timeRemaining = timeRemaining + 5;
        score = score + amount;
        value.text = "Fixed Robots: " + score.ToString();
        if (score >= 6)
        {
            winText.SetActive(true);
            gameOver = true;
            PlaySound(winSong);
        }

    }
    public void ChangeCogs(int amount)
    {
        timeRemaining = timeRemaining + 5;
        cogs = cogs + amount;
        Cogs.text = "Cogs:" + cogs.ToString();


    }

    void Launch()
    {
        if (cogs >= 1)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 300);

            animator.SetTrigger("Launch");

            PlaySound(throwSound);
            cogs = cogs - 1;
            Cogs.text = "Cogs:" + cogs.ToString();
        }

    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        PlaySound(wallThud);

    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void moveBack()
        {
        Vector2 position = rigidbody2d.position;
        position.x = position.x - speed * horizontal * Time.deltaTime;
        position.y = position.y - speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
        Debug.Log("cringe");
    }
    public void OnCollisionExit2D(Collision2D other)
    {
        RubyController controller = other.gameObject.GetComponent<RubyController>();
        if (controller != null)
        {
            controller.speed = 3;
        }
    }


}