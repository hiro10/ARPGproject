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
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerBoost;
    [Header("Prefabs")]
    public GameObject particle;

    // カメラにアタッチされているPost Processing Volume
    [SerializeField]private Volume volume;

    // Vignetteの設定
    private Vignette vignette;

    // 変更するVignetteの色
    public Color targetColor;
    public Color defaultColor;

    // Start is called before the first frame update
    void Start()
    {
        // Volumeを取得
        volume = Camera.main.GetComponent<Volume>();
        // VolumeからVignetteの設定を取得
        volume.profile.TryGet(out vignette);

        rotationObj = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if(volume==null)
        //{
        //    // Volumeを取得
        //    volume = Camera.main.GetComponent<Volume>();
        //    // VolumeからVignetteの設定を取得
        //    volume.profile.TryGet(out vignette);
        //}
    }

    public void OnROtationObj(InputAction.CallbackContext context)
    {
        Debug.Log("押された");
        if (context.started)
        {
            Instantiate(particle, player.transform.position, Quaternion.identity);
            if (rotationObj == false)
            {
                // Vignetteの色を変更
                
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
    }
}