using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//TODO:�v�C��
public class PlayerLockOn : MonoBehaviour
{
    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] Transform originTrn;
    [SerializeField] float lockonRange = 20;
    [SerializeField] string targetLayerName;
    [SerializeField] GameObject lockonCursor;
    [SerializeField] GameObject player;
    public GameObject target;


    bool lockonInput = false;
    bool isLockon = false;

    Camera mainCamera;
    Transform cameraTrn;
    


    void Start()
    {
        mainCamera = Camera.main;
        cameraTrn = mainCamera.transform;
    }


    void Update()
    {
       
        // ��苗���O�ɏo���烍�b�N�I�����O��
        if (target != null)
        {
            // ���łɃ��b�N�I���ς݂Ȃ��������
            if (Vector3.Distance(target.transform.position, originTrn.position) >= lockonRange|| target.activeSelf == false)
            {
                isLockon = false;
                target = null;
                playerCamera.InactiveLockonCamera();
                lockonCursor.SetActive(false);
                lockonInput = false;
                return;
            }
        }
        // ���b�N�I���{�^�����������ۂ̏���
        if (lockonInput)
        {
            // ���łɃ��b�N�I���ς݂Ȃ��������
            if (isLockon)
            {
                isLockon = false;
                target = null;
                playerCamera.InactiveLockonCamera();
                lockonCursor.SetActive(false);
                lockonInput = false;
                return;
            }

            // ���b�N�I���Ώۂ̌����A����Ȃ烍�b�N�I���A���Ȃ��Ȃ�J�����p�x�����Z�b�g
            target = GetLockonTarget();
            if (target/*&&target.gameObject.GetComponent<EnemyController>().CurrentHp()>=0*/)
            {
                isLockon = true;
                playerCamera.ActiveLockonCamera(target);
                lockonCursor.SetActive(true);
            }
            else
            {
                playerCamera.ResetFreeLookCamera();
            }
            lockonInput = false;
        }

        // ���b�N�I���J�[�\��
        if (isLockon)
        {
            lockonCursor.transform.position = mainCamera.WorldToScreenPoint(new Vector3(target.transform.position.x, target.transform.position.y + 1f, target.transform.position.z));
        }
    }
    // ���b�N�I�����͂��󂯎��֐�
    public void OnLockOn(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("���b�N�I��");
            if (lockonInput == false)
                lockonInput = true;
            else
                lockonInput = false;

        }

    }

    /// <summary>
    /// ���b�N�I���Ώۂ̌v�Z�������s���擾����
    /// �v�Z��3�̍H���ɕ������
    /// </summary>
    /// <returns></returns>
    GameObject GetLockonTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // "enemy"�^�O�̑S�ẴI�u�W�F�N�g���擾

        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            Vector3 direction = enemy.transform.position - transform.position; // �J��������G�ւ̕����x�N�g��
            float angle = Vector3.Angle(transform.forward, direction); // �J��������G�ւ̕����ƃJ�����̐��ʂƂ̊p�x

            if (angle <= 45.0f)
            {
                float distanceToPlayer = Vector3.Distance(enemy.transform.position, player.transform.position); // �G�ƃv���C���[�̋���

                if (distanceToPlayer < closestDistance)
                {
                    closestEnemy = enemy;
                    closestDistance = distanceToPlayer;
                }
            }
        }

        
        return closestEnemy;

    }
}