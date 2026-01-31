using UnityEngine;

public abstract class Mask : MonoBehaviour
{
    public Player player;
    public float timer = 0;
    public bool isOn;
    public int maskIndex;
    public float rangeBonus;
    public float attackSpeedBonus;
    public float attackBonus;
    public float skill1Timer;
    public float skill2Timer;

    private void Update()
    {
        if (!isOn) return;
        if (isOn)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if(timer!=0)timer = 0;
        }
        if (skill1Timer > 0)
        {
            skill1Timer-= Time.deltaTime;
            if (skill1Timer <= 0)
            {
                {
                    skill1Timer = 0f;
                }
            }
        }
        if (skill2Timer > 0)
        {
            skill2Timer -= Time.deltaTime;
            if (skill2Timer <= 0)
            {
                {
                    skill2Timer = 0f;
                }
            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            player = collision.gameObject.GetComponent<Player>();
            if (!player.masks.Contains(this)) player.masks[maskIndex] = this;
            WearMask();
        }
    }
    public void WearMask()
    {
        player.wearedMask.UnWearMask();
        player.wearedMask = this;
        player.attackDamage += attackBonus;
        player.attackRange += rangeBonus;
        player.attackSpeed += attackSpeedBonus;
        isOn = true;
    }
    public void UnWearMask()
    {
        player.attackDamage -= attackBonus;
        player.attackRange -= rangeBonus;
        player.attackSpeed -= attackSpeedBonus;
        isOn = false;
    }
    public void UseSkill(char c)
    {
        switch (c)
        {
            case 'e':
                if(timer > 15f)
                    SkillAttack1();
                break;
            case 'q':
                if (timer > 30f && maskIndex==1)
                    SkillAttack2();
                break;
        }
        if (timer > 30f && maskIndex != 1)
            SkillAttack2();
    }
    public abstract void BaseAttack();
    public abstract void SkillAttack1();
    public abstract void SkillAttack2();
}
