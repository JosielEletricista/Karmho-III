using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private Rigidbody2D selfRig;
    public float knockbackForce = 5f;

    void Start()
    {
        selfRig = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Camera.main.GetComponent<CameraController>().startShake = true;


            Knockback(player.transform, knockbackForce); //Kockback apos ser atingido
            Camera.main.GetComponent<Hitstop>().Stop(0.2f); //Hitstop apos ser atingido
        }
    }

    //void para aplicar o knockback ao personagem, recebendo a posição do inimigo e a força do knockback
    public void Knockback(Transform colliderTransform, float knockbackForce)
    {
        Vector2 direction = (transform.position - colliderTransform.position).normalized;

        selfRig.linearVelocity = direction * knockbackForce;
    }
}