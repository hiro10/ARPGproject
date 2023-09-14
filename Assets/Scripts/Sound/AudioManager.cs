using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AudioManager : MonoBehaviour
{
    public Transform player;             // �v���C���[��Transform
    public string enemyTag = "Enemy";     // �G�l�~�[�̃^�O
    public AudioSource[] bgmSources;      // BGM�̃I�[�f�B�I�\�[�X�i�����j
    public AudioClip newBgmClip;         // �V����BGM�̃I�[�f�B�I�N���b�v
    public float fadeInTime = 1f;         // �N���X�t�F�[�h�̃t�F�[�h�C������
    public AudioClip beforeBgm;
    private bool isCrossFading;           // �N���X�t�F�[�h�����ǂ���
    private bool isInRange;               // ���͈͓��ɂ��邩�ǂ���
    private AudioSource activeBgmSource;  // �A�N�e�B�u��BGM�I�[�f�B�I�\�[�X
    private float baseVolume;
    private void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.BGM.Field);
      
       
        isCrossFading = false; // �N���X�t�F�[�h���̃t���O��������
        isInRange = false;     // ���͈͓��ɂ��邩�ǂ����̃t���O��������

        // �ŏ��̃A�N�e�B�u��BGM�I�[�f�B�I�\�[�X��ݒ�
        activeBgmSource = bgmSources[0];
        baseVolume = bgmSources[0].volume;
    }

    private void Update()
    {
        // �G�l�~�[�̈��͈͓��ɓ��������ǂ����𔻒�
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        bool isInEnemyRange = false;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, player.position);
            if (distance <= 10f)
            {
                isInEnemyRange = true;
                break;
            }
        }

        // ���͈͓��ɓ��������̏���
        if (isInEnemyRange && !isInRange)
        {
            // �N���X�t�F�[�h���J�n
            CrossFadeBgm(newBgmClip, fadeInTime);
            isInRange = true;
        }
        // ���͈͊O�ɏo�����̏���
        else if (!isInEnemyRange && isInRange)
        {
            // �N���X�t�F�[�h���J�n���Č���BGM�ɖ߂�
            CrossFadeBgm(beforeBgm, fadeInTime);
            isInRange = false;
        }
    }

    private void CrossFadeBgm(AudioClip nextBgmClip, float fadeTime)
    {
        if (isCrossFading) return;

        isCrossFading = true; // �N���X�t�F�[�h���̃t���O��ݒ�

        // �A�N�e�B�u��BGM�I�[�f�B�I�\�[�X�̐؂�ւ�
        activeBgmSource = (activeBgmSource == bgmSources[0]) ? bgmSources[1] : bgmSources[0];

        // �V����BGM���Z�b�g���čĐ�
        activeBgmSource.clip = nextBgmClip;
        activeBgmSource.Play();

        // �N���X�t�F�[�h
        activeBgmSource.volume = 0f;
        activeBgmSource.DOFade(baseVolume, fadeTime)
            .OnComplete(() =>
            {
                isCrossFading = false; // �N���X�t�F�[�h���̃t���O������
            });

        // ����BGM�I�[�f�B�I�\�[�X�̉��ʂ����X�ɉ�����
        foreach (var bgmSource in bgmSources)
        {
            if (bgmSource != activeBgmSource)
            {
                bgmSource.DOFade(0f, fadeTime);
            }
        }
    }
}