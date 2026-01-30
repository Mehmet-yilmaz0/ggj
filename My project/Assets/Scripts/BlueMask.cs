using System.Collections.Generic;


public class BlueMask : Mask
{
    public override void BaseAttack()
    {
        List<Entity> list=player.GetClosestEnemiesByCount(2);
        if (list != null && list.Count > 0)
        {
            player.AlanHasar(list);
        }
    }

    public override void SkillAttack1()
    {
        List<Entity>list=player.GetClosestEnemiesArea(2);
        if (list != null && list.Count > 0)
        {
            player.AlanHasar(list);
            skill1Timer = 5f;
        }
    }

    public override void SkillAttack2()
    {
        List<Entity> list = player.GetClosestEnemies(360);
        if (list != null && list.Count > 0)
        {
            player.AlanHasar(list,-3);
            skill2Timer = 0.1f;
        }
    }
}
