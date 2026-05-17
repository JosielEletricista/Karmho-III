using UnityEngine;
using System.Collections;

public class CollisionDetector : MonoBehaviour
{
    //Variáveis para o knockback
    private Rigidbody2D selfRig;
    public float knockbackForce = 5f;

    //Variáveis para o flash de dano
    private SpriteRenderer sprite;
    private Color flashColor = Color.white;
    private float flashTime = 0.1f;

    private Color originalColor;

    void Start()
    {
        selfRig = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Camera.main.GetComponent<CameraController>().startShake = true;


            Knockback(player.transform, knockbackForce); //Kockback apos ser atingido
            Camera.main.GetComponent<Hitstop>().Stop(0.2f); //Hitstop apos ser atingido
            StartCoroutine(HitFlash()); //Inicia o flash de dano
        }
    }

    //void para aplicar o knockback ao personagem, recebendo a posição do inimigo e a força do knockback
    public void Knockback(Transform colliderTransform, float knockbackForce)
    {
        Vector2 direction = (transform.position - colliderTransform.position).normalized;

        selfRig.linearVelocity = direction * knockbackForce;
    }

    IEnumerator HitFlash()
    {
        sprite.color = flashColor; //Muda a cor do sprite para a cor de flash
        yield return new WaitForSeconds(flashTime);
        sprite.color = originalColor; //Restaura a cor original do sprite
    }

}