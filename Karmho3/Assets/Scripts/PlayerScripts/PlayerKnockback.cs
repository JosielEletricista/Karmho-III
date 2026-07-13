using UnityEngine;
using System.Collections;

public class PlayerKnockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isKnocked = false;
    private Movement movementScript;
    private AudioSource audioSource;
    [SerializeField] private int knockbackForce = 10;
    [SerializeField] private float knockbackTime = 0.5f;
    [SerializeField] private float verticalMultiplier = 1.3f;
    [SerializeField] private AudioClip knockbackSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        movementScript = GetComponent<Movement>();
        audioSource = GetComponent<AudioSource>();
    }

    
    public void ApplyKnockBack(Vector3 attackerPosition)
    {
        if (!isKnocked)
        {
            StartCoroutine(KnockbackRoutine(attackerPosition));
        }
    }

    private IEnumerator KnockbackRoutine(Vector3 attackerPosition)
    {
        isKnocked = true;
        audioSource.PlayOneShot(knockbackSound); // Toca o som de knockback

        movementScript.enabled = false; // Desativa o script de movimento para evitar conflitos durante o knockback
        Camera.main.GetComponent<CameraController>().startShake = true;
        Vector2 direction = (transform.position - attackerPosition).normalized;

        // Opcional: dá um boost vertical para ficar mais satisfatório em platformer
        if (direction.y > -0.2f)// evita knockback muito forte pra baixo
        {
            direction.y *= verticalMultiplier;
        }
        
        Camera.main.GetComponent<Hitstop>().Stop(0.2f); //Hitstop apos ser atingido
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackTime);

        movementScript.enabled = true; // Reativa o script de movimento após o knockback
        isKnocked = false;
    }
}
