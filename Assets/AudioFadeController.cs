using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//TODO:Find関数部分はエネミーの生成処理後にイベント通知に変更
public class AudioFadeController : MonoBehaviour
{
    public AudioSource bgmSource1;              // BGMオーディオソース1
    public AudioSource bgmSource2;              // BGMオーディオソース2
    public AudioClip normalBgm;                 // 通常のBGMオーディオクリップ
    public AudioClip battleBgm;                 // バトル用のBGMオーディオクリップ
    public Collider playerDetectionCollider;    // プレイヤーの索敵用コライダー
    public float baseVolume;
    [SerializeField] string tagName;
    bool hasEnemyInCollider;

    private bool isBattleBgmPlaying = false;     // バトル用BGMが再生中かどうかのフラグ
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
            // コライダー内に"Enemy"タグのゲームオブジェクトがあるかチェック
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
            // バトル用BGMが再生されていない場合、曲をクロスフェードしてバトル用のBGMに変更
            CrossFadeBgm(battleBgm);
            yield return new WaitForSeconds(1f);
            isBattleBgmPlaying = true;
        }
        else if (!hasEnemyInCollider && isBattleBgmPlaying)
        {
            // バトル用BGMが再生中でなく、コライダー内に"Enemy"タグのゲームオブジェクトがない場合、通常のBGMに戻す
            CrossFadeBgm(normalBgm);
            yield return new WaitForSeconds(1f);
            isBattleBgmPlaying = false;
        }
    }


    private void CrossFadeBgm(AudioClip nextBgm)
    {
        AudioSource fadeOutSource = isBattleBgmPlaying ? bgmSource1 : bgmSource2;
        AudioSource fadeInSource = isBattleBgmPlaying ? bgmSource2 : bgmSource1;
        // フェードアウト
        fadeOutSource.DOFade(0f, 1f).OnComplete(() =>
        {
            fadeOutSource.Stop();
            fadeOutSource.volume =0f;
        });

        // フェードイン
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

