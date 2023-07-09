using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

// �o�����̉�]�I�u�W�F�N�g�p
public class RotateUnit : MonoBehaviour
{
    //�@���񂷂�^�[�Q�b�g
    [SerializeField]
    private Transform target;
    //�@���݂̊p�x
    [SerializeField]private float angle;
    //�@��]����X�s�[�h
    float rotateSpeed=1000f;
    //�@�^�[�Q�b�g����̋���
    [SerializeField]
    private Vector3 distanceFromTarget = new Vector3(0f, 1f, 1f);
    private void Awake()
    {
        gameObject.SetActive(false);
        this.transform.localScale = Vector3.zero; 
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

    public void OnRoitationWepons()
    {
        gameObject.SetActive(true);
        DOTween.To(() => 0.01f, (n) => Time.timeScale = n, 1f, 0.2f).SetEase(Ease.Linear);
        DOTween.To(() => 1000f, (n) => rotateSpeed = n, 270f, 1.5f).SetEase(Ease.Linear);
        
        this.gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetLink(gameObject);
    }

    public void OffRoitationWepons()
    {
        this.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }
}
