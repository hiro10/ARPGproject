using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ConboUiManager : MonoBehaviour
{
    // 今のところコンボの加算とdotweenアニメーションのみ
    private float step_time;
    private int combo;
    [SerializeField] private GameObject comboTextObject;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI comboText2;

    private void Start()
    {

        comboTextObject.SetActive(false);
        combo = 0;
        step_time = 0f;
    }
    private void Update()
    {
        step_time += Time.deltaTime;
        // コンボテキストの表示(setactiveでもいい気がする)
        if (step_time > 5f)
        {
            combo = 0;
            DOTween.ToAlpha(() => comboText.color, color => comboText.color = color, 0f, 0.1f).OnComplete(() => NonActiveComboText());
            DOTween.ToAlpha(() => comboText2.color, color => comboText2.color = color, 0f, 0.1f).OnComplete(() => NonActiveComboText());
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// コンボテキストの動き
    /// </summary>
    public void ComboAnim()
    {
        comboText2.color = new Color32(255, 255, 255, 255);
        comboText.color = new Color32(255, 255, 255, 255);
        step_time = 0f;
        if (!comboTextObject.activeSelf)
        {
            comboTextObject.SetActive(true);
        }
        combo++;
        comboText.text = combo.ToString();
        comboText.transform.DOPunchScale(new Vector3(1.05f, 1.05f, 1.05f), 0.1f).OnComplete(() => BaseScale());
    }
    /// <summary>
    /// コンボテキストの元の大きさ（ラムダ式用）
    /// </summary>
    private void BaseScale()
    {
        comboText.transform.localScale = Vector3.one;
    }

    private void NonActiveComboText()
    {
        comboTextObject.SetActive(false);
        comboText2.color = new Color32(255, 255, 255, 255);
        comboText.color = new Color32(255, 255, 255, 255);
    }
}
