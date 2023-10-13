using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using Cinemachine;
using ARPG.Dialogue;

// �v���C���[�̋����S��
public class PlayerController : MonoBehaviour
{
    private new Rigidbody rigidbody;
    public Vector3 move;
    public Vector3 moveForward;
    [Header("�ړ����x")]
    [SerializeField] private float moveSpeed;
    [Header("��]����")]
    [SerializeField] private float turnTimeRate;
    public LayerMask layerMask;
    // �A�N�V�����t���O�i��𒆂��j
    public bool avoid ;
    // �ړ������t���O
    [SerializeField] private bool mov = true;
    // ��]�����t���O
    [SerializeField] private bool rot = true;
    //�A�j���[�^�[
    Animator animator;

    [SerializeField] private PlayableDirector[] timeline;

    //�U���p�A�N�V�����t���O
    public bool attack;

    // �R���{����p�t���O
    private int comboFlgl;

    // �R���{��
    private int coumboCount = 0;

    [SerializeField] private PlayableDirector[] attackTimeline;
    [SerializeField] GameObject wepon;
    [SerializeField] WarpConntroller warpConntroller;
    [SerializeField] PlayerLockOn playerLockOn;
    [SerializeField] PlayerAttackController playerAttack;

    // �n�ʂɐڂ��Ă��邩
    public bool isGrounded;
    // ���݂̃X�e�[�g
    public PLAYER_STATE state;

    private float lockOnSpeed = 10f;
    
    // ������̃X�s�[�h
    private float avoidSpeed = 5f;
    // ���ݒn
    private Vector3 nowPosition;

    [SerializeField] PlayerData playerData;
    [SerializeField] BattleSceneManager sceneManager;
    [SerializeField] PlayerConversant playerConversant;
    [SerializeField] RotationObjects rotationObjects;
    // �v���C���[������ł��邩
    bool playerDead;

    // �o�������ǂ���
    private bool isAwakening;
    public bool IsAwakening
    {
        get
        {
            return isAwakening;
        }
        set
        {
            isAwakening = value;
        }
    }

    public enum PLAYER_STATE
    {
        TOWN,    // ���ɂ���Ƃ�
        BATTLE,  // �퓬��
        RESULT,
        ENDING
    }
    /// <summary>
    /// �J�n����
    /// </summary>
    private void Awake()
    {
        isAwakening = false;
        playerDead = false;
        isGrounded = true;
        AttackOff();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        animator = GetComponent<Animator>();
        wepon.SetActive(false);
    }

    private void Start()
    {
        ChangeState();
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    private void Update()
    {
        if (playerData.PlayerCurrentHp <= 0 && !playerDead && state != PLAYER_STATE.RESULT)
        {// Hp��0
            PlayerDead();
        }
        //�@�W�����v�͂��A�j���[�V�����p�����[�^�ɐݒ�i�v�C���j
        animator.SetFloat("FoolSpeed", rigidbody.velocity.y);
        animator.SetBool("isGround", isGrounded);

        //�ڒn����
        isGrounded = CheckGrounded();
        if (isGrounded)
        {
            animator.SetFloat("FoolSpeed", 0f);
        }
        if (attack == true || avoid == true || playerConversant.isTaking)
        {
            // �U������y���̗͂𔭐������Ȃ�
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
            RotaionOff();
            MoveOff();
        }
        else
        {
            MoveOn();
            RotaionOn();
        }
        //  �ړ�����
        if (mov)
        {
            Move();
        }

        if (playerConversant.isTaking)
        {
            animator.SetFloat("Speed", 0f);
        }
        if(isAwakening&&playerData.PlayerCurrentAwake<=0)
        {
            rotationObjects.OffRotationObj();
        }
    }
   
    private void FixedUpdate()
    {
        // ���̎��̓��b�N�I���ł��Ȃ�����
        if (state != PLAYER_STATE.TOWN)
        {
            if (playerLockOn.target != null)
            {
                animator.SetBool("LockOn", true);
            }
            else
            {
                animator.SetBool("LockOn", false);
            }
        }
        if (rot)
        { 
            // ��]
            Rotation();
        }

        if (warpConntroller.isWarp == false)
        {
            SetLocalGravity(); //�d�͂�AddForce�ł����郁�\�b�h���ĂԁBFixedUpdate���D�܂����B
        }
       
    }
    private void LateUpdate()
    {
        // �v���C���[�̉�]��ۑ�
        Quaternion savedRotation = transform.rotation;

        if (state == PLAYER_STATE.BATTLE)
        {
            if (playerLockOn.target && move.magnitude <= 0 && isGrounded && !avoid)
            {
                // �^�[�Q�b�g�̕������������߂̉�]���v�Z
                Quaternion targetRotation = Quaternion.LookRotation(playerLockOn.target.transform.position - transform.position);

                // ��ԏ������s���Ċ��炩�ɉ�]������
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lockOnSpeed * Time.fixedDeltaTime);

                // x����]�����ɖ߂�
                Vector3 eulerRotation = transform.rotation.eulerAngles;
                eulerRotation.x = savedRotation.eulerAngles.x;
                transform.rotation = Quaternion.Euler(eulerRotation);
            }
            // �������
            Avoid();
            DecAwakeGage();
        }
    }

