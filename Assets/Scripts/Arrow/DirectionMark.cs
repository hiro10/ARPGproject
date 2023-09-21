using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionMark : MonoBehaviour
{
    //=============================================================================
    //     �ϐ�
    //=============================================================================
    // �v���C���[�̏��
    [SerializeField, Tooltip("�v���C���[�I�u�W�F�N�g")]
    private Transform player = null;

    // �J�������
    [SerializeField, Tooltip("�v���C���[���f���J����")]
    private Transform Camera = null;

    // �^�[�Q�b�g
    [SerializeField, Tooltip("�ǂ�������^�[�Q�b�g")]
    public List<Transform> targets; // �^�[�Q�b�g�̃��X�g

    private Transform closestTarget; // �ł��������߂��^�[�Q�b�g
    [SerializeField] GameObject arrowPearent;
    //=============================================================================
    //     �v���p�e�B
    //=============================================================================
    public Transform SetPlayer { get { return player; } set { player = value; } }
    public Transform SetCamera { get { return Camera; } set { Camera = value; } }
   // public Transform SetTarget { get { return Target; } set { Target = value; } }

    //=============================================================================
    //     �A�b�v�f�[�g
    //=============================================================================
    void Update()
    {
        TurnAroundDirectionTarget();
    }

    //=============================================================================
    //     ������]������
    //=============================================================================
    private void TurnAroundDirectionTarget()
    {
        float closestDistance = float.MaxValue; // �ł��������߂��^�[�Q�b�g�܂ł̋�����������

        targets.RemoveAll(obj => obj.GetComponent<EnemySpawner>().inPlayer == true);

        if(targets.Count<=0)
        {
            Destroy(arrowPearent);
        }

        foreach (Transform target in targets)
        {
            // �^�[�Q�b�g�ƃv���C���[�̈ʒu���擾
            Vector3 targetPosition = target.position;
            Vector3 playerPosition = player.position;

            // �^�[�Q�b�g�ƃv���C���[�̋������v�Z
            float distance = Vector3.Distance(targetPosition, playerPosition);

            // �ł��������߂��^�[�Q�b�g���������ꍇ�A���̃^�[�Q�b�g���L�^
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }


        // �v���C���[����^�[�Q�b�g�܂ł̃x�N�g�����v�Z
        Vector3 Direction = (closestTarget.position - player.transform.position).normalized;

        // ���߂������ւ̉�]�ʂ����߂�
        Quaternion RotationalVolume = Quaternion.LookRotation(Direction, Vector3.up);

        // �J�����������ɉ�]�ʂ̕␳
        Quaternion CorrectionVolume = Quaternion.FromToRotation(Camera.transform.forward, Vector3.forward);

        Vector3 vec = (RotationalVolume * CorrectionVolume).eulerAngles;

        transform.rotation = Quaternion.Euler(0, vec.y, 0);
    }
}