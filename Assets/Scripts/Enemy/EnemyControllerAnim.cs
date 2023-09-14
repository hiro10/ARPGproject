using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerAnim : MonoBehaviour
{
    Animator animator;

    EnemyController enemy;

    private void Start()
    {
        enemy = GetComponent<EnemyController>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if(enemy.state==EnemyController.State.Idle)
        {
            // �ҋ@���[�V����
            animator.SetTrigger("Idle");
        }

        if (enemy.state == EnemyController.State.Run)
        {
            // �ҋ@���[�V����
            animator.SetTrigger("Run");
        }

        if (enemy.state == EnemyController.State.Attack)
        {
            // �ҋ@���[�V����
            animator.SetTrigger("Attack");
            // �U�����[�V����
        }
        if(enemy.state==EnemyController.State.Die)
        {
            animator.SetTrigger("Die");
        }
    }
}
