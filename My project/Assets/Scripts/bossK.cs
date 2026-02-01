using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossK : Boss
{
    public List<GameObject> waepons = new List<GameObject>();

    [Header("Auto Attack Settings")]
    [SerializeField] private float autoAttackRange = 2.5f;     // dinamik yakýnlýk
    [SerializeField, Range(0f, 1f)] private float fxBiasToTarget = 0.8f; // 0.8 => target'a yakýn
    [SerializeField] private float fxActiveTime = 0.25f;       // anim objesini ne kadar açýk tutsun

    [SerializeField]private GameObject autoAttackAnimation;
    private Coroutine _fxRoutine;

    void Awake()
    {
        // "auto attack animation" isimli objeyi oluþtur
        autoAttackAnimation = new GameObject("auto attack animation");
        autoAttackAnimation.SetActive(false);
    }

    void Update()
    {
        AutoAttack();
        base.Update();
    }

    public void AutoAttack()
    {
        if (!MoveIndex) return;            // takip modunda deðilse auto attack yapma (istersen kaldýr)
        if (Target == null) return;

        // target'ýn Entity componentini bul
        Entity targetEntity = Target.GetComponent<Entity>();
        if (targetEntity == null) return;

        // cooldown sayacý
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
            return;
        }

        // mesafe kontrolü (world)
        float dist = Vector3.Distance(transform.position, Target.transform.position);
        if (dist > autoAttackRange) return;

        // Attack çaðýr (bossK içindeki Attack)
        Attack(targetEntity);

        // FX objesini target'a yakýn olacak þekilde iki obje arasýna koy
        Vector3 bossPos = transform.position;
        Vector3 targetPos = Target.transform.position;

        Vector3 fxPos = Vector3.Lerp(bossPos, targetPos, fxBiasToTarget);
        autoAttackAnimation.transform.position = fxPos;

        // FX'i aç-kapat
        if (_fxRoutine != null) StopCoroutine(_fxRoutine);
        _fxRoutine = StartCoroutine(FxPulse());
    }

    private IEnumerator FxPulse()
    {
        autoAttackAnimation.SetActive(true);
        yield return new WaitForSeconds(fxActiveTime);
        autoAttackAnimation.SetActive(false);
        _fxRoutine = null;
    }

    public void BigAttack()
    {
        MoveIndex = false;
    }

    public override void Attack(Entity entity, float bonusattack = 0)
    {
        attackTimer = attackSpeed;
        entity.GetDamage(attackDamage + bonusattack);
    }
}
