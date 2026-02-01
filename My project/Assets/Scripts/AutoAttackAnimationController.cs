using UnityEngine;

public class AutoAttackAnimationController : MonoBehaviour
{
    public void OnAnimationFinished()
    {
        transform.localPosition=Vector2.zero;  
        gameObject.SetActive(false);
    }
}
