using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public List<Mask> masks;
    public Mask wearedMask;
    LayerMask enemyLayer;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        masks=new List<Mask>();
        masks.Add(null);
        masks.Add(null);
        masks.Add(null);
    }
    private void Update()
    {
        ChangeMaskKey();
        if (attackTimer <= 0f)DetectEnemy();
        Move();
        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
        else attackTimer = 0;
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
                    wearedMask = masks[0];
                    wearedMask.WearMask();
                }
                break;
            case 2:
                if (masks[1] != null)
                {
                    wearedMask = masks[1];
                    wearedMask.WearMask();
                }
                break;
            case 3:
                if (masks[2] != null)
                {
                    wearedMask = masks[2];
                    wearedMask.WearMask();
                }
                break;
        }
    }

    public override void Attack(Entity ent)
    {
        if (attackTimer == 0)
        {
            attackTimer = attackSpeed;
            ent.GetDamage(attackDamage);
        }  
    }

    public override void Move()
    {
        float x = 0f;
        float y = 0f;

        if (Input.GetKey(KeyCode.A)) x = -1f;
        if (Input.GetKey(KeyCode.D)) x = 1f;
        if (Input.GetKey(KeyCode.S)) y = -1f;
        if (Input.GetKey(KeyCode.W)) y = 1f;

        Vector2 dir = new Vector2(x, y).normalized;
        rb.linearVelocity = dir * speed;
    }

    public override void Death()
    {
        throw new System.NotImplementedException();
    }
    void DetectEnemy()
    {
        Entity entity = GetClosestEnemy();
        if (entity != null)
        {
            Attack(entity);
        }
    }
    Entity GetClosestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            attackRange,
            enemyLayer
        );

        Entity closest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
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

}
