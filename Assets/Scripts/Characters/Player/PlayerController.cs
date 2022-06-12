using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : Singleton<PlayerController>
{
    private const float StoppingDistance = 0.5f;

    private const float ThrowForce = 20.0f;

    public float RollCoolDown = 0.6f;

    public GameObject bloodPS;

    private float rollCoolDown = 0.0f;
    
    private NavMeshAgent agent;

    [HideInInspector]
    public Animator animator;

    private GameObject attackTarget;

    [HideInInspector]
    public CharacterStats characterStats;

    private float attackTime;

    public bool isKnockDown = false;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.RigisterPlayer(gameObject);  
        TranstionManager.Instance.RigisterPlayer(gameObject);
        MouseManager.Instance.OnMouseClick += MoveToTarget;
        MouseManager.Instance.OnEnemyClick += EventAttack;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        if(MouseManager.Instance != null)
        {
            MouseManager.Instance.OnMouseClick -= MoveToTarget;
            MouseManager.Instance.OnEnemyClick -= EventAttack;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAnimation();

        attackTime -= Time.deltaTime;
    }

    bool FoundEnemyInAttackRange()
    {
        var colliders = Physics.OverlapSphere(transform.position, agent.stoppingDistance);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                return true;
            }
        }
        return false;
    }

    void SwitchAnimation()
    {
        SwitchMoveAnimation();

        SwitchRollAnimation();

        SwitchDeathAnimation();
    }

    void SwitchMoveAnimation()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    void SwitchRollAnimation()
    {
        if(rollCoolDown < 0 && Input.GetKeyDown(KeyCode.LeftShift))
        {
            rollCoolDown = RollCoolDown;
            animator.SetTrigger("Roll");
        }
        else
        {
            rollCoolDown -= Time.deltaTime;
        }
    }

    void SwitchDeathAnimation()
    {
        if (characterStats.CurrentHealth == 0)
        {
            animator.SetBool("Death", true);
            GameManager.Instance.NotifyObserver();
            attackTarget = null;
            agent.enabled = false;
            MouseManager.Instance.OnMouseClick -= MoveToTarget;
            MouseManager.Instance.OnEnemyClick -= EventAttack;
        }
    }

    void MoveToTarget(Vector3 pos)
    {
        StopAllCoroutines();
        agent.stoppingDistance = StoppingDistance;
        agent.isStopped = false;
        agent.destination = pos;
    }

    void EventAttack(GameObject enemy) 
    {
        if(enemy != null)
        {
            attackTarget = enemy;
            agent.stoppingDistance = characterStats.AttackRange + StoppingDistance + Mathf.Max(enemy.transform.localScale.x - 2, 0);
            characterStats.isCritical = (UnityEngine.Random.value < characterStats.CriticalChance);
            StartCoroutine(MoveToAttackEnemy());
        }
    }

    IEnumerator MoveToAttackEnemy()
    {
        agent.isStopped = false;

        while (Vector3.Distance(transform.position,attackTarget.transform.position) > agent.stoppingDistance + 0.1f)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;

        if(attackTime < 0.0f) // 攻击
        {
            transform.LookAt(attackTarget.transform.position);
            characterStats.isCritical = (Random.value < characterStats.CriticalChance);
            if (characterStats.isCritical)
            {
                animator.SetTrigger("Critical");
            }
            else
            {
                animator.SetTrigger("Attack");
            }
            attackTime = characterStats.AttackCoolDown;
        }

        yield return null; 
    }

    void Hit() // 关键帧事件，造成伤害
    {
        if (attackTarget == null) return;// 如果此时死亡，攻击无效

        if (attackTarget.CompareTag("Attackable")) // 如果为可破坏物体
        {
            
        } // TODO: 可以删掉

        if (!transform.IsFacingTarget(attackTarget.transform)) return; // 如果此时敌人未处于前方，攻击无效
        if (!FoundEnemyInAttackRange()) return; // 如果此时敌人离开，攻击无效    

        if(attackTarget.CompareTag("Enemy")) // 如果为敌人
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats, targetStats);
        }
        
    }

    public void StartBlooding()
    {
        bloodPS.SetActive(true);
    }

    public void EndBlooding()
    {
        bloodPS.SetActive(false);
    }

}
