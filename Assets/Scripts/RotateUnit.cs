using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUnit : MonoBehaviour
{
    //�@���񂷂�^�[�Q�b�g
    [SerializeField]
    private Transform target;
    //�@���݂̊p�x
    [SerializeField]private float angle;
    //�@��]����X�s�[�h
    [SerializeField]
    private float rotateSpeed = 180f;
    //�@�^�[�Q�b�g����̋���
    [SerializeField]
    private Vector3 distanceFromTarget = new Vector3(0f, 1f, 1f);
    private void Start()
    {
        Debug.Log("�t�@���g���\�[�h�I��");
    }

    // Update is called once per frame
    void LateUpdate()
    {

        //�@���j�b�g�̈ʒu = �^�[�Q�b�g�̈ʒu �{ �^�[�Q�b�g���猩�����j�b�g�̊p�x �~�@�^�[�Q�b�g����̋���
        transform.position = target.position + Quaternion.Euler(0f, angle, 0f) * distanceFromTarget;
        //�@���j�b�g���g�̊p�x = �^�[�Q�b�g���猩�����j�b�g�̕����̊p�x���v�Z����������j�b�g�̊p�x�ɐݒ肷��
        transform.rotation = Quaternion.LookRotation(transform.position - new Vector3(target.position.x, transform.position.y, target.position.z), Vector3.up);
        //�@���j�b�g�̊p�x��ύX
        angle += rotateSpeed * Time.deltaTime;
        //�@�p�x��0�`360�x�̊ԂŌJ��Ԃ�
        angle = Mathf.Repeat(angle, 360f);
    }
}
