using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class SoldierAttackHandler : MonoBehaviour
{
    private Transform enemyBase;
    private NavMeshAgent agent;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float targetRange = 10f;
    [SerializeField] private float attackPower = 1f;
    [SerializeField] private float attackDelay = 1f;
    private Transform currentTarget;
    [SerializeField] private LayerMask enemyLayer;
    public UnityEvent attackEvent;
    private SoldierHealthSystem health;
    public AudioClip AttackSFX;

    private bool canAttack = true;
    void OnEnable()
    {
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.3f);
    }

    void OnDisable()
    {
        CancelInvoke(nameof(UpdateTarget));
    }
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        if(gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            enemyBase = GameObject.FindGameObjectWithTag("EnemyBase").transform;
        }
        else
        {
            enemyBase = GameObject.FindGameObjectWithTag("OurBase").transform;
        }
        agent.SetDestination(enemyBase.position);
        health = GetComponent<SoldierHealthSystem>();
    }
    void Update()
    {
        if(health.getCurrentHealth()<=0) return;
        if (currentTarget != null)
        {
            agent.SetDestination(currentTarget.position);
        }
        HandleAttack();
    }
    void UpdateTarget()
    {
        Transform nearestEnemy = FindNearestEnemy();

        if (nearestEnemy != null)
        {
            currentTarget = nearestEnemy;
        }
        else
        {
            currentTarget = enemyBase;
        }
    }
    Transform FindNearestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, targetRange, enemyLayer);

        float minDist = Mathf.Infinity;
        Transform nearest = null;

        foreach (Collider hit in hits)
        {
            float dist = Vector3.Distance(transform.position, hit.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                nearest = hit.transform;
            }
        }
        Debug.Log("Nearest Enemy: " + (nearest != null ? nearest.name : "None"));
        return nearest;
    }
    void HandleAttack()
    {
        if (currentTarget == null) return;
        float dist = Vector3.Distance(transform.position, currentTarget.position);

        if (dist <= attackRange)
        {
            agent.isStopped = true;

            if (agent.velocity.magnitude < 0.1f && canAttack)
            {
                canAttack = false;
                GameObject targetObj = currentTarget.gameObject;
                IHealth health = targetObj.GetComponent<IHealth>();
                health.TakeDamage(attackPower);
                SoundManager.Instance.PlaySFX(AttackSFX);
                StartCoroutine(ResetAttack());
                attackEvent.Invoke();
            }
        }
        else
        {
            agent.isStopped = false;
        }
    }
    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }
}
