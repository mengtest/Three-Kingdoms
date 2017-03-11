using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkillTest : MonoBehaviour
{
    public NavMeshAgent m_NavAgent;
    public Animator m_Animator;
    public float m_speed = 3.0f;
    public int SkillId = 0;

    protected SkillTest m_Target;
    private Skill m_Skill = null;

    void Awake()
    {
        m_NavAgent = gameObject.AddComponent<NavMeshAgent>();
        m_NavAgent.speed = 0;
        m_NavAgent.acceleration = 0;
        m_NavAgent.angularSpeed = 0;
        //m_NavAgent.avoidancePriority;
        m_NavAgent.height = 2.0f;
        m_NavAgent.radius = 1.5f;
        m_NavAgent.stoppingDistance = 2.0f;

        CapsuleCollider collider = gameObject.AddComponent<CapsuleCollider>();
        collider.height = m_NavAgent.height;
        collider.radius = m_NavAgent.radius;
        collider.center = Vector3.up*(Mathf.Max(collider.height/2.0f, collider.radius) + 0.03f);

        m_Animator = GetComponent<Animator>();
    }

	void Start () {
	    if (SkillId > 0)
	    {
	        m_Skill = new Skill(SkillId);
	    }
	}
	
	void Update () {
		FightStateUpdate();
	}

    void FightStateUpdate()
    {
        if (m_Target == null)
        {
            m_Target = FindTargetInRadius();
        }
        else
        {
            bool moveToTarget = false;
            TryAttack(out moveToTarget);
            if (moveToTarget)
            {
                MoveToTarget();
            }
        }
    }

    public SkillTest FindTargetInRadius()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 100, 1 << LayerMask.NameToLayer("Warrior"));

        foreach (Collider collider in colliders)
        {
            SkillTest unit = collider.gameObject.GetComponent<SkillTest>();
            if (unit == null)
            {
                continue;
            }
            if (unit == this)
            {
                continue;
            }
            return unit;
        }
        return null;
    }

    void MoveToTarget()
    {
        SetDesination(m_Target.transform.position);
    }


    public bool IsIdleToUseSkill
    {
        get
        {
            int curSkillId = m_Animator.GetInteger("SkillId");
            AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
            // 状态机父级处于idle状态并且没有在转换，同时当前skillid == 0
            if (curSkillId == 0 && stateInfo.IsName("Base Layer.idle") && !m_Animator.IsInTransition(0))
            {
                return true;
            }
            return false;
        }    
    }

    public void TryAttack(out bool moveToTarget)
    {
        moveToTarget = false;
        if (m_Skill != null)
        {
            if (!m_Skill.CoolDown)
            {
                if (Vector3.Distance(transform.position, m_Target.transform.position) >= m_Skill.AttackDist)
                {
                    moveToTarget = true;
                    return;
                }
                else
                {
                    ReadyToAttack();
                }
                if (IsIdleToUseSkill)
                {
                    m_Animator.SetInteger("SkillId", m_Skill.SkillId);
                }
                return;
            }
        }
    }

    public void ReadyToAttack()
    {
        if (m_Target != null)
        {
            RotateToTarget(m_Target.transform.position);
        }
        StopMove();
    }

    private void StopMove()
    {
        SetDesination(transform.position);
    }

    void SetDesination(Vector3 pos)
    {
        m_NavAgent.SetDestination(pos);
        m_NavAgent.Stop();
    }

    protected void RotateToTarget(Vector3 pos)
    {
        // 以自己为坐标原点的相对位置
        Vector3 relative = transform.InverseTransformPoint(pos);
        float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
        transform.Rotate(Vector3.up * angle);
    }
}
