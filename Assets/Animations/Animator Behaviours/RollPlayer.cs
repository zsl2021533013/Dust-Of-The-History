using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RollPlayer : StateMachineBehaviour
{
    const float RollSpeed = 12.0f;

    const float RollAcceleration = 8000.0f;

    const float OriginalAccelerationd = 80.0f;

    Vector3 dir;
    Vector3 targetPos;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GameManager.Instance.player) // 如果 player 注册了才执行
        {
            GameManager.Instance.player.GetComponent<NavMeshAgent>().acceleration = RollAcceleration;
            GameManager.Instance.player.GetComponent<NavMeshAgent>().speed = RollSpeed;
            GameManager.Instance.player.transform.LookAt(MouseManager.Instance.hitInfo.point);

            dir = (MouseManager.Instance.hitInfo.point - GameManager.Instance.player.transform.position).normalized;

            targetPos = GameManager.Instance.player.transform.position + 4.0f * dir;

            GameManager.Instance.player.GetComponent<NavMeshAgent>().destination = targetPos;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GameManager.Instance.player) // 如果 player 注册了才执行
        {
            GameManager.Instance.player.GetComponent<NavMeshAgent>().speed = GameManager.Instance.characterStats.ChaseSpeed;
            // 速度回归正常
            GameManager.Instance.player.GetComponent<NavMeshAgent>().acceleration = OriginalAccelerationd;
            GameManager.Instance.player.GetComponent<NavMeshAgent>().isStopped = true;
            GameManager.Instance.player.GetComponent<NavMeshAgent>().isStopped = false;
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
