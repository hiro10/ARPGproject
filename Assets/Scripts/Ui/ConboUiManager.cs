using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ConboUiManager : MonoBehaviour
{
    // ���̂Ƃ���R���{�̉��Z��dotween�A�j���[�V�����̂�
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
        // �R���{�e�L�X�g�̕\��(setactive�ł������C������)
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
    /// �R���{�e�L�X�g�̓���
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
    /// �R���{�e�L�X�g�̌��̑傫���i�����_���p�j
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
