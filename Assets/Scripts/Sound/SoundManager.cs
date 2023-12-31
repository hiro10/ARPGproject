﻿using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // 音を鳴らす
    // AudioSource:: スピーカー
    // AudioClip:: CD（素材）

    //BGM
    public AudioSource audioSourceBGM;
    [SerializeField] AudioClip[] audioClipsBGM;

    //SE
    [SerializeField] AudioSource audioSourceSE;
    [SerializeField] AudioClip[] audioClipsSE;

    private const float BGM_VOLUME_DEFULT = 1.0f;
    private const float SE_VOLUME_DEFULT = 1.0f;

    /// <summary>
    /// BGMの列挙型
    /// </summary>
    public enum BGM
    {
        Title, // タイトル画面用の曲
        Village,   // ゲーム画面用の曲
        Field,
        Boss1,
        Boss2,
        Town,
        ResultMusic,
        TownClear // クリア後の村のBGM
    }

    /// <summary>
    /// BGMの列挙型
    /// </summary>
    public enum SE
    {
        RotOn, // ボールが破裂するとき
        RotOff,   // ボールに触れた時
        ScreenCrash, // 爆弾が爆発するとき
        ScreenExpoltion, // ボタンの決定音
        WarpAttack, // ワープ攻撃の音
        ShiftThrow,// カウントダウンの音
        AttackHitSe,// プレイヤーからエネミーへの攻撃の音
        GaugeMaxSe,
        EnemyAttack1,//エネミーのナイフ攻撃
        PlaterAwakeGardSe,// 覚醒時にダメージSE
        EnemyAttack2,//プレイヤーのダメージモーション時のSe
        HealingSe,// 回復SE
    }

    // シングルトンにする
    // ゲーム内にただ一つのだけのもの（シーンが変わっても破壊されない）
    // どのコードからもアクセスしやすい
    public static SoundManager instance;
    /// <summary>
    ///  シングルトンのお約束
    /// </summary>
    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        // 音量データの読み込み（からの場合は１を入れる）
       audioSourceBGM.volume = PlayerPrefs.GetFloat("BGM_VOLUME", BGM_VOLUME_DEFULT);
       audioSourceSE.volume = PlayerPrefs.GetFloat("SE_VOLUME", SE_VOLUME_DEFULT);
    }

    /// <summary>
    /// BGMの再生
    /// </summary>
    public void PlayBGM(BGM bgm)
    {
        // 列挙型から流したいBGMを選ぶ（intでキャスト）
        audioSourceBGM.clip = audioClipsBGM[(int)bgm];
        audioSourceBGM.Play();
       
    }

    public void StopBgm()
    {
        audioSourceBGM.Stop();
    }

    /// <summary>
    /// SEの再生
    /// </summary>
    /// <param name="se"></param>
    public void PlaySE(SE se)
    {
        audioSourceSE.PlayOneShot(audioClipsSE[(int)se]);
        
    }

    /// <summary>
    /// BGMの音量変更
    /// </summary>
    /// <param name="BGMVolume"> スライダーのボリューム </param>
    public void ChangeVolumeBGM(float BGMVolume)
    {
         audioSourceBGM.volume = BGMVolume;
        //audioSourceBGM.volume = 0.2f;
        // 音量の保存
        PlayerPrefs.SetFloat("BGM_VOLUME", audioSourceBGM.volume);
       
    }

    /// <summary>
    /// SEの音量変更
    /// </summary>
    /// <param name="SEVolume"> スライダーのボリューム </param>
    public void ChangeVolumeSE(float SEVolume)
    {
        audioSourceSE.volume = SEVolume;

        // 音量の保存
        PlayerPrefs.SetFloat("SE_VOLUME", audioSourceSE.volume);

    }
}
