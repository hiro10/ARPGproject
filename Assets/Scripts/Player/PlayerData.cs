using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
public class PlayerData : MonoBehaviour
{
    [SerializeField] StatuaData playerStatuaData;
    [SerializeField] PlayerController player;
    private int playerCurrentHp;
    private int playerMaxHp;
    private float playerCurrentMp;
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
    public int PlayerMaxHp
    {
        get
        {
            return playerMaxHp;
        }
    }

    public float PlayerCurrentMp
    {
        get
        {
            return playerCurrentMp;
        }
        set
        {
            playerCurrentMp = value;
        }
    }
    //覚醒ゲージ
    private float playerCurrentAwake;
    public float PlayerCurrentAwake
    {
        get
        {
            return playerCurrentAwake;
        }
        set
        {
            playerCurrentAwake = value;
        }
    }

    private bool playerDead;

    //liderのHPゲージ指定
    [SerializeField] Slider hpSlider;
    //liderのMPゲージ指定
    [SerializeField] Slider mpSlider;
    //liderの覚醒ゲージ指定
    [SerializeField] Slider awakeSlider;

    [SerializeField] TextMeshProUGUI playerNameText;

    [SerializeField] TextMeshProUGUI playerHpText;

    [SerializeField] TextMeshProUGUI playerAtkText;

    [SerializeField] TextMeshProUGUI playerDefText;

    [SerializeField] TextMeshProUGUI playerLevelText;

    [SerializeField] TextMeshProUGUI playerAwakeGaugeText;
    // Start is called before the first frame update
    void Start()
    {
        playerDead = false;
        playerNameText.text = playerStatuaData.NAME;
        playerCurrentHp = playerStatuaData.MAXHP;
        playerMaxHp = playerStatuaData.MAXHP;
        playerCurrentMp = playerStatuaData.MAXMP;
        playerHpText.text = playerCurrentHp.ToString();
        playerCurrentAwake = 0;
    }
    private void LateUpdate()
    {
        if (playerCurrentMp < playerStatuaData.MAXMP)
        {
            CurrentMpSlider();
            playerCurrentMp+=2f*Time.deltaTime;
        }
        if (playerCurrentAwake <= 100)
        {
            CurrentAwakeSlider();
        }
    }

    /// <summary>
    /// ダメージ分プレイヤーのスライダーを減少させる
    /// </summary>
    public void CurrentHpSlider()
    {
        hpSlider.value = (float)playerCurrentHp / (float)playerStatuaData.MAXHP;
    }

    /// <summary>
    /// プレイヤーのMpスライダーを反映させる
    /// </summary>
    public void CurrentMpSlider()
    {
        mpSlider.value = (float)playerCurrentMp / (float)playerStatuaData.MAXMP;
    }

    /// <summary>
    /// プレイヤーの覚醒スライダーを反映させる
    /// </summary>
    public void CurrentAwakeSlider()
    {
        awakeSlider.value = (float)playerCurrentAwake / 100f;
        playerAwakeGaugeText.text= (int)playerCurrentAwake+"/ 100";
    }
}
