using NUnit;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    public List<Mask> masks;
    public Mask wearedMask;
    [SerializeField] LayerMask enemyLayer;
    Rigidbody2D rb;
    int lastButton = 0;//1sað,2sol,3yukarý,4aþaðý
    [SerializeField] GameObject noMaskAttackSprite;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (!isDead)
        {
            ChangeMaskKey();
            if (attackTimer <= 0f)BaseAttack();
            Move();
            if (attackTimer > 0)
                attackTimer -= Time.deltaTime;
            else attackTimer = 0;
            UseSkill();
        }
        
    }
    void UseSkill()
    {
        if (wearedMask != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                wearedMask.UseSkill('e');
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                wearedMask.UseSkill('q');
            }
        }
    }
    void ChangeMaskKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeMask(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeMask(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeMask(3);
        }
    }
    void ChangeMask(int index)
    {
        switch (index)
        {
            case 1:
                if (masks[0] != null)
                {
                    wearedMask.UnWearMask();
                    wearedMask = masks[0];
                    wearedMask.WearMask();
                }
                break;
            case 2:
                if (masks[1] != null)
                {
                    wearedMask.UnWearMask();
                    wearedMask = masks[1];
                    wearedMask.WearMask();
                }
                break;
            case 3:
                if (masks[2] != null)
                {
                    wearedMask.UnWearMask();
                    wearedMask = masks[2];
                    wearedMask.WearMask();
                }
                break;
        }
    }

    public override void Attack(Entity ent, float bonusattack = 0)
    {
        ent.GetDamage(attackDamage+bonusattack);
    }
    public void TekliHasar(Entity ent,float bonusattack= 0)
    {
        attackTimer = attackSpeed;
        Attack(ent, bonusattack);
    }
    public void AlanHasar(List<Entity> ents, float bonusattack=0)
    {
        attackTimer = attackSpeed;
        foreach(Entity ent in ents)
            Attack(ent,bonusattack);
    }

    public override void Move()
    {
        float x = 0f;
        float y = 0f;

        if (Input.GetKey(KeyCode.A)) { x = -1f; lastButton = 2; }
        if (Input.GetKey(KeyCode.D)) {x = 1f; lastButton = 1; }
        if (Input.GetKey(KeyCode.S)) {y = -1f; lastButton = 4; }
        if (Input.GetKey(KeyCode.W)) { y = 1f; lastButton = 3; }

        Vector2 dir = new Vector2(x, y).normalized;
        rb.linearVelocity = dir * speed;
    }
    public float dashForce = 8f;

    public void Dash()
    {
        Vector2 dir = Vector2.zero;

        if (lastButton == 3) dir = Vector2.up;
        else if (lastButton == 2) dir = Vector2.left;
        else if (lastButton == 4) dir = Vector2.down;
        else if (lastButton == 1) dir = Vector2.right;

        if (dir == Vector2.zero) return;

        rb.linearVelocity = Vector2.zero; 
        rb.AddForce(dir * dashForce, ForceMode2D.Impulse);
    }

    public override void Death()
    {
        SceneManager.LoadScene(0);
    }
    void BaseAttack()
    {
        if (wearedMask == null)
        {
            Entity entity = GetClosestEnemy();
            if (entity != null)
            {
                StartCoroutine(NoMaskAttackCoroutine( entity.transform.position));
                TekliHasar(entity);
            }
        }
        else
        {
            wearedMask.BaseAttack();
        }
    }
    IEnumerator NoMaskAttackCoroutine(Vector2 endpos)
    {
        float duration = 0.1f;
        float timer = 0;
        while (timer < duration)
        {
            noMaskAttackSprite.transform.position=Vector3.Lerp(transform.position,endpos,timer/duration);
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0;
        while (timer < duration)
        {
            noMaskAttackSprite.transform.position = Vector3.Lerp(endpos, transform.position, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
    }
    public Entity GetClosestEnemy(float overrideRange = -1)
    {
        float range = overrideRange > 0 ? overrideRange : attackRange;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            range,
            enemyLayer
        );

        Entity closest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (hit.transform == transform) continue;

            Entity ent = hit.GetComponent<Entity>();
            if (ent == null) continue;

            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = ent;
            }
        }
        return closest;
    }

    public List<Entity> GetClosestEnemies(float rangeAngle, float rangeBonus = 0)
    {
        List<Entity> result = new List<Entity>();
        float totalRange = attackRange + rangeBonus;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            totalRange,
            enemyLayer
        );

        if (hits.Length == 0) return result;

        Entity closest = GetClosestEnemy(totalRange);

        if (closest == null) return result;

        Vector2 baseDir = (closest.transform.position - transform.position).normalized;

        foreach (var hit in hits)
        {
            if (hit.transform == transform) continue;

            Entity ent = hit.GetComponent<Entity>();
            if (ent == null) continue;

            Vector2 dir = (hit.transform.position - transform.position).normalized;
            float angle = Vector2.Angle(baseDir, dir);

            if (angle <= rangeAngle * 0.5f)
            {
                result.Add(ent);
            }
        }

        return result;
    }
    public List<Entity> GetClosestEnemiesByCount(int count)
    {
        List<Entity> enemies = new List<Entity>();

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            attackRange,
            enemyLayer
        );

        foreach (var hit in hits)
        {
            Entity ent = hit.GetComponent<Entity>();
            if (ent == null) continue;

            enemies.Add(ent);
        }

        enemies.Sort((a, b) =>
        {
            float da = Vector2.Distance(transform.position, a.transform.position);
            float db = Vector2.Distance(transform.position, b.transform.position);
            return da.CompareTo(db);
        });

        if (enemies.Count > count)
            enemies.RemoveRange(count, enemies.Count - count);

        return enemies;
    }
    public List<Entity> GetClosestEnemiesArea(float range)
    {
        List<Entity> enemies = new List<Entity>();
        Entity closest = GetClosestEnemy();
        if (closest == null)
            return enemies;
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            closest.transform.position,
            range,
            enemyLayer
        );
        foreach (var hit in hits)
        {
            Entity ent = hit.GetComponent<Entity>();
            if (ent == null) continue;
            if (ent == this) continue;

            enemies.Add(ent);
        }
        return enemies;
    }

}
