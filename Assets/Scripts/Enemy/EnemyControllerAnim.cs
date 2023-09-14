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
            // 待機モーション
            animator.SetTrigger("Idle");
        }

        if (enemy.state == EnemyController.State.Run)
        {
            // 待機モーション
            animator.SetTrigger("Run");
        }

        if (enemy.state == EnemyController.State.Attack)
        {
            // 待機モーション
            animator.SetTrigger("Attack");
            // 攻撃モーション
        }
        if(enemy.state==EnemyController.State.Die)
        {
            animator.SetTrigger("Die");
        }
    }
}
