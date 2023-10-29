using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// ��ʂɂ�����G�t�F�N�g�̊Ǘ��N���X
/// �|�X�g�G�t�F�N�g�ƃ����_�[�p�C�v���C���̃}�e���A��
/// </summary>
public class FullScreenEfectManager : MonoBehaviour
{
    // ��ʃG�t�F�N�g�p�}�e���A��
    [SerializeField] private Material fillScreenMat;

    // ��ʃG�t�F�N�g�̑���ϐ�
    private float effectValue;
    
    // �v���C���[�R���g���[�̎擾 
    [SerializeField] PlayerController player;

    // �v���C���[���o������
    bool playerAwake;

    // �J�����ɃA�^�b�`����Ă���Post Processing Volume
    [SerializeField] private Volume volume;

    // Vignette�̐ݒ�
    private Vignette vignette;

    // �ύX����Vignette�̐F
    public Color targetColor;
    public Color defaultColor;
    public Color endColor;

    // �o�����̔�������J�b�g�C��
    [SerializeField] CutInManager cutInManager;

    void Start()
    {
        // ��ʃG�t�F�N�g�̏�����
        ChengeFullScreenEffect(0f);

        // �v���C���[�͖��o��
        playerAwake = false;

        // Volume���擾
        volume = Camera.main.GetComponent<Volume>();

        // Volume����Vignette�̐ݒ���擾
        volume.profile.TryGet(out vignette);
    }


    void Update()
    {
        if(GameManager.Instance.isMovePlaying)
        {// ���[�r�[���͉�ʌ��ʂ��o���Ȃ�
            ChengeFullScreenEffect(0f);
            vignette.color.Override(endColor);
            vignette.smoothness.Override(0.35f);
        }

        if(player.IsAwakening&&!playerAwake)
        {// �o���������i�e����ʂ�on�Ɂj
            cutInManager.StoryEventTriggered();
            ChengeFullScreenEffect(0.05f);
            playerAwake = true;
            vignette.color.Override(targetColor);
            vignette.smoothness.Override(1f);
        }
        else if(!player.IsAwakening && playerAwake)
        {// �o���������i�e����ʂ�off�Ɂj
            ChengeFullScreenEffect(0f);
            playerAwake = false;
            vignette.color.Override(defaultColor);
            vignette.smoothness.Override(0.35f);
        }
    }
    /// <summary>
    /// �����_�[�p�C�v���C���̃}�e���A���̃V�F�[�_�[�̕ϐ���ς���
    /// </summary>
    /// <param name="volume"></param>
    private void ChengeFullScreenEffect(float volume)
    {
        fillScreenMat.SetFloat("_FullScreenIntensity", volume);
    }

    private void OnDestroy()
    {
        ChengeFullScreenEffect(0f);
    }
}
