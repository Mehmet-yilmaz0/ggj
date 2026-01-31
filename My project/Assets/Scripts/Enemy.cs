using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    [Tooltip("kullanýcýyý farketme sýnýrý")]
    public float range;

    [SerializeField] LayerMask playerLayer;
    [SerializeField] float rangeBetweenPlayer = 1.5f;
    [SerializeField] Sprite deathSprite;

    [Tooltip("1 kýrmýzý, 2 mavi, 3 yeþil, 4 siyah")]
    [SerializeField] int enemyType;

    GameObject target;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    bool canAttack = false;
    public bool istouchingPlayer = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isDead) return;

        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
        else
            attackTimer = 0;

        FindPlayer();
        if (enemyType == 4)
        {
            if (attackTimer <= 0)
                Move();
        }
        else Move();

        if (canAttack && attackTimer <= 0)
            Saldir();
    }

    public void Saldir()
    {
        if (enemyType == 2 || enemyType == 3)
            SendAttack();

        if (enemyType == 1 && istouchingPlayer)
            Exploding(target.GetComponent<Entity>());

        if (enemyType == 4 && istouchingPlayer)
            NormalAttack(target.GetComponent<Entity>());
    }

    private void NormalAttack(Entity ent, float bonusAttack = 0)
    {
        Debug.Log("normal");
        Attack(ent, bonusAttack);
    }

    private void Exploding(Entity ent, float bonusAttack = 0)
    {
        Debug.Log("explode");
        Attack(ent, bonusAttack);
        Death();
    }

    public override void Attack(Entity entity, float bonusattack = 0)
    {
        attackTimer = attackSpeed;
        entity.GetDamage(attackDamage + bonusattack);
    }

    public void SendAttack()
    {
        if (!canAttack || attackTimer > 0 || target == null) return;

        attackTimer = attackSpeed;
        GameObject sendingAttack = AttackParticlePool.instance.Spawn(transform.position, Quaternion.identity);
        sendingAttack.GetComponent<AttackParticle>().Init(this, target.transform.position);
    }

    public override void Death()
    {
        if (isDead) return;

        isDead = true;
        rb.linearVelocity = Vector2.zero;
        spriteRenderer.sprite = deathSprite;
        StartCoroutine(DeathAnimation());
    }

    IEnumerator DeathAnimation()
    {
        float duration = 5f;
        float timer = 0f;
        Color color = spriteRenderer.color;
        this.gameObject.GetComponent<Collider2D>().isTrigger = true;
        while (timer <= duration)
        {
            color.a = Mathf.Lerp(1f, 0f, timer / duration);
            spriteRenderer.color = color;
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    public override void Move()
    {
        if (target == null)
        {
            rb.linearVelocity = Vector2.zero;
            canAttack = false;
            return;
        }

        float dist = Vector2.Distance(transform.position, target.transform.position);

        if (dist > rangeBetweenPlayer)
        {
            if(rangeBetweenPlayer!=0)
                canAttack = false;
            Vector2 dir = (target.transform.position - transform.position).normalized;
            rb.linearVelocity = dir * speed;
        }
        else
        {
            canAttack = true;
            rb.linearVelocity = Vector2.zero;
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
        if (collision.gameObject.CompareTag("Player") && (enemyType == 1 || enemyType == 4))
        {
            istouchingPlayer = true;
            canAttack = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (enemyType == 1 || enemyType == 4))
        {
            istouchingPlayer = false;
            canAttack = false;
        }
    }
}
