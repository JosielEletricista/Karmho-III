using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEditor.Experimental.GraphView;

public class AttackPlayer : MonoBehaviour
{
    [Header("Referências")]
    public GameObject attackZone;
    private AudioSource attackSource;

    [Header("Config")]
    [SerializeField] private float attackDuration = 0.2f;
    [SerializeField] private float attackCooldown = 0.4f;
    public bool canAttack = true;

    private bool isAttacking = false;

    [Header("Audio")]
    [SerializeField] private AudioClip attackAudio;
    //private float lastDirection = 1f; // 1 = direita, -1 = esquerda


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        attackZone.SetActive(false);
        attackSource = GetComponent<AudioSource>();
    }

    public void InputAttack(InputAction.CallbackContext context)
    {
        if (context.performed && canAttack)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    IEnumerator AttackCoroutine()
    {
        attackSource.PlayOneShot(attackAudio);
        canAttack = false;
        isAttacking = true;

        attackZone.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        attackZone.SetActive(false);
        isAttacking = false;

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
