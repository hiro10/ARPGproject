using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �v���C���[��Hp�c�ʂɉ����ĕ\������v���C���[�A�C�R����ς���N���X
public class PlayerIconSwitcher : MonoBehaviour
{
    // Hp�X���C�_�[
    [Header("�v���C���[��Hp�X���C�_�[")]
    [SerializeField] private Slider hpSlider;
    // Hp��1/3�ȏ�̎��ɕ\������摜
    [Header("�ʏ펞�̃A�C�R��")]
    [SerializeField] private Sprite normalImage;

    // Hp���s���`�̎��ɕ\������摜
    [Header("�s���`���̃A�C�R��")]
    [SerializeField] private Sprite crisisImage;

    private Image imageComponent;
    void Start()
    {
        imageComponent = GetComponent<Image>();  
    }

    // Update is called once per frame
    void Update()
    {
        // �X���C�_�[�̒l��1/3�ȉ��̏ꍇ�A�摜��؂�ւ���
        if (hpSlider.value <= 1.0f / 3.0f)
        {
            imageComponent.sprite = crisisImage;
        }
        else
        {
            // �X���C�_�[�̒l��1/3����̏ꍇ�A���̉摜�ɖ߂�
            imageComponent.sprite = normalImage;
        }
    }
}
