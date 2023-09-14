using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPC�����͈͓��Ȃ�player�̕�������������
/// </summary>
public class NpcController : MonoBehaviour
{
    // �v���C���[�̈ʒu
    public Transform player;
    // ��]���x
    public float rotationSpeed = 5f;
    // ��]�͈�
    public float detectionRange = 10f;
    // Npc�̏�����]�ʒu
    private Quaternion initialRotation;

    /// <summary>
    /// �J�n����
    /// </summary>
    void Start()
    {
        // �I�u�W�F�N�g�̏�����]��ۑ�
        initialRotation = transform.rotation;
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    void Update()
    {
        // �v���C���[��NPC�̋������v�Z
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �v���C���[���͈͓��ɂ���ꍇ
        if (distanceToPlayer <= detectionRange)
        {
            // �v���C���[�̕�������
            Vector3 directionToPlayer = player.position - transform.position;
            
            // Y���������[���ɐݒ肷�邱�Ƃ�X����]�������Ȃ�
            directionToPlayer.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            
            // �⊮���Ȃ���player�̕�������
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // ���̌����ɖ߂�
            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
