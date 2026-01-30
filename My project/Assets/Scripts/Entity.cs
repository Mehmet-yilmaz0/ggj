using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    private float _health;
    public float health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            if (_health <= 0)
            {
                _health = 0;
                Death();
            }
        }
    }
    public float attackDamage { get; set; }
    public float speed { get; set; }

    public float attackRange;
    public abstract void Attack(Entity entity);
    public abstract void Move();
    public void GetDamage(float damage)
    {
        if (damage < 0) return;
        health -=damage;
    }
    public abstract void Death();
}
