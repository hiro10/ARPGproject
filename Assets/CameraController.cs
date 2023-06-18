using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    // �J�����̑���X�s�[�h
    private Vector3 speed;

    // �v���C���[�̒Ǐ]�ݒ�
    // �v���C���[�I�u�W�F�N�g
    [SerializeField] private GameObject TargetObject;
    
    // �J�����̍����I�t�Z�b�g
    [SerializeField] private float height = 1.5f;
    // �J�����Ƃ̃I�t�Z�b�g
    [SerializeField] private float distance = 15f;
    // �������̃J�����p�x
    [SerializeField] private float rotAngle = 0f;
    // �c�����̃J�����p�x
    [SerializeField] private float heightAngle = 10f;
    // ���グ�����̃J��������
    [SerializeField] private float dis_min = 5f;
    // �ʏ�̃J��������
    [SerializeField] private float dis_mdl = 10f;
    // ���݂̃v���C���[�ʒu
    private Vector3 nowPos;
    // ���݂̉������̃J�����p�x
    private float nowRotAngle;
    // ���݂̐��������̃J�����p�x
    private float nowHeightAngle;

    /// <summary>
    /// ��������
    /// </summary>
    // �����t���O
    [SerializeField]private bool enableAtten = true;
    // �␳�p�p�����[�^
    [SerializeField] private float attenRate = 3.0f;
    [SerializeField] private float forawardDistance = 2.0f;
    private Vector3 addForeward;
    private Vector3 prevTargetPos;
    [SerializeField] private float rotAngleAttenRate=5.0f;
    [SerializeField] private float angleAttenRate = 1f;

    // ���b�N�I���@�\
    [Header("���b�N�I������")]
    public bool rock = false;
    public GameObject rockonTarget;
    // ���b�N�I���p�Z���T�[
    public GameObject searchCircle;

    // Start is called before the first frame update
    void Start()
    {
        nowPos = TargetObject.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // �L�[���͂ɂ�鐅�������̉��Z
        rotAngle -= speed.x * Time.deltaTime * 50f;
        // �L�[���͂ɂ�鐂�������̉��Z
        heightAngle += speed.z * Time.deltaTime * 50f;
        // ���������̊p�x����
        heightAngle = Mathf.Clamp(heightAngle, -40f, 60f);
        // �J������������
        distance = Mathf.Clamp(distance, 5f, 40f);

        // �^�[�Q�b�g
        rockonTarget = searchCircle.GetComponent<RockonSensor>().nowTarget;
        
        if(enableAtten)
        {
            // �^�[�Q�b�g�ʒu���v���C���[��
            var target = TargetObject.transform.position;
            
            // ���b�N�I���@�\
            if(rock)
            {
                if(rockonTarget!=null)
                {
                    // �^�[�Q�b�g�����b�N�I���Ώۂɂ���
                    target = rockonTarget.transform.position;
                }
                else
                {
                    rock = false;
                }
            }
            
            var halfPoint = (TargetObject.transform.position + target) / 2;
            // �ʒu�̔���������
            var deltaPos = halfPoint - prevTargetPos;
            prevTargetPos = halfPoint;
            deltaPos *= forawardDistance;

            addForeward += deltaPos * Time.deltaTime * 20f;
            addForeward = Vector3.Lerp(addForeward, Vector3.zero, Time.deltaTime * attenRate);

            nowPos = Vector3.Lerp(nowPos, halfPoint + Vector3.up * height + addForeward, Mathf.Clamp01(Time.deltaTime * attenRate));
        }
        else
        {
            // ��ʂ̒��S�ʒu
            nowPos = TargetObject.transform.position + Vector3.up * height;
        }

        if(enableAtten)
        {
            nowRotAngle = Mathf.Lerp(nowRotAngle, rotAngle, Time.deltaTime * rotAngleAttenRate);
        }
        else
        {
            // �J�����̐����A�����p�x
            nowRotAngle = rotAngle;
        }
        if (enableAtten)
        {
            nowHeightAngle = Mathf.Lerp(nowHeightAngle, heightAngle, Time.deltaTime * rotAngleAttenRate);
        }
        else
        {
            nowHeightAngle = heightAngle;
        }

        if (rock)
        {
            var dis = Vector3.Distance(TargetObject.transform.position, rockonTarget.transform.position);

            // �J�����̐����p�x�ɂ��J��������
            if (heightAngle > 30)
            {
                //distance = Mathf.Lerp(distance, 20f * heightAngle / 30f, Time.deltaTime);
                distance = Mathf.Lerp(distance, dis_min*dis/10*heightAngle/30f, Time.deltaTime);
            }
            else if (heightAngle <= 30 && heightAngle >= -3)
            {
                distance = Mathf.Lerp(distance, dis_mdl*dis/10f, Time.deltaTime);
            }
            else if (heightAngle < -3)
            {
                distance = Mathf.Lerp(distance, dis_min, Time.deltaTime);

            }
        }
        else
        {
            // �J�����̐����p�x�ɂ��J��������
            
                distance = Mathf.Lerp(distance, dis_min, Time.deltaTime);

            
        }
            //distance = Mathf.Lerp(distance, dis_min, Time.deltaTime);
            // �J�����̈ʒu���W������
            var deg = Mathf.Deg2Rad;
        var cx = Mathf.Sin(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * distance;
        var cz = -Mathf.Cos(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * distance;
        var cy = Mathf.Sin(nowHeightAngle * deg) * distance;

        transform.position = nowPos + new Vector3(cx, cy, cz);
        var rot = Quaternion.LookRotation(nowPos - transform.position).normalized;
        if (enableAtten)
        {
            transform.rotation = rot;
        }
        else
        {
            transform.rotation = rot;
        }
    }

    public void OnCamera(InputAction.CallbackContext context)
    {
        speed = new Vector3(context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y);
    }

    public void OnRockOn(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(rock)
            {
                rock = false;
            }
            else
            {
                rock = true;
            }
        }
    }
}
