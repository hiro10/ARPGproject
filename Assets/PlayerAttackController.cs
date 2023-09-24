using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAttackController : MonoBehaviour
{
    public Transform enemy; // �G�l�~�[��Transform�R���|�[�l���g
    public float attackDistance = 5f; // �U���͈�
    public float moveDistance = 1f; // �ړ�����
    public float attackDuration = 1f; // �U���̈ړ��ɂ����鎞��

    public PlayerLockOn playerLock;
    public PlayerController playerController;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void StartAttack()
    {
        if (playerLock.target)
        {
            enemy = playerLock.target.transform;
            // �G�l�~�[�Ƃ̋������v�Z
            float distance = Vector3.Distance(transform.position, enemy.position);

            // �U���{�^����������A�G�l�~�[���U���͈͓��ɂ���ꍇ
            if ( distance <= attackDistance)
            {
                playerLock.target.GetComponent<EnemyController>().lockOn=true;
                // �v���C���[���G�l�~�[�̕����Ɍ�����
                Vector3 targetLookDirection = enemy.position - transform.position;
                targetLookDirection.y = 0f; // y�������̉�]�𖳌���
                transform.rotation = Quaternion.LookRotation(targetLookDirection);
                if (distance >= moveDistance)
                {

                    animator.SetFloat("Speed", -1f);

                    // �v���C���[���猩���G�l�~�[�̈ʒu�̏�����O�܂ňړ�����
                    Vector3 targetPosition = enemy.position - transform.forward*1.5f;
                    targetPosition.y = transform.position.y; // y���ʒu���ێ�����
                    transform.DOMove(targetPosition, 0.5f);
                }
                
            }
        }
    }
}
