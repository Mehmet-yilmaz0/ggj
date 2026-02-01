using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Mask : MonoBehaviour
{
    public List<Sprite> forms;
    public Player player;
    public float timer = 0;
    public bool isOn;
    public int maskIndex;
    public float rangeBonus;
    public float attackSpeedBonus;
    public float attackBonus;
    public float skill1Timer;
    public float skill2Timer;
    public bool isGot = false;
    public GameObject Skill1;
    public GameObject Skill2;

    private void Start()
    {
        maskController();
    }
    private void Update()
    {
        if (!isGot) return;
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
        maskController();
        transform.position=player.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<Player>();
            isGot= true;
            player.masks[maskIndex - 1] = this;
            WearMask();

        }
    }
    public void WearMask()
    {
        if(player.wearedMask!=null) player.wearedMask.UnWearMask();
        Color open = gameObject.GetComponent<SpriteRenderer>().color;
        open.a = 1f;
        gameObject.GetComponent<SpriteRenderer>().color = open;
        player.wearedMask = this;
        player.attackDamage += attackBonus;
        player.attackRange += rangeBonus;
        player.attackSpeed += attackSpeedBonus;
        isOn = true;
    }
    public void UnWearMask()
    {
        Color close= gameObject.GetComponent<SpriteRenderer>().color;
        close.a = 0f;
        gameObject.GetComponent<SpriteRenderer>().color=close;
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
    public void maskController()
    {
        if (isGot)
        {
            if (timer < 15f)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = forms[1];
            }
            else if (timer > 15f && timer < 30f)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = forms[2];
            }
            else
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = forms[3];
            }
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = forms[0];
        }
    }
    public abstract void BaseAttack();
    public abstract void SkillAttack1();
    public abstract void SkillAttack2();
}
