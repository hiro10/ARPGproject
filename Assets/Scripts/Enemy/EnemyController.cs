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
    // �i�r���b�V��
    private NavMeshAgent navMeshAgent;
    // �v���C���[��ǐՂ���p�x
    private float trackingAngle = 180f; 
    // �v���C���[�Ƃ̍ő勗��
    private float maxDistance = 10f;
    // �v���C���[�������ɐԂ��Ȃ�I�u�W�F�N�g
    [SerializeField] GameObject findPlayerEffect;
    // ���݂̃G�l�~�[�̏��
    public State state;
    // �A�j���[�^�[
    Animator animator;
    // �v���C���[�Ƃ̋���
    float distanceToPlayer;
    // �ő�Hp�@�@
    private int maxHp;
    // ���݂�Hp
    private int currentHp;
    // Hp�pSlider������
    public Slider slider;
    // ������΂��I�u�W�F�N�g��Rigidbody
    private Rigidbody rb;
    // �X�N���v�^�u���I�u�W�F�N�g�ŃX�e�[�^�X�Ǘ�
    public EnemyStateSo enemyState;
    // �V�[���}�l�[�W���[
    BattleSceneManager sceneManager;
    //
    public Collider enemyCollider;

    // �v���C���[���烍�b�N�I������Ă��邩�H
    public bool lockOn;

    //�G�l�~�[����v���C���[�ւ̍U��
    [SerializeField] EnemyToPlayerDamageManager damageManager;

    // �U���̃N�[���_�E�����ԁi�b�j
    private float attackCooldown = 2.0f; 
    // ���b�X�V�̍U�����Ԃ̃X�p��
    public float attackTimer = 0.0f;

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

    /// <summary>
    /// �J�n����
    /// �e��Ԕ���p�ϐ��̏������A�擾
    /// </summary>
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

    /// <summary>
    /// �X�V����
    /// </summary>
    void Update()
    {
        if (player != null)
        {
            // Hp��0�ȉ��Ȃ玀�S
            if (currentHp <= 0)
            {
              
                Die();
                //nemyDie();
            }

            else
            {
                // �v���C���[�Ƃ̋������v�Z
                distanceToPlayer = Vector3.Distance(transform.position, player.position);

                // �v���C���[�Ƃ̋��������ȉ��Ȃ�U�����[�h�Ɉڍs
                if (distanceToPlayer <= 2f)
                {
                    attackTimer += Time.deltaTime;
                    // �U�����[�V�����p�̑ҋ@���[�V�����ɐ؂�ւ�
                    animator.SetBool("BattleIdle", true);
                    // ����h�~�Ńi�r���b�V�����~�߂�
                    navMeshAgent.isStopped = true;
                    // �U���ҋ@���ԁi���j
                    if (attackTimer >= attackCooldown)
                    {
                        animator.SetBool("BattleIdle", false);
                        Attack();
                        attackTimer = 0.0f; // �^�C�}�[�����Z�b�g
                    }
                   
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
    /// �ď���������
    /// </summary>
    private void ResetEnemy()
    {
        rb.isKinematic = true;
        navMeshAgent.enabled = true;
        //Slider�𖞃^���ɂ���B
        slider.value = 1;
        //���݂�HP���ő�HP�Ɠ����ɁB
        currentHp = maxHp;
        // ��Ԃ�ҋ@��
        state = State.Idle;
        // �G�t�F�N�g���\����
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
        animator.SetBool("BattleIdle", false);
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

    // �U����Ԃ̏���
    // 
    private void Attack()
    {
        // �U����ԂɕύX
        state = State.Attack;
        if (state == State.Attack)
        {   
            animator.SetTrigger("Attack");
            // ���̓����蔻���L����
            damageManager.SwordCollision(true);
            // �G�l�~�[����v���C��[�ւ̒P�ʃx�N�g�������߂�
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            
            findPlayerEffect.SetActive(true);
            transform.forward = directionToPlayer;
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
            // ������΂�����
            Vector3 forceDirection = -transform.forward;
            // ������΂�����
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
           
            animator.SetTrigger("Attack"); // �U���A�j���[�V�������Đ�
        }
        else
        {
            // �ҋ@��I��
            Idle(true);
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