    void DecAwakeGage()
    {
        if (isAwakening&&!GameManager.Instance.isPause)
        {
            playerData.PlayerCurrentAwake -= 10f*Time.deltaTime;
        }
    }
    /// <summary>
    /// �V�[���̏�Ԕ���
    /// </summary>
    void ChangeState()
    {
        if (GameManager.Instance.nowSceneName == "DemoScene" || GameManager.Instance.nowSceneName == "Test")
        {
            state = PLAYER_STATE.BATTLE;
        }
        else
        {
            state = PLAYER_STATE.TOWN;
        }
    }
    /// <summary>
    /// �x�[�X�̏d��
    /// </summary>
    private void SetLocalGravity()
    {
        rigidbody.AddForce(new Vector3(0f,-30f,0f), ForceMode.Acceleration);
    }
    /// <summary>
    /// �ړ�����
    /// </summary>
    private void Move()
    {
        // ���C���J�����̑O�������̃x�N�g��
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1f, 0f, 1f)).normalized;
        // �����L�[�̓��͒l�ƃJ�����̌�������ړ�����������
        moveForward = cameraForward * move.z + Camera.main.transform.right * move.x;
        moveForward = moveForward.normalized;
        var speedw = Mathf.Abs(rigidbody.velocity.z);
        // �ړ����x���A�j���[�^�[�ɔ��f
        animator.SetFloat("Speed", move.magnitude, 0.1f, Time.deltaTime);

        if (move.magnitude>0)
        {
            rigidbody.velocity = moveForward * moveSpeed * move.magnitude + new Vector3(0, rigidbody.velocity.y, 0);
        }
        // �������͂��Ă��Ȃ�
        else
        {
            rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);
        }    
    }
    /// <summary>
    /// ��]����
    /// </summary>
    private void Rotation()
    {
        // ���C���J�����̑O�������̃x�N�g��
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1f, 0f, 1f)).normalized;
        // �����L�[�̓��͒l�ƃJ�����̌�������ړ�����������
        moveForward = cameraForward * move.z + Camera.main.transform.right * move.x;
        moveForward = moveForward.normalized;
        if (move.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnTimeRate);
        }
        // �������͂��Ă��Ȃ�
        else
        {
            Quaternion targetRotation = transform.rotation;
            transform.rotation = targetRotation;
        }
    }
    /// <summary>
    /// InputSystem���f�p
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        move = new Vector3(context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y);
    }
    /// <summary>
    /// ����̓��͏���
    /// </summary>
    /// <param name="context"></param>
    public void OnAvoid(InputAction.CallbackContext context)
    {
        Debug.Log("�����ꂽ");
        if(context.started&&state == PLAYER_STATE.BATTLE)
        {
            if(!avoid&&isGrounded&&!warpConntroller.isWarp )
            {
                avoid = true;
                // �ړ����
                if (move.magnitude > 0)
                {  
                    MoveOff();
                    RotaionOff();
                    animator.SetBool("Avoid",true);
                }

                //�ʏ���
                else
                {
                    MoveOff();
                    RotaionOff();
                    animator.SetBool("Avoid", true);                   
                }
                
            }
        }
    }
    
    /// <summary>
    /// ����A�j���[�V�����I�����i�A�j���[�V�����N���b�v�p�j
    /// </summary>
    public void AvoidEnd()
    {
        avoid = false;
        rigidbody.velocity = Vector3.zero;
        animator.SetBool("Avoid", false);   
    }

    /// <summary>
    /// �������
    /// TODP:�v�C��
    /// �s�x�Εӂ����߂Đ��K������
    /// </summary>
    private void Avoid()
    {
        if (avoid == true)
        {
            
            Vector3 playerForwardUp= (transform.forward + transform.up).normalized;
            Vector3 playerForwardDown = Vector3.zero;//= (transform.forward - transform.up).normalized;
            // �v���C���[����O���Ƀ��C���΂�
            Ray rayFront = new Ray(transform.position, transform.forward);
            float rayFrontDistance = 1f; // ���C�̒���
            // ���C�L���X�g�̌��ʂ��i�[����ϐ�
            RaycastHit hitFront;

            // ���C���v���C���[�̑������牺�����ɔ�΂�
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;
            float distanceToGround=0f;
            if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Ground")
            {
                 distanceToGround = hit.distance;
            }
           
            // ���C�L���X�g�̎��s
            if (Physics.Raycast(rayFront, out hitFront, rayFrontDistance))
            {
                // ���C��"ground"�^�O�ɓ��������ꍇ�̏���
                if (hitFront.collider.CompareTag("Ground"))
                {
                    rigidbody.velocity = playerForwardUp * avoidSpeed + new Vector3(0, rigidbody.velocity.y, 0);
                }
            }
            else if (!isGrounded)
            {
                playerForwardDown = (transform.forward - new Vector3(0, distanceToGround, 0)).normalized;
                rigidbody.velocity = playerForwardDown * avoidSpeed + new Vector3(0, rigidbody.velocity.y, 0);
            }
            else
            {
                rigidbody.velocity = transform.forward * avoidSpeed + new Vector3(0, rigidbody.velocity.y, 0);
                
            }
        }
    }

    /// <summary>
    /// �U���̓��͏���
    /// </summary>
    /// <param name="context"></param>
    public void OnAttack(InputAction.CallbackContext context)
    {
        // �퓬���
        if (state == PLAYER_STATE.BATTLE)
        {
            if (context.started)
            {
                if (!attack && !avoid && warpConntroller.isWarp == false)
                {
                    if (isGrounded)
                    {
                        attack = true;
                        playerAttack.StartAttack();
                        StartCoroutine(ComboStart());
                    }
                }
            }
        }
    }
    IEnumerator ComboStart()
    {
        if (playerLockOn.target)
        {
            
            yield return new WaitForSeconds(0.3f);
            switch (coumboCount)
            {
                case 0:
                    attackTimeline[0].Play();
                    wepon.SetActive(true);
                    Debug.Log("�U���{�^���������ꂽ");
                    break;
            }
        }
        else
        {
            switch (coumboCount)
            {
                case 0:
                    attackTimeline[0].Play();
                    wepon.SetActive(true);
                    Debug.Log("�U���{�^���������ꂽ");
                    break;
            }
        }
     
    }
    public void OnCombo(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(attack&&comboFlgl==1)
            {
                comboFlgl = 2;
            }
        }
    }
    public void ComboEnable()
    {
        if(comboFlgl==0)
        {
            comboFlgl = 1;
        }
    }
    public void ComboCheak()
    {
        if(comboFlgl==2)
        {
            switch(coumboCount)
            {
                case 0:
                    attackTimeline[0].Stop();
                    attackTimeline[1].Play();
                    coumboCount = 1;
                    break;
                case 1:
                    attackTimeline[1].Stop();
                    attackTimeline[2].Play();
                    coumboCount = 2;
                    break;
                case 2:
                    attackTimeline[2].Stop();
                    attackTimeline[3].Play();
                    coumboCount = 3;
                    break;
                
            }
            comboFlgl = 0;
        }
    }
    public void AttackStop()
    {
        if(comboFlgl!=4)
        {
            comboFlgl=0;
        }
        attack = false;
        wepon.SetActive(false);
        coumboCount = 0;
    }

    /// <summary>
    ///  jump�̏����i�A�j���[�V�����N���b�v�ɂĎ��s�j
    /// </summary>
    void Jump()
    {
        MoveOff();
        rigidbody.velocity = Vector3.up * 10f;
    }

    /// <summary>
    /// �v���C���[�̎��S���̏���
    /// </summary>
    public void PlayerDead()
    {

        playerDead = true;
        state = PLAYER_STATE.RESULT;
        animator.SetTrigger("Dead");
        StartCoroutine(sceneManager.DeadResultStart());

    }

    /// <summary>
    /// InputSystem�p�@�W�����v�{�^������
    /// </summary>
    /// <param name="context"></param>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isGrounded)
            {
                Jump();
                //animator.SetTrigger("isJump");
            }
        }
    }

    /// <summary>
    /// �ڒn����
    /// </summary>
    /// <returns>�ڒn true ����ȊOfalse��Ԃ�</returns>
    bool CheckGrounded()
    {
        //�������̏����ʒu�Ǝp��
        var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        //�����̋���(����J�v�Z���I�u�W�F�N�g�ɐݒ肷��̂�Height/2 + 0.1�ȏ��ݒ�)
        var distance = 0.5f;
        //Raycast��hit���邩�ǂ����Ŕ��背�C���[���w�肷�邱�Ƃ��\
        return Physics.Raycast(ray, distance,layerMask);    
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Stage")
        {
            transform.position = nowPosition;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Stage")
        {
            nowPosition= transform.position;
        }
    }

    // �^�C�����C���Ăяo���p
    public void MoveOn()
    {
        mov = true;
    }
    public void MoveOff()
    {
        mov = false;
    }
    public void RotaionOn()
    {
        rot = true;
    }
    public void RotaionOff()
    {
        rot = false;
    }
    public void ActionFlugReset()
    {
        avoid = false;
    }

    public void AttackOn()
    {
        attack = true;
    }

    public void AttackOff()
    {
        attack = false;
    }
}
