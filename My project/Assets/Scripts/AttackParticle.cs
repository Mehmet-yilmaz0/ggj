using UnityEngine;

public class AttackParticle : MonoBehaviour
{
    Enemy sender;
    Vector2 direction;
    float speed = 5f;
    public void Init(Enemy enemy,Vector2 dir)
    {
        sender = enemy;
        direction = dir;
    }
    private void Update()
    {
        if (direction != null)
        {
            Vector2 control=sender.transform.position-transform.position;
            if (control.x < 10 && control.y < 10) 
            {
                transform.position += (Vector3)(direction * speed * Time.deltaTime);
            }
            else
            {
                AttackParticlePool.instance.Despawn(this.gameObject);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            sender.Attack(collision.gameObject.GetComponent<Player>());
    }
}
