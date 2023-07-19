using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNeckController : MonoBehaviour
{
    public Transform enemy; // �G�l�~�[��Transform�R���|�[�l���g
    public Transform neckBone; // ���Transform�R���|�[�l���g
    public float detectionRadius = 10f; // ���m�͈͔��a
    public float maxAngle = 45f; // �v���C���[���猩���ő�p�x
    [SerializeField] PlayerLockOn playerLock;
    private Quaternion originalRotation; // ������]�̕ۑ�

    private void Start()
    {
        originalRotation = neckBone.localRotation;
    }

    private void FixedUpdate()
    {
        if (playerLock.target!=null)
        {
            enemy = playerLock.target.transform;
            // �v���C���[�ƃG�l�~�[�̋������v�Z
            float distance = Vector3.Distance(transform.position, enemy.position);

            if (distance <= detectionRadius)
            {
                // �v���C���[����G�l�~�[�ւ̕����x�N�g�����v�Z
                Vector3 direction = enemy.position - transform.position;
                direction.y = 0f; // y�������̉�]�𖳌���

                // �v���C���[���猩���G�l�~�[�̊p�x���v�Z
                float angle = Vector3.Angle(transform.forward, direction);

                if (angle <= maxAngle)
                {
                    // ��̉�]���G�l�~�[�̕����ɕ⊮�I�ɕύX
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    neckBone.rotation = Quaternion.Slerp(originalRotation, targetRotation, 0.5f);
                }
                else
                {
                    // ������]�ɖ߂�
                    neckBone.localRotation = originalRotation;
                }
            }
            else
            {
                // ������]�ɖ߂�
                neckBone.localRotation = originalRotation;
            }
        }
        else
        {
            // ������]�ɖ߂�
            neckBone.localRotation = originalRotation;
        }
    }
    //public Transform enemy; // �G�l�~�[��Transform�R���|�[�l���g
    //public float detectionRadius = 10f; // ���m�͈͔��a
    ////public float maxAngle = 360f; // �v���C���[���猩���ő�p�x
    //[SerializeField] PlayerLockOn playerLock;
    //private void FixedUpdate()
    //{
    //    if (playerLock.target)
    //    {
    //        enemy = playerLock.target.transform;
    //        // �v���C���[�ƃG�l�~�[�̋������v�Z
    //        float distance = Vector3.Distance(transform.position, enemy.position);

    //        if (distance <= detectionRadius)
    //        {
    //            // �v���C���[����G�l�~�[�ւ̕����x�N�g�����v�Z
    //            Vector3 direction = enemy.position - transform.position;
    //           // direction.y = 0f; // y�������̉�]�𖳌���

    //            // �v���C���[���猩���G�l�~�[�̊p�x���v�Z
    //            //float angle = Vector3.Angle(transform.forward, direction);

    //           // if (angle <= maxAngle)
    //            {
    //                // �v���C���[�̕������G�l�~�[�̕����Ɍ�����
    //                transform.LookAt(enemy);
    //            }
    //        }
    //    }
    //}

    //private void TargetIn()
    //{
    //    enemy = playerLock.target.transform;
    //}
}
