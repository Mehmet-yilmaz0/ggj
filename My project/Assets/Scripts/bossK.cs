using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bossK : Boss
{
    public List<GameObject> waepons = new List<GameObject>();
    public GameObject exit;
    [Header("Auto Attack Settings")]
    [SerializeField] private float autoAttackRange = 2.5f;
    [SerializeField, Range(0f, 1f)] private float fxBiasToTarget = 0.8f;
    public GameObject Dead;
    [Header("Auto Attack FX (Assign in Inspector)")]
    [SerializeField] private GameObject autoAttackAnimation;

    [Header("Big Attack Settings")]
    [SerializeField] private float bigAttackStepDelay = 0.5f;

    private Coroutine _bigAttackRoutine;
    private void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(BigAttackTimeline());
    }

    IEnumerator BigAttackTimeline()
    {
        yield return new WaitForSeconds(3);
        yield return BigAttackSequence();

        yield return new WaitForSeconds(5);
        yield return BigAttackSequence();

        yield return new WaitForSeconds(9);
        yield return BigAttackSequence();

        yield return new WaitForSeconds(13);
        yield return BigAttackSequence();
    }

    void Update()
    {
        if(isDead) return;
        AutoAttack();
        base.Update();
    }

    public void AutoAttack()
    {
        if (Target == null) return;
        if (autoAttackAnimation == null) return;

        Entity targetEntity = Target.GetComponent<Entity>();
        if (targetEntity == null) return;

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
            return;
        }

        float dist = Vector3.Distance(transform.position, Target.transform.position);
        if (dist > autoAttackRange) return;

        Attack(targetEntity);

        Vector3 fxPos = Vector3.Lerp(transform.position, Target.transform.position, fxBiasToTarget);
        autoAttackAnimation.transform.position = fxPos;

        autoAttackAnimation.SetActive(false);
        autoAttackAnimation.SetActive(true);
    }

    public void BigAttack()
    {
        MoveIndex = false;

        // üst üste çaðrýlýrsa önceki diziyi durdur (çifte tetiklemeyi engeller)
        if (_bigAttackRoutine != null)
            StopCoroutine(_bigAttackRoutine);

        _bigAttackRoutine = StartCoroutine(BigAttackSequence());
    }

    private IEnumerator BigAttackSequence()
    {
        if (waepons == null || waepons.Count == 0)
        {
            _bigAttackRoutine = null;
            yield break;
        }

        int pairEndExclusive = Mathf.Min(22, waepons.Count);
        for (int i = 0; i + 1 < pairEndExclusive; i += 2)
        {
            if (waepons[i] != null) waepons[i].SetActive(true);
            if (waepons[i + 1] != null) waepons[i + 1].SetActive(true);

            yield return new WaitForSeconds(bigAttackStepDelay);
        }

        for (int i = 22; i < waepons.Count; i++)
        {
            if (waepons[i] != null)
                waepons[i].SetActive(true);

            yield return new WaitForSeconds(bigAttackStepDelay);
        }

        MoveIndex = true;

        _bigAttackRoutine = null;
    }


    public override void Attack(Entity entity, float bonusattack = 0)
    {
        attackTimer = attackSpeed;
        entity.GetDamage(attackDamage + bonusattack);
    }

    public override void Death()
    {
        exit.SetActive(true);
        canMove = false;
        isDead = true;
        Dead.SetActive(true);
        this.gameObject.GetComponent<SpriteRenderer>().color.WithAlpha(0);
    }
}
