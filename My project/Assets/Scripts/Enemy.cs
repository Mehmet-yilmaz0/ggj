using System;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using UnityEngine;

public class Enemy : Entity
{
    [Tooltip("kullanýcýyý farketme sýnýrý")]
    public float range;
    [SerializeField] LayerMask playerLayer;
    GameObject target;
    float rangeBetweenPlayer;
    Rigidbody2D rb;
    bool canAttack = false;
    [SerializeField] Sprite deathSprite;
    SpriteRenderer spriteRenderer;
    [Tooltip("1 kýrmýzý, 2 mavi, 3 yeþil, 4 siyah")]
    [SerializeField]int enemyType;
    bool istouchingPlayer = false;
    private void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (!isDead)
        {
            attackTimer -= Time.deltaTime;
            FindPlayer();
            Move();
            if (canAttack && attackTimer<=0) Saldir();
        }
    }
    public void Saldir()
    {
        if (enemyType == 3 || enemyType == 2) SendAttack();
        if (enemyType == 1 && istouchingPlayer) Exploding(target.GetComponent<Entity>());
        if (enemyType == 4 && istouchingPlayer) NormalAttack(target.GetComponent<Entity>());
    }

    private void NormalAttack(Entity ent, float bonusAttack=0)//dokunma hasarý
    {
        Attack(ent, bonusAttack);
    }

    private void Exploding(Entity ent, float bonusAttack = 0)//patlama hasarý
    {
        Attack(ent, bonusAttack);
        Death();//obj pooling deðiþtirme yeri
    }

    public override void Attack(Entity entity, float bonusattack = 0)
    {
        attackTimer = attackSpeed;
        entity.GetDamage(attackDamage + bonusattack);
    }
    public void SendAttack()//uzaktan hasar
    {
        if (canAttack && attackTimer <= 0 )
        {
            GameObject sendingAttack = AttackParticlePool.instance.Spawn(transform.position, transform.rotation);
            sendingAttack.GetComponent<AttackParticle>().Init(this, target.transform.position);
        }
    } 
    public override void Death()
    {
        isDead = true;
        health = 0;
        spriteRenderer.sprite = deathSprite;
        StartCoroutine(DeathAnimation());
    }
    IEnumerator DeathAnimation()
    {
        float duration = 2f;
        float timer = 0f;
        Color color = spriteRenderer.color;
        while (timer <= duration)
        {
            color.a= Mathf.Lerp(1f, 0f, timer / duration);
            spriteRenderer.color=color;
            yield return null;
        }
        Destroy(this);//object pooling kullanýlacaksa deðiþtir-despawn noktasý
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (enemyType==1 || enemyType==4))
        {
            istouchingPlayer = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (enemyType == 1 || enemyType == 4))
        {
            istouchingPlayer = false;
        }
    }

}
