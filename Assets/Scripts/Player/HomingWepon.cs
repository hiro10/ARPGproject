using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingWepon : MonoBehaviour
{
    public float missileSpeed = 20f; // �~�T�C���̑��x
    public float rotationSpeed = 10f; // �~�T�C���̉�]���x
    public float homingAngle = 180f; // ���b�N�I���̊p�x
    public Transform lockOnTarget; // ���b�N�I���^�[�Q�b�g
    [SerializeField] PlayerLockOn player;
    public Transform launchPosition; // ���ˈʒu
    void Update()
    {
        Homing();
    }

    void Homing()
    {

        lockOnTarget = player.target.transform;
        if (lockOnTarget != null)
        {

            // �^�[�Q�b�g�̕���������
            transform.LookAt(lockOnTarget);
            // ���b�N�I���^�[�Q�b�g������ꍇ�A���̕����Ɍ������Đi��
            Vector3 targetDirection = lockOnTarget.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * missileSpeed * Time.deltaTime;
        }
        else
        {
            // ���b�N�I���^�[�Q�b�g�����Ȃ��ꍇ�A�v���C���[�̐���180�x�ȓ��ň�ԋ߂��G��_��
            Collider[] colliders = Physics.OverlapSphere(transform.position, homingAngle, LayerMask.GetMask("Enemy"));
            if (colliders.Length > 0)
            {
                Transform nearestEnemy = null;
                float minDistance = float.MaxValue;

                foreach (var collider in colliders)
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestEnemy = collider.transform;
                    }
                }

                if (nearestEnemy != null)
                {
                    Vector3 targetDirection = nearestEnemy.position - transform.position;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), rotationSpeed * Time.deltaTime);
                    transform.position += transform.forward * missileSpeed * Time.deltaTime;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Enemy")
        {
            HitEnemy();
        }
    }

    // �^�[�Q�b�g�ɓ��������Ƃ��ɌĂяo����郁�\�b�h
    public void HitEnemy()
    {
        lockOnTarget = null; // �^�[�Q�b�g�����Z�b�g
        gameObject.SetActive(false); // �~�T�C�����A�N�e�B�u�ɂ���
        transform.position = launchPosition.position; // ���ˈʒu�ɖ߂�
    }
}