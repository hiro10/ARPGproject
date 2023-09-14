using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // �v���C���[�̈ʒu
    private Transform player;
    private NavMeshAgent navMeshAgent;

    // �v���C���[��ǐՂ���p�x
    private float trackingAngle = 180f; 

    // �v���C���[�Ƃ̍ő勗��
    private float maxDistance = 10f;
 

    // ���݂̃G�l�~�[�̏��
    public State state;

    public int Hp = 1;
�@ public enum State
    {
        // �ҋ@������
        Idle,
        // �v���C���[��ǂ�
        Run,
        // �U��
        Attack,
        // ���S
        Die
    }

    void Start()
    {
        // �ҋ@���
        state = State.Idle;
        player = GameObject.FindWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player != null&&Hp>0)
        {

            // �v���C���[�Ƃ̋������v�Z
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            Debug.Log("�G�l�~�[�ƃv���C���[�̋���" + distanceToPlayer);
            // �v���C���[�Ƃ̋��������ȉ��Ȃ�U�����[�h�Ɉڍs
            if (distanceToPlayer <= 2f)
            {
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                transform.forward = directionToPlayer;
                navMeshAgent.isStopped = true;
                state = State.Attack;
                return;
            }
            else
            {
                // �v���C���[�̈ʒu���擾
                Vector3 playerDirection = player.position - transform.position;
                float angle = Vector3.Angle(transform.forward, playerDirection);
                // �v���C���[���U���͈͓��ɂ��邩�m�F

                // �v���C���[������90�x�ȓ��ɂ��邩�m�F
                if (angle <= trackingAngle / 2f && distanceToPlayer <= maxDistance)
                {
                    navMeshAgent.isStopped = false;
                    state = State.Run;
                    // �v���C���[�̕����Ɍ������Ēǐ�
                    navMeshAgent.SetDestination(player.position);
                }
                else if (distanceToPlayer > maxDistance)
                {
                    navMeshAgent.isStopped = true;
                    state = State.Idle;

                    // ���������͈͂ɖ߂�
                    // �����ʂ̍s���������Ŏ��s
                    // ��: �ҋ@���邩�A�����_���ȕ����Ɉړ�����
                }

            }
        }
    }

    private void ResetEnemy()
    {
        state = State.Idle;
        navMeshAgent.isStopped = false;
        Hp = 1;
    }


    // �G�l�~�[���S
    public void EnemyDie()
    {
         
        state = State.Die;
        
    }

    // ���S���A�A�j���[�V�����N���b�v�ɂĔ�\���ɂ���
    public void EnemyFalse()
    {
        ResetEnemy();
        this.gameObject.SetActive(false);
        
    }
   
}