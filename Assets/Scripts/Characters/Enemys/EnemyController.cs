using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SerializeField]
public enum EnemyState
{
    GUARD,
    PATROL,
    CHASE,
    ATTCK,
    DEAD,
    WIN
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(HealthBarUI))]
public class EnemyController : MonoBehaviour, IEndGameObserver
{
    private const float stoppingDistance = 1.0f;

    public EnemyState enemyState;

    private float patrolCoolDown, attackCoolDown, skillCoolDown;

    private NavMeshAgent agent;

    protected Animator animator;

    protected GameObject attackTarget; // ����Ŀ��

    protected CharacterStats characterStats; // �������ݴ���

    private new Collider collider; //  �ƺ��汾���º���Щ���⣿

    private Vector3 originalPos; // ��ʼ�ص㣬����Ѳ���ж�

    private Quaternion originalRot; // ��ʼ��ת

    private Vector3 targetPos; // Ѳ��ʱ�ĵ�Ŀ��ص�

    private EnemyState originalState;

    private bool isIdleBattle, isWalk, isChase, isFollow, isDie = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        collider = GetComponent<Collider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        float tmp = characterStats.PatrolSpeed;
        agent.speed = tmp;
        agent.stoppingDistance = stoppingDistance;
        originalPos = transform.position;
        originalState = enemyState;
        originalRot = transform.rotation;
        targetPos = transform.position;
        attackCoolDown = skillCoolDown = 0.0f;
        GameManager.Instance.RigisterObserver(this);
    }

    //void OnEnable()
    //{

    //}

    void OnDisable()
    {
        if (GameManager.Instance == null)
        {
            return; // ��Ϸ�ر�ʱ���ظ�ɾ�����˴��жϿɱ��ⱨ��
        }
        GameManager.Instance.RemoveObserver(this);

        if(GetComponent<LootSpawner>() && isDie)
        {
            Debug.Log("Spawn Loop");
            GetComponent<LootSpawner>().SpawnLoop();
        }

    }

    // Update is called once per frame
    void Update()
    {   
        SwitchState();
        ChangeAnimator();
    }

    protected bool FoundPlayerInSightRange()
    {
        var colliders = Physics.OverlapSphere(transform.position, characterStats.SightRange);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                attackTarget = collider.gameObject;
                return true;
            }
        }
        return false;
    }

    protected bool FoundPlayerInAttackRange()
    {
        var colliders = Physics.OverlapSphere(transform.position, characterStats.AttackRange);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                attackTarget = collider.gameObject;
                return true;
            }
        }
        
        return false;
    }

    protected bool FoundPlayerInSkillRange()
    {
        var colliders = Physics.OverlapSphere(transform.position, characterStats.SkillRange);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                attackTarget = collider.gameObject;
                return true;
            }
        }
        
        return false;
    }

    void SwitchState()
    {
        if (characterStats.CurrentHealth == 0)
        {
            enemyState = EnemyState.DEAD;
        }

        switch (enemyState)
        {
            case EnemyState.GUARD:
                EnterGuardState();
                ExitGuardState();
                break;

            case EnemyState.PATROL:
                EnterPatrolState();
                ExitPatrolState();
                break;

            case EnemyState.CHASE:
                EnterChaseState();
                ExitChaseState();
                break;

            case EnemyState.ATTCK:
                EnterAttackState();
                ExitAttackState();
                break;

            case EnemyState.DEAD:
                EnterDeadState();
                ExitDeadState();
                break;
            case EnemyState.WIN:
                EnterWinState();
                ExitWinState();
                break;
        }
    }

    void EnterGuardState()
    {
        isIdleBattle = false;
        isWalk = true;
        isChase = false;
        isFollow = false;
        characterStats.isCritical = false;
        agent.destination = originalPos;
        agent.speed = characterStats.PatrolSpeed;
        agent.stoppingDistance = stoppingDistance;

        if (Vector3.Distance(transform.position, originalPos) < agent.stoppingDistance)
        {
            isWalk = false;
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRot, 0.01f);
        }
    }

    void ExitGuardState()
    {
        if (FoundPlayerInSightRange())
        {
            enemyState = EnemyState.CHASE;
        }
    }

    void EnterPatrolState()
    {
        isIdleBattle = false;
        isWalk = true;
        isChase = false;
        isFollow = false;
        characterStats.isCritical = false;
        agent.destination = originalPos;
        agent.speed = characterStats.PatrolSpeed;
        
        agent.stoppingDistance = stoppingDistance;

        if (Vector3.Distance(targetPos, transform.position) > agent.stoppingDistance)
        {
            agent.destination = targetPos;
        }
        else
        {
            if (patrolCoolDown > 0)
            {
                isWalk = false;
                agent.destination = transform.position;
                patrolCoolDown -= Time.deltaTime;
            }
            else
            {
                isWalk = true;
                patrolCoolDown = characterStats.PatrolCoolDown;

                float randomX = Random.Range(-characterStats.PatrolRange, characterStats.PatrolRange);
                float randomZ = Random.Range(-characterStats.PatrolRange, characterStats.PatrolRange);
                Vector3 randomPoint = new Vector3(originalPos.x + randomX, transform.position.y, originalPos.z + randomZ);

                NavMeshHit hit;
                targetPos = NavMesh.SamplePosition(randomPoint, out hit, characterStats.PatrolRange, 1) ? hit.position : transform.position;
            }
            
        }
    }

    void ExitPatrolState()
    {
        if (FoundPlayerInSightRange())
        {
            enemyState = EnemyState.CHASE;
        }
    }

    void EnterChaseState()
    {
        isIdleBattle = true;
        isWalk = false;
        isChase = true;
        isFollow = true;
        characterStats.isCritical = false;
        agent.speed = characterStats.ChaseSpeed;
        agent.stoppingDistance = characterStats.AttackRange;

        if (Vector3.Distance(transform.position, attackTarget.transform.position) > agent.stoppingDistance)
        {
            agent.destination = attackTarget.transform.position;
        }
    }

    void ExitChaseState()
    {
        if (FoundPlayerInSkillRange() || FoundPlayerInAttackRange())
        {
            agent.destination = transform.position;
            enemyState = EnemyState.ATTCK;
        }

        if (!FoundPlayerInSightRange())
        {
            agent.destination = transform.position;
            enemyState = originalState;
        }
    }

    public virtual void EnterAttackState()
    {
        isIdleBattle = true;
        isWalk = false;
        isChase = false;
        isFollow = false;
        agent.stoppingDistance = characterStats.AttackRange + attackTarget.GetComponent<NavMeshAgent>().radius;

        if(skillCoolDown > 0)
        {
            skillCoolDown -= Time.deltaTime;
        }
        else if(FoundPlayerInSkillRange())// ���ܹ���
        {
            transform.LookAt(attackTarget.transform);
            characterStats.isCritical = (Random.value < characterStats.CriticalChance);
            animator.SetBool("Critical", characterStats.isCritical);
            animator.SetTrigger("SkillAttack");
            skillCoolDown = characterStats.SkillCoolDown;
        }

        if (attackCoolDown > 0)
        {
            attackCoolDown -= Time.deltaTime;
        }
        else if(FoundPlayerInAttackRange()) // ����
        {
            transform.LookAt(attackTarget.transform);
            characterStats.isCritical = (Random.value < characterStats.CriticalChance);
            animator.SetBool("Critical", characterStats.isCritical);
            animator.SetTrigger("Attack");
            attackCoolDown = characterStats.AttackCoolDown;
        }
    }

    public virtual void ExitAttackState()
    {
        if (!FoundPlayerInAttackRange() && !FoundPlayerInSkillRange())
        {
            if (FoundPlayerInSightRange())
            {
                enemyState = EnemyState.CHASE;
            }
            else
            {
                enemyState = originalState;
            }
        }
    }

    void EnterDeadState()
    {
        isDie = true;
        attackTarget = null; // ע�⣬��ɫ�������粻��Ŀ����գ�����ڽ�ɫ�ݻٺ���� Hit() ��� bug
        agent.radius = 0;
        agent.isStopped = true;
        collider.enabled = false;
        Destroy(gameObject, 2.0f);
    }

    void ExitDeadState()
    {
        return ;
    }

    void EnterWinState()
    {
        animator.SetBool("Win", true);
        attackTarget = null;
    }

    void ExitWinState()
    {
        return;
    }

    void ChangeAnimator()
    {
        animator.SetBool("IdleBattle", isIdleBattle);
        animator.SetBool("Walk",isWalk);
        animator.SetBool("Chase", isChase);
        animator.SetBool("Follow", isFollow);
        animator.SetBool("Death", isDie);
    }

    public virtual void Hit() // �ؼ�֡�¼�������˺�
    {
        if (attackTarget == null) return;// �����ʱ������������Ч
        if (!transform.IsFacingTarget(attackTarget.transform)) return; // �����ʱ���δ����ǰ����������Ч
        if (!FoundPlayerInAttackRange()) return; // �����ʱ����뿪��������Ч

        var targetStats = attackTarget.GetComponent<CharacterStats>();
        characterStats.TakeDamage(characterStats, targetStats);
    }

    public void EndNotify() // �������ʱ������Ϊ
    {
        enemyState = EnemyState.WIN;
    }
}
