using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AudioManager : MonoBehaviour
{
    public Transform player;             // プレイヤーのTransform
    public string enemyTag = "Enemy";     // エネミーのタグ
    public AudioSource[] bgmSources;      // BGMのオーディオソース（複数）
    public AudioClip newBgmClip;         // 新しいBGMのオーディオクリップ
    public float fadeInTime = 1f;         // クロスフェードのフェードイン時間
    public AudioClip beforeBgm;
    private bool isCrossFading;           // クロスフェード中かどうか
    private bool isInRange;               // 一定範囲内にいるかどうか
    private AudioSource activeBgmSource;  // アクティブなBGMオーディオソース
    private float baseVolume;
    private void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.BGM.Field);
      
       
        isCrossFading = false; // クロスフェード中のフラグを初期化
        isInRange = false;     // 一定範囲内にいるかどうかのフラグを初期化

        // 最初のアクティブなBGMオーディオソースを設定
        activeBgmSource = bgmSources[0];
        baseVolume = bgmSources[0].volume;
    }

    private void Update()
    {
        // エネミーの一定範囲内に入ったかどうかを判定
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

        // 一定範囲内に入った時の処理
        if (isInEnemyRange && !isInRange)
        {
            // クロスフェードを開始
            CrossFadeBgm(newBgmClip, fadeInTime);
            isInRange = true;
        }
        // 一定範囲外に出た時の処理
        else if (!isInEnemyRange && isInRange)
        {
            // クロスフェードを開始して元のBGMに戻す
            CrossFadeBgm(beforeBgm, fadeInTime);
            isInRange = false;
        }
    }

    private void CrossFadeBgm(AudioClip nextBgmClip, float fadeTime)
    {
        if (isCrossFading) return;

        isCrossFading = true; // クロスフェード中のフラグを設定

        // アクティブなBGMオーディオソースの切り替え
        activeBgmSource = (activeBgmSource == bgmSources[0]) ? bgmSources[1] : bgmSources[0];

        // 新しいBGMをセットして再生
        activeBgmSource.clip = nextBgmClip;
        activeBgmSource.Play();

        // クロスフェード
        activeBgmSource.volume = 0f;
        activeBgmSource.DOFade(baseVolume, fadeTime)
            .OnComplete(() =>
            {
                isCrossFading = false; // クロスフェード中のフラグを解除
            });

        // 他のBGMオーディオソースの音量を徐々に下げる
        foreach (var bgmSource in bgmSources)
        {
            if (bgmSource != activeBgmSource)
            {
                bgmSource.DOFade(0f, fadeTime);
            }
        }
    }
}