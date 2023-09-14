using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class EnemyController : MonoBehaviour
{
    // �v���C���[�̈ʒu
    private Transform player;
    private NavMeshAgent navMeshAgent;

    // �v���C���[��ǐՂ���p�x
    private float trackingAngle = 180f; 

    // �v���C���[�Ƃ̍ő勗��
    private float maxDistance = 10f;

    [SerializeField] GameObject findPlayerEffect;
    // ���݂̃G�l�~�[�̏��
    public State state;

    

    Animator animator;

    float distanceToPlayer;

    private int maxHp;
    // ���݂�Hp
    private int currentHp;
    //Slider������
    public Slider slider;

    // �X�N���v�^�u���I�u�W�F�N�g�ŃX�e�[�^�X�Ǘ�
    public EnemyStateSo enemyState;
    public enum State
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
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        maxHp = enemyState.maxHealth;
        //Slider�𖞃^���ɂ���B
        slider.value = 1;
        //���݂�HP���ő�HP�Ɠ����ɁB
        currentHp = maxHp;
        // �ҋ@���
        Idle();
       
    }

    void Update()
    {
        Debug.Log("currentHp" + currentHp);
        if (player != null)
        {
            if (currentHp <= 0)
            {
                Die();
            }


            // �v���C���[�Ƃ̋������v�Z
            distanceToPlayer = Vector3.Distance(transform.position, player.position);
           // Debug.Log("�G�l�~�[�ƃv���C���[�̋���" + distanceToPlayer);

            // �v���C���[�Ƃ̋��������ȉ��Ȃ�U�����[�h�Ɉڍs
            if (distanceToPlayer <= 2f)
            {
                Attack();
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
                    Run();
                }
                else if (distanceToPlayer > maxDistance)
                {
                    Idle();
                    // ���������͈͂ɖ߂�
                    // �����ʂ̍s���������Ŏ��s
                    // ��: �ҋ@���邩�A�����_���ȕ����Ɉړ�����
                }

            }
        }
        
    }

    private void ResetEnemy()
    {
        //Slider�𖞃^���ɂ���B
        slider.value = 1;
        //���݂�HP���ő�HP�Ɠ����ɁB
        currentHp = maxHp;
        state = State.Idle;
        navMeshAgent.isStopped = false;
        findPlayerEffect.SetActive(false);
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

    // �ҋ@���
    private void Idle()
    {
        // ��Ԃ�ҋ@
        state = State.Idle;
        // �ҋ@�A�j���[�V����
        animator.SetTrigger("Idle");
        // �I�[��������
        findPlayerEffect.SetActive(false);
        // �i�r���b�V�����~�߂�
        navMeshAgent.isStopped = true;
    }

    // �v���C���[��ǂ������鏈��
    private void Run()
    {
        state = State.Run;
        animator.SetTrigger("Run");
        findPlayerEffect.SetActive(true);
        navMeshAgent.isStopped = false;
        
        // �v���C���[�̕����Ɍ������Ēǐ�
        navMeshAgent.SetDestination(player.position);
    }

    // �U�����
    private void Attack()
    {
        state = State.Attack;
        animator.SetTrigger("Attack");
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        findPlayerEffect.SetActive(true);
        transform.forward = directionToPlayer;
        navMeshAgent.isStopped = true;
        
    }
    // ���S���
    private void Die()
    {
        state = State.Die;
        animator.SetTrigger("Die");
    }
   
    public int CurrentHp()
    {
        return currentHp;
    }

    public void SetCurrentHp(int damage)
    {
        currentHp -= damage;
    }

    public int MaxHp()
    {
        return maxHp;
    }
}