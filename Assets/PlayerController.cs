using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Vector3 move;
    private Vector3 moveForward;
    [Header("�ړ����x")]
    [SerializeField] private float moveSpeed;
    [Header("��]����")]
    [SerializeField] private float turnTimeRate;

    private CameraController cameraController;

    // �A�N�V�����t���O�i��𒆂��j
    [SerializeField]private bool avoid = false;
    // �ړ������t���O
    [SerializeField] private bool mov = true;
    // ��]�����t���O
    [SerializeField] private bool rot = true;

    Animator animator;

    [SerializeField] private PlayableDirector[] timeline;

    //�U���p�A�N�V�����t���O
    public bool attack=false;

    // �R���{����p�t���O
    private int comboFlgl;

    // �R���{��
    private int coumboCount=0;

    [SerializeField] private PlayableDirector[] attackTimeline;

   

    // �X�e�B�b�N�p�x
    private float degree;

    /// <summary>
    /// �J�n����
    /// </summary>
    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    private void Update()
    {
        SticeAngle();

        Debug.Log("�R���{�t���O��" + comboFlgl);

        Debug.Log(degree);
        if (animator==null)
        {
            return;
        }
        if (mov)
        {

            Move();
        }
     
        if (avoid&&move.magnitude==0)
        {
            rigidbody.AddForce(-transform.forward * 4.5f, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        if (rot)
        {
            if (cameraController.rock)
            {
                // ���b�N�I�����̓e�[�Q�b�g�̐��ʂ�
                var dir = cameraController.rockonTarget.transform.position - this.gameObject.transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnTimeRate);
            }
            else
            {
                // ��]
                Rotation();
            }
        }
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

        if(avoid)
        {
            if(move.magnitude>0)
            {
                rigidbody.AddForce(moveForward * 50f, ForceMode.Impulse);
            }
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
            if(!avoid)
            {
                // �ړ����
                if(move.magnitude>0)
                {
                    timeline[0].Play();
                    MoveOff();
                    RotaionOff();
                }
                else if (move.magnitude > 0)
                {
                    timeline[0].Play();
                    MoveOff();
                    RotaionOff();
                }
                else if (move.magnitude > 0)
                {
                    timeline[0].Play();
                    MoveOff();
                    RotaionOff();
                }
                //�ʏ���
                else
                {
                    timeline[1].Play();
                    MoveOff();
                    RotaionOff();
                }
                avoid = true;
            }
        }
    }

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
        
        if (context.started)
        {
            if (!attack && !avoid)
            {
                attack = true;

                switch(coumboCount)
                {
                    case 0:
                        attackTimeline[0].Play();
                        Debug.Log("�U���{�^���������ꂽ");
                        break;
                }
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
        coumboCount = 0;
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

}
