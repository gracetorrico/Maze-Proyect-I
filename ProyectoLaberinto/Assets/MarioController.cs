using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class MarioController : MonoBehaviour
{
    public float moveSpeed;
    public float forceMultiplier;
    private Animator animator;
    private Vector3 originalScale;
    private Rigidbody2D rb;
    private int score;
    private int lifes;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI msg;
    private Vector3 initialPosition;
    private bool gameEnded;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5f;
        forceMultiplier = 5f;
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        rb.gravityScale = 0f;
        score = 0;
        lifes = 3;
        UpdateScoreText();
        UpdateLifeText();
        msg.enabled = false;
        gameEnded = false;
    }
    void FixedUpdate()
    {
        if (!gameEnded)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized;

            animator.SetBool("IsLateralWalking", Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput));
            animator.SetBool("IsUpWalking", verticalInput > 0);
            animator.SetBool("IsDownWalking", verticalInput < 0);
            rb.velocity = movement * moveSpeed;

            if (movement != Vector2.zero)
            {
                if (horizontalInput > 0) // Derecha
                {
                    transform.localScale = originalScale;
                }
                else if (horizontalInput < 0) // Izquierda
                {
                    transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fruit"))
        {
            Debug.Log("Collision with Fruit");
            score += 100; 
            UpdateScoreText(); 
        }
        if (collision.CompareTag("Saw"))
        {
            Debug.Log("Collision with Saw");
            score -= 50;
            lifes -= 1;
            UpdateLifeText();
            UpdateScoreText();
            transform.position = initialPosition;
            if (lifes==0)
            {
                Debug.Log("Game over");
                msg.text = "¡GAME OVER!";
                StartCoroutine(Ending(1));
            }
        }
        if(collision.CompareTag("Trophy"))
        {
            Debug.Log("Collision with Trophy");
            msg.text = "¡YOU WON!";
            StartCoroutine(Ending(3));
        }
    }

    IEnumerator Ending(int i)
    {
        msg.enabled = true;
        gameEnded = true;
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(i);
    }


    void UpdateScoreText()
    {
        scoreText.text = "SCORE: " + score.ToString();
    }
    void UpdateLifeText()
    {
        lifeText.text = "LIFE: " + lifes.ToString();
    }
}
