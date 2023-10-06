using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
// �U���q�b�g���̃��A�N�V�����j
public class DamageReaction : MonoBehaviour
{
    //�̂�����ɂ��_���[�W���o�ŉ�]����{�[��
    [SerializeField] private Transform waistBone;

    //�̂�����ɂ��_���[�W���o�ɂ��{�[���̉�]�p�x
    private Vector3 offsetAnglesWaist;

    private Sequence seq;

    [SerializeField] float damp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wepon"&&this.gameObject.tag=="Enemy")
        {
            // �q�b�g�����U������̐i�s�����Ɍ����đ̂�|�����߁A�p�x���擾
            var bulletAngles = other.transform.eulerAngles;
            // X�p�x�͖���
            bulletAngles.x = 0f;
            HitTiltWaist(Quaternion.Euler(bulletAngles) * Vector3.forward);
        }
        else if((other.gameObject.tag == "EnemysWepon" && this.gameObject.tag == "Player"))
        {
            // �q�b�g�����U������̐i�s�����Ɍ����đ̂�|�����߁A�p�x���擾
            var bulletAngles = other.transform.eulerAngles;
            // X�p�x�͖���
            bulletAngles.x = 0f;
            HitTiltWaist(Quaternion.Euler(bulletAngles) * Vector3.forward);
        }
    }

    /// <summary> �̂�����i�{�[����]�j�ɂ��_���[�W���o���Đ� </summary>
    /// <param name="vector">�q�b�g�����U������̐i�s����</param>
    private void HitTiltWaist(Vector3 vector)
    {
        seq?.Kill();
        seq = DOTween.Sequence();
        // �U������̐i�s���������g��Transform�̃��[�J�����W�n�ɕϊ�
        vector = transform.InverseTransformVector(vector);
        // �U������̐i�s�����Ɍ�����10�x�X����
        // FIXME: Vector3�̂ǂ̗v�f�ɍU������̐i�s�����x�N�g���̊e�v�f���g�����A��/�����߂��ǂ����邩�́A���f���̃{�[���\���ɍ��킹�ēK�X�ύX����
        var tiltAngles = new Vector3(0f, -vector.x, -vector.z).normalized * damp;
        seq.Append(DOTween.To(() => Vector3.zero, angles => offsetAnglesWaist = angles, tiltAngles, 0.1f));
        seq.Append(DOTween.To(() => tiltAngles, angles => offsetAnglesWaist = angles, Vector3.zero, 0.2f));
        seq.Play();
    }

    private void LateUpdate()
    {
        // Animator�ɂ�鍡�t���[���̃{�[���̊p�x�����܂�����A�̂�����ɂ���]�p�x��^����
        waistBone.localEulerAngles += offsetAnglesWaist;
    }
}
