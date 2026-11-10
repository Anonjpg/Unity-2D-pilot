using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 25;
    public float bulletLifetime = 3f;


    [Header("ParticalEffect")]
    public GameObject hitEffectPrefab;

    private bool hasHit = false;

    void Start()
    {
        Destroy(gameObject, bulletLifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("bullet collided with" + other.name);
        if (hasHit)
        {
            return;
        }

        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            Debug.Log("enemy detected");
            hasHit = true;

            Vector2 hitPosition = other.ClosestPoint(transform.position);

            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, hitPosition, Quaternion.identity);
            }

            enemyHealth.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
