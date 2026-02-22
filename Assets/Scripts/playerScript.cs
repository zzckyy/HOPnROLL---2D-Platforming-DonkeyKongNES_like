using UnityEngine;

public class playerScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 3f;
    public float jumpHeight = 8f;

    [Header("Ground & Climb Check Sensor")]
    public Transform groundCheck;
    public float Radius = 0.1f;
    public LayerMask groundLayer;
    public LayerMask PanjatLayer;
    public bool canManjat;

    public float scaleX;
    bool isFlip;
    Rigidbody2D rb;
    Transform pos;
    Animator anim;

    bool isGrounded;

    enum playerState
    {
        Idle,
        Run,
        Jump,
        Fall,
        Climb
    }

    playerState currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentState = playerState.Idle;
        pos = GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        stateHandler();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, Radius, groundLayer);
        canManjat = Physics2D.OverlapCircle(pos.position, Radius, PanjatLayer);
    }

    void stateHandler()
    {
        switch (currentState)
        {
            case playerState.Idle:
                playerIdle();
                break;

            case playerState.Run:
                playerRun();
                break;

            case playerState.Jump:
                playerJump();
                break;

            case playerState.Fall:
                playerFall();
                break;

            case playerState.Climb:
                playerClimb();
                break;


        }
    }

    void playerIdle()
    {
        float inputPanjat = Input.GetAxisRaw("Vertical");
        float input = Input.GetAxisRaw("Horizontal");

        if (!isGrounded)
        {
            currentState = playerState.Fall;
            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            currentState = playerState.Jump;
            return;
        }

        if (Mathf.Abs(input) > 0)
        {
            currentState = playerState.Run;
        }

        if (Mathf.Abs(inputPanjat) > 0 && canManjat == true)
        {
            currentState = playerState.Climb;
        }

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    void playerRun()
    {
        float inputPanjat = Input.GetAxisRaw("Vertical");
        float input = Input.GetAxisRaw("Horizontal");

        if (!isGrounded)
        {
            currentState = playerState.Fall;
            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            currentState = playerState.Jump;
            return;
        }

        if (Mathf.Abs(input) == 0)
        {
            currentState = playerState.Idle;
        }

        if (input > 0)
        {
            isFlip = false;
            pos.localScale = new Vector3(scaleX, pos.localScale.y, pos.localScale.z);
        }

        else if (input < 0)
        {
            isFlip = true;
            pos.localScale = new Vector3(-scaleX, pos.localScale.y, pos.localScale.z);
        }

        rb.linearVelocity = new Vector2(input * moveSpeed, rb.linearVelocity.y);
    }

    void playerJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
        currentState = playerState.Fall;
    }

    void playerFall()
    {
        if (isGrounded == true)
        {
            currentState = playerState.Idle;
        }
    }

    void playerClimb()
    {   
        float inputPanjat = Input.GetAxisRaw("Vertical");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, inputPanjat * moveSpeed);

        if (Mathf.Abs(inputPanjat) == 0)
        {
            currentState = playerState.Idle;
        }


    }
}
