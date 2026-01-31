using System.Collections.Generic;
using UnityEngine;

public class bossK : Boss
{
    public bool MoveIndex=false;
    Transform target;
    public List<GameObject> waepons = new List<GameObject>();
    //33 tane bomba var. ilk 22si 1 2 (yan yana) baþlayacak ve ardýndan ise 3 4 aktif olacak ve bu þekilde sýrayla her patlama belirli aralýklarla aktif olacak.

    // Update is called once per frame
    void Update()
    {
        
        moveFunc(MoveIndex);
        AutoAttack();
    }


    public void moveFunc(bool Index)
    {
        if(MoveIndex)
        {
            
        }
        else
        {
            
        }
    }
    public void AutoAttack()
    {

    }
    
}
