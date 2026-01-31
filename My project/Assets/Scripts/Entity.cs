using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]private float _health;
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _speed;
    public bool isDead=false;
    public float health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            if (_health <= 0 && !isDead)
            {
                _health = 0;
                Death();
            }
        }
    }
    public float attackDamage {
        get
        {
            return _attackDamage;
        }
        set
        {
            _attackDamage = value;
        }
    }
    public float speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }

    public float attackRange {
        get
        {
            return _attackRange;
        }
        set
        {
            _attackRange = value;
        }
    }
    public float attackSpeed {
        get
        {
            return _attackSpeed;
        }
        set
        {
            _attackSpeed = value;
        }
    }
    [HideInInspector]public float attackTimer;
    public abstract void Attack(Entity entity, float bonusattack = 0);
    public abstract void Move();
    public void GetDamage(float damage)
    {
        if (damage < 0) return;
        health -=damage;
    }
    public abstract void Death();
}
