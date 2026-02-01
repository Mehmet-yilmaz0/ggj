using UnityEngine;

public class AutoAttackAnimationController : MonoBehaviour
{
    public void OnAnimationFinished()
    {
        gameObject.SetActive(false);
    }
}
