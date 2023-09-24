using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
// �U���q�b�g���̃��A�N�V�����j
public class DamageReaction : MonoBehaviour
{
    /// <summary> �̂�����ɂ��_���[�W���o�ŉ�]����{�[�� </summary>
    [SerializeField]
    private Transform _waistBone;

    /// <summary> �̂�����ɂ��_���[�W���o�ɂ��{�[���̉�]�p�x </summary>
    private Vector3 _offsetAnglesWaist;

    private Sequence _seq;

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
        _seq?.Kill();
        _seq = DOTween.Sequence();
        // �U������̐i�s���������g��Transform�̃��[�J�����W�n�ɕϊ�
        vector = transform.InverseTransformVector(vector);
        // �U������̐i�s�����Ɍ�����10�x�X����
        // FIXME: Vector3�̂ǂ̗v�f�ɍU������̐i�s�����x�N�g���̊e�v�f���g�����A��/�����߂��ǂ����邩�́A���f���̃{�[���\���ɍ��킹�ēK�X�ύX����
        var tiltAngles = new Vector3(0f, -vector.x, -vector.z).normalized * damp;
        _seq.Append(DOTween.To(() => Vector3.zero, angles => _offsetAnglesWaist = angles, tiltAngles, 0.1f));
        _seq.Append(DOTween.To(() => tiltAngles, angles => _offsetAnglesWaist = angles, Vector3.zero, 0.2f));
        _seq.Play();
    }

    private void LateUpdate()
    {
        // Animator�ɂ�鍡�t���[���̃{�[���̊p�x�����܂�����A�̂�����ɂ���]�p�x��^����
        _waistBone.localEulerAngles += _offsetAnglesWaist;
    }
}
