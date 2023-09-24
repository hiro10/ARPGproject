using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//�G�l�~�[�̓���
public class EnemyController : MonoBehaviour
{
    // �v���C���[�̈ʒu
    private Transform player;
    private NavMeshAgent navMeshAgent;

    // �v���C���[��ǐՂ���p�x
    private float trackingAngle = 180f; 

    // �v���C���[�Ƃ̍ő勗��
    private float maxDistance = 10f;

    // �v���C���[�������ɐԂ��Ȃ�I�u�W�F�N�g
    [SerializeField] GameObject findPlayerEffect;

    // ���݂̃G�l�~�[�̏��
    public State state;

    Animator animator;

    // �v���C���[�Ƃ̋���
    float distanceToPlayer;

    private int maxHp;
    // ���݂�Hp
    private int currentHp;
    //Slider������
    public Slider slider;

    private Rigidbody rb; // ������΂��I�u�W�F�N�g��Rigidbody
    private float forceMagnitude = 5f; // ������΂��͂̑傫��
    

    // �X�N���v�^�u���I�u�W�F�N�g�ŃX�e�[�^�X�Ǘ�
    public EnemyStateSo enemyState;

    BattleSceneManager sceneManager;
    public Collider enemyCollider;

    // �v���C���[���烍�b�N�I������Ă��邩�H
    public bool lockOn;

    [SerializeField] EnemyToPlayerDamageManager damageManager;

    private float actionTimer = 0f;
    private bool isAttacking = false;

    public enum State
    {
        // �ҋ@������
        Idle,
        // �v���C���[��ǂ�
        Run,
        // �U��
        Attack,
        // ���S
        Die,
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        sceneManager = GameObject.FindWithTag("BattleSceneManager").GetComponent<BattleSceneManager>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        maxHp = enemyState.maxHealth;
        //Slider�𖞃^���ɂ���B
        slider.value = 1;
        //���݂�HP���ő�HP�Ɠ����ɁB
        currentHp = maxHp;
        // �ҋ@���
        Idle(false);
        rb = GetComponent<Rigidbody>();
        enemyCollider = GetComponent<Collider>();
        lockOn = false;
    }

    void Update()
    {
        if (player != null)
        {
            if (currentHp <= 0)
            {
                Die();
            }

            else
            {
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
                        if (!lockOn)
                        {
                            Run();
                        }
                        else
                        {
                            Idle(true);
                        }
                    }
                    else if (distanceToPlayer > maxDistance)
                    {
                        Idle(false);
                        // ���������͈͂ɖ߂�
                        // �����ʂ̍s���������Ŏ��s
                        // ��: �ҋ@���邩�A�����_���ȕ����Ɉړ�����
                    }

                }
            }
        }
        
    }

    /// <summary>
    /// ����������
    /// </summary>
    private void ResetEnemy()
    {
        rb.isKinematic = true;
        navMeshAgent.enabled = true;
        //Slider�𖞃^���ɂ���B
        slider.value = 1;
        //���݂�HP���ő�HP�Ɠ����ɁB
        currentHp = maxHp;
        state = State.Idle;
        //navMeshAgent.isStopped = false;
        findPlayerEffect.SetActive(false);
        enemyCollider.enabled = true;
        lockOn = false;
    }


    // �G�l�~�[���S
    public void EnemyDie()
    {
        navMeshAgent.enabled = false;
        state = State.Die;
        sceneManager.DeadCount++;
        sceneManager.AllEnemyDeadCount++;
    }

    // ���S���A�A�j���[�V�����N���b�v�ɂĔ�\���ɂ���
    public void EnemyFalse()
    {
        ResetEnemy();
        this.gameObject.SetActive(false);
        
    }

    // �ҋ@���
    private void Idle(bool isBattle)
    {
        // ��Ԃ�ҋ@
        state = State.Idle;
        if (state == State.Idle)
        {
            // �ҋ@�A�j���[�V����
            animator.SetTrigger("Idle");
            if (!isBattle)
            {
                // �I�[��������
                findPlayerEffect.SetActive(false);
            }
            // �i�r���b�V�����~�߂�
            navMeshAgent.isStopped = true;
        }
    }

    // �v���C���[��ǂ������鏈��
    private void Run()
    {
        state = State.Run;
        if (state == State.Run)
        {
            animator.SetTrigger("Run");
            findPlayerEffect.SetActive(true);
            navMeshAgent.isStopped = false;

            // �v���C���[�̕����Ɍ������Ēǐ�
            navMeshAgent.SetDestination(player.position);
        }
    }

    // �U�����
    private void Attack()
    {
        state = State.Attack;
        if (state == State.Attack)
        {
            animator.SetTrigger("Attack");
            damageManager.SwordCollision(true);
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            findPlayerEffect.SetActive(true);
            transform.forward = directionToPlayer;
            navMeshAgent.isStopped = true;
        }
    }

    public void AttackEnd()
    {
        damageManager.SwordCollision(false);
    }
    // ���S���
    private void Die()
    {
        state = State.Die;
        if (state == State.Die)
        {
            navMeshAgent.enabled = false;
            findPlayerEffect.SetActive(false);
            rb.isKinematic = false;
            Vector3 forceDirection = -transform.forward; // ������΂������i��: �I�u�W�F�N�g�̑O���j
            rb.AddForce(Vector3.up * 0.5f, ForceMode.Impulse);
            rb.AddForce(forceDirection, ForceMode.Impulse);
            enemyCollider.enabled = false;
            animator.SetTrigger("Die");
        }
    }

    // �����_���ȃA�N�V�����̊J�n
    private void StartRandomAction()
    {
        // �����_����0����1�̊Ԃ̒l���擾���A�U�����ҋ@��I��
        float randomValue = Random.Range(0f, 1f);
        if (randomValue <= 0.2f) // 0.2�i20%�j�ȉ��Ȃ�U��
        {
            // �U����I��
            isAttacking = true;
            animator.SetTrigger("Attack"); // �U���A�j���[�V�������Đ�
            actionTimer = 2f; // �U���A�j���[�V�����̒����ɍ��킹��ȂǁA�K�؂Ȏ��Ԃ�ݒ�
        }
        else
        {
            // �ҋ@��I��
            animator.SetTrigger("Idle"); // �ҋ@�A�j���[�V�������Đ�
            // �ҋ@���Ԃ�ݒ肵�A�ҋ@�A�j���[�V�������I��������ēx�����_���ȃA�N�V�������J�n
            float waitTime = Random.Range(1f, 3f);
            Invoke("StartRandomAction", waitTime);
        }
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