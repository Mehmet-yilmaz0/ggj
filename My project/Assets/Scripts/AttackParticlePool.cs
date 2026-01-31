using System.Collections.Generic;
using UnityEngine;

public class AttackParticlePool : MonoBehaviour
{
    public static AttackParticlePool instance;
    [SerializeField]GameObject attackParticle;

    Queue<GameObject> pool = new Queue<GameObject>();
    private void Awake()
    {
        if(instance == null && instance != this)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

    }
    public GameObject Spawn(Vector2 position, Quaternion rotation)
    {
        GameObject obj;

        if (pool.Count > 0)
            obj = pool.Dequeue();
        else
            obj = Instantiate(attackParticle, transform);

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.gameObject.SetActive(true);

        return obj;
    }

    public void Despawn(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
