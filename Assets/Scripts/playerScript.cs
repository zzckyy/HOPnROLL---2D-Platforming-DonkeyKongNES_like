using UnityEngine;

public class playerScript : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpHeight = 8f;

    Rigidbody2D rb;
    Transform pos;

    enum playerState
    {
        Idle,
        Run,
        Jump,
        Fall
    }

    playerState currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentState = playerState.Idle;
        pos = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        stateHandler();
    }

    void stateHandler()
    {
        switch(currentState)
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

            
        }
    }

    void playerIdle()
    {
        float input = Input.GetAxisRaw("Horizontal");

        if (!IsGrounded())
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

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    void playerRun()
    {
        float input = Input.GetAxisRaw("Horizontal");

        if (!IsGrounded())
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

        rb.linearVelocity = new Vector2(input * moveSpeed, rb.linearVelocity.y);
    }

    void playerJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
        currentState = playerState.Fall;
    }

    void playerFall()
    {
        if (IsGrounded())
        {
            currentState = playerState.Idle;
        }
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(pos.position, Vector2.down, 0.1f);
    }
}
