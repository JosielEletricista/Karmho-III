using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEditor.Experimental.GraphView;

public class Movement : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private PlayerStats playerStats;

    //Xmove
    [Header("Xmove")]
    [SerializeField] private float speed = 5f; // Speed of movement
    private Vector2 direction; // Direction of movement
    private bool isFacingRight = true; // To track the facing direction of the player

    //Jump
    [Header("Jump")]
    [SerializeField] private float jumpingPower = 16f;
    public bool isJumping;

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
        playerStats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        //Move void
        Move();
        //Flip void
        Flip();

        //Jump
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    #region MoveX/Jump
    
    //Variavel que define se esta no chão ou não, usando um circulo para detectar o chão
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public void InputMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    //Void para movimentação horizontal do jogador
    void Move()
    {
        Vector3 movement = new Vector3(direction.x, 0, 0);
        transform.position += movement * speed * Time.deltaTime;
    }

    public void Jump(InputAction.CallbackContext context)
    {      
        if (context.performed)
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

        if (context.canceled && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);

            coyoteTimeCounter = 0f;
        }
    }

    //Corrotina para evitar que o jogador possa pular novamente enquanto estiver no ar
    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }
    
    private void Flip()
    {
        if (direction.x > 0f && !isFacingRight)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            isFacingRight = true;
        }
        else if (direction.x < 0f && isFacingRight)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            isFacingRight = false;
        }
    }
    #endregion

    #region Dash
    //Void para dash do jogador, usando o shift esquerdo para ativar o dash
    public void InputDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && !isDashing && playerStats.reliquias > 0)
        {
            if(!hasDashingInAir)
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
        GetComponent<AttackPlayer>().canAttack = false; // Impede o jogador de atacar durante o dash

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        
        float dashDirection = isFacingRight ? 1f : -1f; 
        rb.linearVelocityX = dashDirection * dashSpeed;
        
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
        GetComponent<AttackPlayer>().canAttack = true; // Permite o jogador de atacar novamente após o dash

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    #endregion


}
