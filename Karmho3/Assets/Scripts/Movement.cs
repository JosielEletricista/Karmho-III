using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //Xmove
    [Header("Xmove")]
    [SerializeField] private float speed = 5f; // Speed of movement

    //Jump
    [Header("Jump")]
    [SerializeField] private float jumpingPower = 16f;
    private bool isJumping;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    //Dash
    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    public bool canDash = true;
    private bool isDashing;
    private bool hasDashingInAir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Move void
        Move();

        //Dash void
        Dash();

        //Jumping
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);

            jumpBufferCounter = 0f;

            StartCoroutine(JumpCooldown());
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);

            coyoteTimeCounter = 0f;
        }
    }

    #region MoveX/Jump

    //Variavel que define se esta no chão ou não, usando um circulo para detectar o chão
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    //Void para movimentação horizontal do jogador
    void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        transform.position += movement * speed * Time.deltaTime;
    }

    //Corrotina para evitar que o jogador possa pular novamente enquanto estiver no ar
    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }
    #endregion

    #region Dash
    //Void para dash do jogador, usando o shift esquerdo para ativar o dash
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
        {
            if( !hasDashingInAir)
            {
                StartCoroutine(DashCoroutine());
            }

        }
    }

    //Corrotina para o dash do jogador, desativando a gravidade e a velocidade horizontal durante o dash, e depois reativando a gravidade e a velocidade horizontal após o dash
    IEnumerator DashCoroutine()
    {
       canDash = false;
       isDashing = true;
        
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        
        rb.linearVelocityX = transform.localScale.x * dashSpeed;
        /*
        if(rb.linearVelocity.y > 0f)
        {
            hasDashingInAir = true;
        }
        */

        yield return new WaitForSeconds(dashDuration);
        rb.gravityScale = originalGravity;
        rb.linearVelocityX = 0f;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    #endregion


}
