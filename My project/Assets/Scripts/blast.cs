using UnityEngine;

public class blast : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private float maxRadius = 3f;
    [SerializeField] private float expandSpeed = 6f;
    [SerializeField] private float damage = 20f;

    private CircleCollider2D col;
    private bool exploding;

    void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.05f;
    }

    void OnEnable()
    {
        col.radius = 0.05f;
        exploding = true;
    }

    void FixedUpdate()
    {
        if (!exploding) return;

        col.radius += expandSpeed * Time.deltaTime;

        if (col.radius >= maxRadius)
        {
            exploding = false;
            gameObject.SetActive(false); // kendini kapatýr
            col.radius = 0.05f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Entity e = other.GetComponent<Entity>();
        if (e != null)
        {
            e.GetDamage(damage);
        }
    }

}
