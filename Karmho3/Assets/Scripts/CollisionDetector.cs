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
            Camera.main.GetComponent<CameraController>().startShake = true;

            // Agora pega a direção correta
            Knockback(collision.transform, knockbackForce);
        }
    }

    public void Knockback(Transform colliderTransform, float knockbackForce)
    {
        Vector2 direction = (transform.position - colliderTransform.position).normalized;

        selfRig.linearVelocity = direction * knockbackForce;
    }
}