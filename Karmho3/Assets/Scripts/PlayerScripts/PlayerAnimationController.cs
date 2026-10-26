using UnityEngine;
using UnityEngine.InputSystem;
using System.Reflection;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private Transform groundCheck;
    private LayerMask groundLayer;

    // Referências dos seus scripts existentes (apenas leitura)
    private Movement movement;
    private AttackPlayer attackPlayer;
    private PlayerStats playerStats;

    // Reflection para ler campos privados com segurança sem alterar seus scripts
    private FieldInfo directionField;
    private FieldInfo groundCheckField;
    private FieldInfo groundLayerField;
    private FieldInfo isDashingField;
    private FieldInfo isKnockedField;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<Movement>();
        attackPlayer = GetComponent<AttackPlayer>();
        playerStats = GetComponent<PlayerStats>();

        // Mapeia os campos privados dos scripts originais
        if (movement != null)
        {
            System.Type moveType = movement.GetType();
            directionField = moveType.GetField("direction", BindingFlags.NonPublic | BindingFlags.Instance);
            groundCheckField = moveType.GetField("groundCheck", BindingFlags.NonPublic | BindingFlags.Instance);
            groundLayerField = moveType.GetField("groundLayer", BindingFlags.NonPublic | BindingFlags.Instance);
            isDashingField = moveType.GetField("isDashing", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        PlayerKnockback knockback = GetComponent<PlayerKnockback>();
        if (knockback != null)
        {
            isKnockedField = knockback.GetType().GetField("isKnocked", BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }

    void Update()
    {
        if (anim == null) return;

        // 1. Atualizar velocidade de movimento no chão (speed)
        Vector2 direction = Vector2.zero;
        if (directionField != null && movement != null)
        {
            direction = (Vector2)directionField.GetValue(movement);
        }
        anim.SetFloat("speed", Mathf.Abs(direction.x));

        // 2. Checar grounded e velocidade vertical
        bool isGrounded = IsGroundedCheck();
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);

        // 3. Checar Dash
        if (isDashingField != null && movement != null)
        {
            bool isDashing = (bool)isDashingField.GetValue(movement);
            anim.SetBool("isDashing", isDashing);
        }

        // 4. Checar Knockback
        if (isKnockedField != null)
        {
            PlayerKnockback knockback = GetComponent<PlayerKnockback>();
            if (knockback != null)
            {
                bool isKnocked = (bool)isKnockedField.GetValue(knockback);
                anim.SetBool("isKnocked", isKnocked);
            }
        }
    }

    // Leitura do estado de chão idêntica ao seu script de Movement
    private bool IsGroundedCheck()
    {
        if (groundCheckField != null && groundLayerField != null && movement != null)
        {
            groundCheck = (Transform)groundCheckField.GetValue(movement);
            groundLayer = (LayerMask)groundLayerField.GetValue(movement);

            if (groundCheck != null)
            {
                return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
            }
        }
        return true;
    }

    // Chama o Trigger de Ataque através do mesmo evento do Player Input
    public void InputAttack(InputAction.CallbackContext context)
    {
        if (context.performed && attackPlayer != null && attackPlayer.canAttack)
        {
            anim.SetTrigger("attack");
        }
    }

    // Chama o Trigger de Cura através do mesmo evento do Player Input
    public void Heal(InputAction.CallbackContext context)
    {
        if (context.performed && playerStats != null)
        {
            anim.SetTrigger("heal");
        }
    }

    // Método público que pode ser chamado se o jogador morrer
    public void TriggerDeath()
    {
        anim.SetBool("isDead", true);
    }
}