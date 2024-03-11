using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ManController : MonoBehaviour
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
    private bool hasJumped; // Variable para rastrear si el jugador ha saltado en el último salto
    private int jumpCount; // Contador de saltos

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5f;
        forceMultiplier = 5f;
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        score = 0;
        lifes = 3;
        UpdateScoreText();
        UpdateLifeText();
        msg.enabled = false;
        gameEnded = false;
        hasJumped = false; // Inicialmente el jugador no ha saltado
        jumpCount = 0; // Inicializa el contador de saltos
    }
    void Update()
    {
        if (!gameEnded)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized;
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

            if (Input.GetKeyDown(KeyCode.Space) && !hasJumped)
            {
                rb.AddForce(Vector2.up * (forceMultiplier + 3f), ForceMode2D.Impulse);
                animator.SetBool("IsJumping", true);
                hasJumped = true; 
                jumpCount++; 
            }

            animator.SetBool("IsRunning", Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput));
            animator.SetBool("IsJumping", rb.velocity.y > 0.1f);
            animator.SetBool("IsHitted", false);

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
            else
            {
                animator.SetBool("IsJumping", false);
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
            if (lifes == 0)
            {
                Debug.Log("Game over");
                msg.text = "¡GAME OVER!";
                StartCoroutine(Ending(1));
            }
        }
        if (collision.CompareTag("Trophy"))
        {
            Debug.Log("Collision with Trophy");
            msg.text = "¡YOU WON!";
            StartCoroutine(Ending(1));
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            hasJumped = false;
        }
    }
}
