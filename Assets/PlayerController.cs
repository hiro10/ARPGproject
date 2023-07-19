using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Cinemachine;

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

    Animator animator;

    [SerializeField] private PlayableDirector[] timeline;

    //�U���p�A�N�V�����t���O
    public bool attack;

    // �R���{����p�t���O
    private int comboFlgl;

    // �R���{��
    private int coumboCount=0;

    [SerializeField] private PlayableDirector[] attackTimeline;

    [SerializeField] GameObject wepon;

    // �X�e�B�b�N�p�x
    private float degree;

    public bool isGrounded;

    [SerializeField] WarpConntroller warpConntroller;
    [SerializeField] PlayerLockOn playerLockOn;

    [SerializeField] PlayerAttackController playerAttack;
    public PLAYER_STATE state;

    private float lockOnSpeed = 10f;
    Vector3 beforeGroundVec3;
    // ������̃X�s�[�h
    private float avoidSpeed = 5f;
    public enum PLAYER_STATE
    {
        TOWN,    // ���ɂ���Ƃ�
        BATTLE,  // �퓬��
    }

    /// <summary>
    /// �J�n����
    /// </summary>
    private void Awake()
    {
       
        isGrounded = true;
        AttackOff();
        // cameraController = Camera.main.GetComponent<CameraController>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        animator = GetComponent<Animator>();
        wepon.SetActive(false);
    }

    private void Start()
    {

        if (GameManager.Instance.nowSceneName == "DemoScene"|| GameManager.Instance.nowSceneName == "Test")
        {
            state = PLAYER_STATE.BATTLE;
        }
        else
        {
            state = PLAYER_STATE.TOWN;
        }
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    private void Update()
    {
        
        //�@�W�����v�͂��A�j���[�V�����p�����[�^�ɐݒ�i�v�C���j
        animator.SetFloat("FoolSpeed", rigidbody.velocity.y);
        animator.SetBool("isGround", isGrounded);
        //Debug.Log("isGrounded" + isGrounded);
        SticeAngle();

        //�ڒn����
        isGrounded = CheckGrounded();
        if (isGrounded)
        {
            animator.SetFloat("FoolSpeed", 0f);
        }
        if (attack == true || avoid == true)
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

       

    }

    private void FixedUpdate()
    {
       
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
    }



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
            //if (playerLockOn.target != null)
            //{
            //    transform.LookAt(playerLockOn.target.transform);
            //}
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

    public void OnAvoid(InputAction.CallbackContext context)
    {
        Debug.Log("�����ꂽ");
        if(context.started)
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
    /// �������p�x�ŏo����֐��i���̂Ƃ��떢�g�p�j
    /// </summary>
    void SticeAngle()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        degree = Mathf.Atan2(v, h) * Mathf.Rad2Deg;

        if (degree < 0)
        {
            degree += 360;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
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
                    else
                    {
                        //attack = true;
                        //attackTimeline[4].Play();
                        //wepon.SetActive(true);
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

        //animator.SetFloat("jumpPower",0);
        //�������̏����ʒu�Ǝp��
        var ray = new Ray(transform.position + Vector3.up * 0.01f, Vector3.down);
        //�����̋���(����J�v�Z���I�u�W�F�N�g�ɐݒ肷��̂�Height/2 + 0.1�ȏ��ݒ�)
        var distance = 0.8f;
        //Raycast��hit���邩�ǂ����Ŕ��背�C���[���w�肷�邱�Ƃ��\
        return Physics.Raycast(ray, distance,layerMask);

        
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
