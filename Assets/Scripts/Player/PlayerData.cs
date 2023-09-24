using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
public class PlayerData : MonoBehaviour
{
    [SerializeField] StatuaData playerStatuaData;

    private int playerCurrentHp;
    public int PlayerCurrentHp
    {
        get
        {
            return playerCurrentHp;
        }
        set
        {
            playerCurrentHp = value;
        }
    }

    private bool playerDead;

    //liderのHPゲージ指定
    [SerializeField] Slider slider;

    [SerializeField] TextMeshProUGUI playerNameText;

    [SerializeField] TextMeshProUGUI playerHpText;

    [SerializeField] TextMeshProUGUI playerAtkText;

    [SerializeField] TextMeshProUGUI playerDefText;

    [SerializeField] TextMeshProUGUI playerLevelText;
    // Start is called before the first frame update
    void Start()
    {
        playerDead = false;
        playerNameText.text = playerStatuaData.NAME;
        playerCurrentHp = playerStatuaData.MAXHP;
        playerHpText.text = playerCurrentHp.ToString();
    }

    /// <summary>
    /// ダメージ分プレイヤーのスライダーを減少させる
    /// </summary>
    public void CurrentHpSlider()
    {
        slider.value = (float)playerCurrentHp / (float)playerStatuaData.MAXHP;
    }
}
