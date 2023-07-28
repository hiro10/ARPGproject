using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// ���̃I�u�W�F�N�g����v���C���[�̋��������߂� 
/// </summary>
public class DistanceCalculator : MonoBehaviour
{
    public Transform playerTransform;
    public Transform targetObject; // �������v�Z�������Ώۂ̃I�u�W�F�N�g
    public TextMeshProUGUI distanceText; // TextMeshProUGUI�I�u�W�F�N�g

    private void Update()
    {
        // �^�[�Q�b�g�I�u�W�F�N�g���ݒ肳��Ă���ꍇ�̂ݏ������s��
        if (targetObject != null)
        {
            // �v���C���[�i���̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g�j����^�[�Q�b�g�I�u�W�F�N�g�܂ł̋������v�Z
            float distance = Vector3.Distance(playerTransform.position, targetObject.position);

            // ���������[�g���ɕϊ����Đ����ɕϊ�
            int distanceInMeters = Mathf.RoundToInt(distance * 10); 

            // TextMeshProUGUI�ɋ�����\��
            distanceText.text = distanceInMeters.ToString() + " m"; // �����_�ȉ�2���ŕ\��
        }
    }
}
