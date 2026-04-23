using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 6f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator pAni;
    private bool isGrounded;
    private float moveInput;

    private bool Res = false;
    private bool Speed = false;
    private bool Jump = false;

    public int maxCrystals = 5;
    private int currentCrystals = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();
    }


    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveInput = input.x;
        pAni.SetTrigger("Run");
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            pAni.SetTrigger("Jump");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            if (Res)
                Destroy(collision.gameObject);
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (collision.CompareTag("Finish"))
        {
            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }

        if (collision.CompareTag("Enemy"))
        {
            if (Res)
                Destroy(collision.gameObject);
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (collision.CompareTag("Res"))
        {
            Res = true;
            Invoke(nameof(RemoveRes), 5f);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Speed"))
        {
            Speed = true;
            Invoke(nameof(RemoveSpeed), 7f);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Jump"))
        {
            Jump = true;
            Invoke(nameof(RemoveJump), 8f);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Boss"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (collision.CompareTag("Crystal"))
        {
            currentCrystals++;

            GameObject boss = GameObject.FindGameObjectWithTag("Boss");

            if (boss != null)
            {
                boss.transform.localScale -= new Vector3(2f, 2f, 2f);
            }

            if (currentCrystals >= maxCrystals)
            {
                BossDefeated();
            }

            Destroy(collision.gameObject);
        }
    }

    void BossDefeated()
    {
        SceneManager.LoadScene("End");
    }

    void RemoveRes()
    {
        Res = false;
    }

    void RemoveSpeed()
    {
        Speed = false;
    }

    void RemoveJump()
    {
        Jump = false;
    }

    private void Update()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (Res)
        {
            if (moveInput < 0)
                transform.localScale = new Vector3(-3, 3, 1);
            else if (moveInput > 0)
                transform.localScale = new Vector3(3, 3, 1);
        }
        else
        {
            if (moveInput < 0)
                transform.localScale = new Vector3(-2, 2, 1);
            else if (moveInput > 0)
                transform.localScale = new Vector3(2, 2, 1);
        }

        if (Speed)
        {
            moveSpeed = 9f;
        }
        else
        {
            moveSpeed = 6f;
        }

        if (Jump)
        {
            jumpForce = 12f;
        }
        else
        {
            jumpForce = 6f;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}