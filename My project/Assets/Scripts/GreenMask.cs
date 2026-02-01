using UnityEngine;

public class GreenMask : Mask

{
    public override void BaseAttack()
    {
        Entity ent= player.GetClosestEnemy();
        player.TekliHasar(ent);
    }

    public override void SkillAttack1()
    {
        player.Dash();
        skill1Timer = 5f;
        Skill1.gameObject.SetActive(true);
    }

    public override void SkillAttack2()
    {
        if (player.health < 100)
        {
            player.health+=1;
        }
        skill2Timer = 1;
        Skill2.gameObject.SetActive(true);
    }
}
