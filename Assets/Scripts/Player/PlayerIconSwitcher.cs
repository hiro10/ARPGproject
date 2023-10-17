using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// プレイヤーのHp残量に応じて表示するプレイヤーアイコンを変えるクラス
public class PlayerIconSwitcher : MonoBehaviour
{
    // Hpスライダー
    [Header("プレイヤーのHpスライダー")]
    [SerializeField] private Slider hpSlider;
    // Hpが1/3以上の時に表示する画像
    [Header("通常時のアイコン")]
    [SerializeField] private Sprite normalImage;

    // Hpがピンチの時に表示する画像
    [Header("ピンチ時のアイコン")]
    [SerializeField] private Sprite crisisImage;

    private Image imageComponent;
    void Start()
    {
        imageComponent = GetComponent<Image>();  
    }

    // Update is called once per frame
    void Update()
    {
        // スライダーの値が1/3以下の場合、画像を切り替える
        if (hpSlider.value <= 1.0f / 3.0f)
        {
            imageComponent.sprite = crisisImage;
        }
        else
        {
            // スライダーの値が1/3より上の場合、元の画像に戻す
            imageComponent.sprite = normalImage;
        }
    }
}
