using UnityEngine;

public class AttackParticle : MonoBehaviour
{
    Enemy sender;
    public void Init(Enemy enemy)
    {
        sender = enemy;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            sender.Attack(collision.gameObject.GetComponent<Player>());
    }
}
