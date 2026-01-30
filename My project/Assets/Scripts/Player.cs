using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public List<Mask> masks;
    public Mask wearedMask;

    private void Start()
    {
        masks=new List<Mask>();
    }
    private void Update()
    {
        ChangeMaskKey();
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
                }
                break;
            case 2:
                if (masks[1] != null)
                {
                    wearedMask = masks[1];
                }
                break;
            case 3:
                if (masks[2] != null)
                {
                    wearedMask = masks[2];
                }
                break;
        }
    }

    public override void Attack(Entity ent)
    {
        ent.GetDamage(attackDamage);
    }

    public override void Move()
    {
        if (Input.GetKeyDown(KeyCode.W)) 
        {
            
        }
        if (Input.GetKeyDown(KeyCode.A))
        {

        }
        if (Input.GetKeyDown(KeyCode.S))
        {

        }
        if (Input.GetKeyDown(KeyCode.D))
        {

        }

    }

    public override void Death()
    {
        throw new System.NotImplementedException();
    }
}
