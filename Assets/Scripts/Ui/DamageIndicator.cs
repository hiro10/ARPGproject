using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// �_���[�W���󂯂����ɃL�����o�X�Ƀ_���[�W���l���o���N���X
public class DamageIndicator : MonoBehaviour
{
    // �_���[�W�e�L�X�g�v���n�u
    public TextMeshProUGUI damageTextPrefab;
    // �L�����o�X
    public Transform canvasTransform;
    // �\������
    private float displayDuration = 0.5f;
  
    /// <summary>
    /// �_���[�W���L�����o�X�ɕ\������֐�
    /// </summary>
    /// <param name="position">�\���ʒu</param>
    /// <param name="damage">�_���[�W���l</param>
    public void ShowDamageIndicator(Vector3 position, int damage)
    {
        // �G�l�~�[�̍U�������������ꏊ�Ƀ_���[�W���l��\��
        TextMeshProUGUI damageText = Instantiate(damageTextPrefab, canvasTransform);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
        damageText.transform.position = screenPosition;

        // �_���[�W���l��ݒ�
        damageText.text = damage.ToString();

        // dotween�ŃA�j���[�V��������ꂽ��
        // ���b��Ƀ_���[�W���l������
        Destroy(damageText.gameObject, displayDuration);
    }
}
