using System.Threading;
using UnityEngine;

public class Enemy : Entity
{
    public float range;
    [SerializeField] LayerMask playerLayer;
    GameObject target;
    public float rangeBetweenPlayer;
    Rigidbody2D rb;
    bool canAttack = false;

    private void Start()
    {
        rb=GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        attackTimer-= Time.deltaTime;
        FindPlayer();
        Move();
    }
    public override void Attack(Entity entity, float bonusattack = 0)
    {
        entity.GetDamage(attackDamage + bonusattack);
    }
    public void SendAttack()
    {
        if (canAttack && attackTimer <= 0)
        {
            GameObject sendingAttack = AttackParticlePool.instance.Spawn(transform.position, transform.rotation);
            sendingAttack.GetComponent<AttackParticle>().Init(this, target.transform.position);
        }
    }
    public override void Death()
    {
        
    }

    public override void Move()
    {
        if (target != null)
        {
            float dist = Vector2.Distance(transform.position, target.transform.position);
            if (dist > rangeBetweenPlayer)
            {
                int x=0;
                int y = 0;
                if (transform.position.x - target.transform.position.x < 0)
                {
                    x = 1;
                }
                if (transform.position.x - target.transform.position.x > 0)
                {
                    x = -1;
                }
                if (transform.position.y - target.transform.position.y < 0)
                {
                    y = 1;
                }
                if (transform.position.y - target.transform.position.y > 0)
                {
                    y = -1;
                }
                Vector2 dir = new Vector2(x, y).normalized;
                rb.linearVelocity=dir*speed;    
            }
            else
            {
                canAttack = true;
            }
        }  
    }
    void FindPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, range, playerLayer);

        if (hit == null)
        {
            target = null;
            return;
        }

        Player player = hit.GetComponent<Player>();
        if (player == null)
        {
            target = null;
            return;
        }

        target = player.gameObject;
    }

}
