using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 画面にかかるエフェクトの管理クラス
/// ポストエフェクトとレンダーパイプラインのマテリアル
/// </summary>
public class FullScreenEfectManager : MonoBehaviour
{
    // 画面エフェクト用マテリアル
    [SerializeField] private Material fillScreenMat;

    // 画面エフェクトの代入変数
    private float effectValue;
    
    // プレイヤーコントろらーの取得 
    [SerializeField] PlayerController player;

    // プレイヤーが覚醒中か
    bool playerAwake;

    // カメラにアタッチされているPost Processing Volume
    [SerializeField] private Volume volume;

    // Vignetteの設定
    private Vignette vignette;

    // 変更するVignetteの色
    public Color targetColor;
    public Color defaultColor;
    public Color endColor;

    // 覚醒時の発生するカットイン
    [SerializeField] CutInManager cutInManager;

    void Start()
    {
        // 画面エフェクトの初期化
        ChengeFullScreenEffect(0f);

        // プレイヤーは未覚醒
        playerAwake = false;

        // Volumeを取得
        volume = Camera.main.GetComponent<Volume>();

        // VolumeからVignetteの設定を取得
        volume.profile.TryGet(out vignette);
    }


    void Update()
    {
        if(GameManager.Instance.isMovePlaying)
        {// ムービー中は画面効果を出さない
            ChengeFullScreenEffect(0f);
            vignette.color.Override(endColor);
            vignette.smoothness.Override(0.35f);
        }

        if(player.IsAwakening&&!playerAwake)
        {// 覚醒発動時（各種効果をonに）
            cutInManager.StoryEventTriggered();
            ChengeFullScreenEffect(0.05f);
            playerAwake = true;
            vignette.color.Override(targetColor);
            vignette.smoothness.Override(1f);
        }
        else if(!player.IsAwakening && playerAwake)
        {// 覚醒解除時（各種効果をoffに）
            ChengeFullScreenEffect(0f);
            playerAwake = false;
            vignette.color.Override(defaultColor);
            vignette.smoothness.Override(0.35f);
        }
    }
    /// <summary>
    /// レンダーパイプラインのマテリアルのシェーダーの変数を変える
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
