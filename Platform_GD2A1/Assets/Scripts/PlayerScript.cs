using Mono.Cecil;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerScript : MonoBehaviour
{
    public int speed = 2;
    private Vector2 move;
    private Rigidbody2D rb;
    private int direction = 1;
    private Animator animator;
    public float jumpHeight = 2.4f;
    private bool isJumping = false;
    private int jumpCount = 0;
    private int score = 0;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameManager gm;
    private int lives=3;
    private Vector2 lastPosition;

     void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lastPosition = rb.transform.position;
    }
    private void Fire()
    {
        GameObject go = Instantiate(projectilePrefab,rb.transform.position, Quaternion.identity);
        Projectile pro = go.GetComponent<Projectile>();
        pro.Launch(new Vector2(direction, 0), 300);
    }
    // Update is called once per frame
    void Update()
    {
        move = InputSystem.actions["Move"].ReadValue<Vector2>();

        if (InputSystem.actions["Jump"].IsPressed() && jumpCount <= 2)
        {
            InputSystem.actions["Jump"].Reset();
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(new Vector2(0,
                Mathf.Sqrt(-2 * Physics2D.gravity.y * jumpHeight)), ForceMode2D.Impulse);
            isJumping = true;
            jumpCount++;
        }
        if (InputSystem.actions["Attack"].IsPressed())
        {
            InputSystem.actions["Attack"].Reset();
            Fire();
        }

        if(move.x != 0)
        {
            direction = move.x < 0 ? -1 : 1;
            animator.SetInteger("Direction", direction);

        }
        animator.SetFloat("Move", move.x);
        Debug.Log(jumpCount);
    }

    void FixedUpdate()
    {
        
        Vector2 position = rb.transform.position;
        position.x += (move.x*speed) * Time.fixedDeltaTime;
        rb.transform.position = position;
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
            jumpCount = 0;
        }
        if (collision.gameObject.tag == "EnemyProjectile")
        {
            lives--;
            gm.updateLives(lives);
            rb.transform.position = lastPosition;
        }
    }

    public void AddCollectible()
    {
        score++;
        gm.UpdateScore(score);
    }
}
