using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Entity
{
    public bool MoveIndex = false;
    [SerializeField] private bool canMove = false;
    public GameObject Target;
    public List<GameObject> states = new List<GameObject>();

    [SerializeField] private float waypointReachedDistance = 0.1f;

    private int _currentStateIndex = 0;

    private void OnEnable()
    {
        canMove = false;
        StartCoroutine(EnableDelay());
    }

    private IEnumerator EnableDelay()
    {
        yield return new WaitForSeconds(2f);
        canMove = true;
    }

    public override void Move()
    {
        if (!canMove) return;

        if (MoveIndex)
        {
            if (Target == null) return;

            transform.position = Vector3.MoveTowards(
                transform.position,
                Target.transform.position,
                speed * Time.deltaTime
            );
            return;
        }

        if (states == null || states.Count == 0) return;

        Vector3 waypointPos = states[_currentStateIndex].transform.position;

        transform.position = Vector3.MoveTowards(
            transform.position,
            waypointPos,
            speed * 1.3f * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, waypointPos) <= 0.1f)
        {
            _currentStateIndex = (_currentStateIndex + 1) % states.Count;
        }
    }


    protected void Update()
    {
        Move();
    }

    public override void Attack(Entity entity, float bonusattack = 0)
    {
        throw new System.NotImplementedException();
    }

    public override void Death()
    {
        throw new System.NotImplementedException();
    }
}
