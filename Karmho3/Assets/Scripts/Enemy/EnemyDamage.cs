using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            PlayerKnockback playerKnockback = collision.gameObject.GetComponent<PlayerKnockback>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageAmount);
                playerKnockback.ApplyKnockBack(transform.position);
            }
        }
    }
}
