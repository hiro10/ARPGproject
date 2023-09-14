using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//TODO:Find�֐������̓G�l�~�[�̐���������ɃC�x���g�ʒm�ɕύX
public class AudioFadeController : MonoBehaviour
{
    public AudioSource bgmSource1;              // BGM�I�[�f�B�I�\�[�X1
    public AudioSource bgmSource2;              // BGM�I�[�f�B�I�\�[�X2
    public AudioClip normalBgm;                 // �ʏ��BGM�I�[�f�B�I�N���b�v
    public AudioClip battleBgm;                 // �o�g���p��BGM�I�[�f�B�I�N���b�v
    public Collider playerDetectionCollider;    // �v���C���[�̍��G�p�R���C�_�[
    public float baseVolume;
    [SerializeField] string tagName;
    bool hasEnemyInCollider;

    private bool isBattleBgmPlaying = false;     // �o�g���pBGM���Đ������ǂ����̃t���O
    private void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.BGM.Field);
        baseVolume = SoundManager.instance.audioSourceBGM.volume;
       
        bgmSource1 = SoundManager.instance.audioSourceBGM;
        
       
    }
    private void LateUpdate()
    {
        if (playerDetectionCollider != null)
        {
            hasEnemyInCollider = false;
            // �R���C�_�[����"Enemy"�^�O�̃Q�[���I�u�W�F�N�g�����邩�`�F�b�N
            Collider[] colliders = Physics.OverlapBox(playerDetectionCollider.bounds.center, playerDetectionCollider.bounds.extents, Quaternion.identity);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag(tagName))
                {
                    hasEnemyInCollider = true;
                    break;
                }
            }

            StartCoroutine(ChageBgm());
            
        }
    }
    IEnumerator ChageBgm()
    {
        if (hasEnemyInCollider && !isBattleBgmPlaying)
        {
            // �o�g���pBGM���Đ�����Ă��Ȃ��ꍇ�A�Ȃ��N���X�t�F�[�h���ăo�g���p��BGM�ɕύX
            CrossFadeBgm(battleBgm);
            yield return new WaitForSeconds(1f);
            isBattleBgmPlaying = true;
        }
        else if (!hasEnemyInCollider && isBattleBgmPlaying)
        {
            // �o�g���pBGM���Đ����łȂ��A�R���C�_�[����"Enemy"�^�O�̃Q�[���I�u�W�F�N�g���Ȃ��ꍇ�A�ʏ��BGM�ɖ߂�
            CrossFadeBgm(normalBgm);
            yield return new WaitForSeconds(1f);
            isBattleBgmPlaying = false;
        }
    }


    private void CrossFadeBgm(AudioClip nextBgm)
    {
        AudioSource fadeOutSource = isBattleBgmPlaying ? bgmSource1 : bgmSource2;
        AudioSource fadeInSource = isBattleBgmPlaying ? bgmSource2 : bgmSource1;
        // �t�F�[�h�A�E�g
        fadeOutSource.DOFade(0f, 1f).OnComplete(() =>
        {
            fadeOutSource.Stop();
            fadeOutSource.volume =0f;
        });

        // �t�F�[�h�C��
        fadeInSource.clip = nextBgm;
        fadeInSource.Play();
        fadeInSource.volume = 0f;
        fadeInSource.DOFade(baseVolume, 1f).SetEase(Ease.InSine);
        
    }

    private void OnDestroy()
    {
        if (SoundManager.instance.audioSourceBGM.volume == 0)
        {
            SoundManager.instance.audioSourceBGM.volume = baseVolume;
        }
    }
}

