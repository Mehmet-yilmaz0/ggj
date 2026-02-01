using UnityEngine;

public class AttackParticle : MonoBehaviour
{
    Enemy sender;
    Vector2 direction;
    float speed = 5f;
    float maxDistance = 10f;
    Vector2 startPos;

    public void Init(Enemy enemy, Vector2 targetPos)
    {
        sender = enemy;
        startPos = transform.position;
        direction = (targetPos - startPos).normalized;
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        if (Vector2.Distance(startPos, transform.position) > maxDistance)
        {
            AttackParticlePool.instance.Despawn(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            sender.Attack(collision.gameObject.GetComponent<Player>());
            AttackParticlePool.instance.Despawn(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
