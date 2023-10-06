using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 回転オブジェクトの処理（将来的には覚醒機能の処理）
/// </summary>
public class RotationObjects : MonoBehaviour
{
    [SerializeField] List<GameObject> rotObjects;
    [SerializeField]bool rotationObj;
    [SerializeField] Transform EffectGeneratePos;
    [SerializeField] GameObject playerBoost;
    [SerializeField] GameObject player;
    [Header("Prefabs")]
    public GameObject particle;

    // カメラにアタッチされているPost Processing Volume
    [SerializeField]private Volume volume;

    // Vignetteの設定
    private Vignette vignette;

    // 変更するVignetteの色
    public Color targetColor;
    public Color defaultColor;

   
    void Start()
    {
        // Volumeを取得
        volume = Camera.main.GetComponent<Volume>();
        // VolumeからVignetteの設定を取得
        volume.profile.TryGet(out vignette);

        rotationObj = false;
    }
   
    public void OnROtationObj(InputAction.CallbackContext context)
    {
        Debug.Log("押された");
        if (context.started&& player.GetComponent<PlayerData>().PlayerCurrentAwake>=100)
        {
           
            if (rotationObj == false)
            {
                // 発生エフェクトの再生
                Instantiate(particle, EffectGeneratePos.transform.position, Quaternion.identity);
                // Vignetteの色を変更
                // プレイヤーの判定を覚醒状態に
                player.GetComponent<PlayerController>().IsAwakening = true;
                playerBoost.SetActive(true);
                SoundManager.instance.PlaySE(SoundManager.SE.RotOn);
                rotationObj = true;
                vignette.color.Override(targetColor);
                vignette.smoothness.Override(1f);
                for (int i = 0; i < rotObjects.Count; i++)
                {
                    rotObjects[i].GetComponent<RotateUnit>().OnRoitationWepons();
                }


            }
            else if (rotationObj == true)
            {
                OffRotationObj();
            }
        }
    }

    public void OffRotationObj()
    {
        // 発生エフェクトの再生
        Instantiate(particle, EffectGeneratePos.transform.position, Quaternion.identity);
        // プレイヤーの判定を覚醒状態に
        player.GetComponent<PlayerController>().IsAwakening = false;
        playerBoost.SetActive(false);
        SoundManager.instance.PlaySE(SoundManager.SE.RotOff);
        rotationObj = false;
        vignette.color.Override(defaultColor);
        vignette.smoothness.Override(0.35f);
        for (int i = 0; i < rotObjects.Count; i++)
        {
            rotObjects[i].GetComponent<RotateUnit>().OffRoitationWepons();
        }
    }
}