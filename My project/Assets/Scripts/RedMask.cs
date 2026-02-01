using System.Collections.Generic;
using UnityEngine;

public class RedMask : Mask
{
    public override void BaseAttack()
    {
        List<Entity> list = player.GetClosestEnemies(90);
        if (list != null && list.Count > 0)
        {
            player.AlanHasar(list);
        }
    }

    public override void SkillAttack1()
    {
        List<Entity> list = player.GetClosestEnemies(360);
        if (list != null && list.Count > 0)
        {
            player.AlanHasar(list);
            if (player.health + list.Count*5 >= 100)
            {
                player.health = 100;
            }
            else
            {
                player.health += list.Count * 5;
            }
            skill2Timer = 5f;
        }
    }

    public override void SkillAttack2()
    {
        List<Entity> list = player.GetClosestEnemies(360, 2);
        if (list != null && list.Count > 0)
        {
            player.AlanHasar(list);
            skill2Timer = 10f;
        }
    }
   
}
