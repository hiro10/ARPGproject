using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ChangeSkyMoveManager : MonoBehaviour
{
    // �ω�������_�V�F�[�_�[�̃}�e���A��
    [SerializeField] private Material cloudMat;

    // �Z���̒l
    private float smoothness=0f;
  
    // Update is called once per frame
    void Update()
    {
        // �_�̑��������炷
        smoothness += 0.4f*Time.deltaTime;
        cloudMat.SetFloat("_CloudPawer", smoothness);
    }

    private void OnDestroy()
    {
        GameManager.Instance.isMovePlaying = false;
        cloudMat.SetFloat("_CloudPawer", 0);
    }

    /// <summary>
    /// �^�C�����C���ɃC�x���g�o�^����֐�
    /// </summary>
    public void StartEndMove()
    {
        // ���[�{�[���n�܂����^�C�~���O�ŃQ�[���}�l�[�W���[�̒l���X�V
        GameManager.Instance.isMovePlaying = true;
    }
}
