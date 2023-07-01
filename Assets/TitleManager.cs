using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    // �_�ŃX�s�[�h
    [SerializeField] private float speed = 1.0f;

    // �_�ł�������UI�i�[
    [SerializeField] private TextMeshProUGUI tapText;

    // �_�ŕp�x
    private float time;

    // �_�ŉ�����ɕ\��������UI
    [SerializeField] GameObject StartMenu;

    // ���j���[�{�^���i�[�p
    [SerializeField] Button[] MenmuButton = new Button[4];

    // �I�v�V������ʗp(DoTween)
    [SerializeField] private GameObject optionPanel;

    // �I�v�V������ʗp(DoTween)
    [SerializeField] private GameObject creditPanel;

    // �w�i�p�l���p
    [SerializeField] private GameObject backPanel;


    // Start is called before the first frame update
    void Start()
    {
        backPanel.SetActive(false);
        if (StartMenu != null)
        {
            StartMenu.SetActive(false);
        }
        // �I�v�V�����p�l����null�`�F�b�N�Ə����X�P�[���ݒ�
        if (optionPanel != null)
        {
            optionPanel.SetActive(false);
            optionPanel.transform.localScale = Vector3.zero;
        }
        else
        {
            return;
        }
        // �N���W�b�g�p�l����null�`�F�b�N�Ə����X�P�[���ݒ�
        if (creditPanel != null)
        {
            creditPanel.SetActive(false);
            creditPanel.transform.localScale = Vector3.zero;
        }
        else
        {
            return;
        }

    }

    // Update is called once per frame
    void Update()
    {
        tapText.color = GetAlphaColor(tapText.color);

        if (Input.GetMouseButton(0))
        {
            StartCoroutine(TapText());
        }
    }

    public void OnTitleButoon()
    {
        SceneManager.LoadSceneAsync("LoadScene");
    }

    /// <summary>
    /// Alpha�l���X�V����Color��Ԃ��_�ŏ���
    /// </summary>
    /// <param name="color"></param>
    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * 5.0f * speed;
        color.a = Mathf.Sin(time) * 0.5f + 0.5f;

        return color;
    }

    /// <summary>
    /// �u�^�b�v�v�����������̃R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator TapText()
    {
        // �_�ő��x�̏㏸
        speed = 4f;
        // 1s�҂�
        yield return new WaitForSeconds(1);
        // �^�b�v�I�u�W�F�N�g���\����
        tapText.gameObject.SetActive(false);
        // �X�^�[�g���j���[��\��
        StartMenu.SetActive(true);
    }

    public void OnPress(InputAction.CallbackContext context)
    {
        // �����ꂽ�u�Ԃ�Performed�ƂȂ�
        if (!context.performed)
        {
            StartCoroutine(TapText());
        }
    }


    /// <summary>
    /// �`���[�g���A���E�C���h�E����
    /// </summary>
    public void OnClickStartOptionsButton(GameObject panels)
    {
        backPanel.SetActive(true);
        //SoundManager.instance.PlaySE(SoundManager.SE.Decision);
        panels.SetActive(true);
        // �I�v�V�����E�B���h�E�����񂾂�g��
        panels.gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetLink(gameObject);
    }

    public void OnClickCloseButton(GameObject panels)
    {
        //SoundManager.instance.PlaySE(SoundManager.SE.Close);
        backPanel.SetActive(false);
        // �I�v�V�����E�B���h�E�����񂾂�g��
        panels.gameObject.transform.DOScale(new Vector3(0f, 0f, 0f), 0.2f).SetLink(gameObject).OnComplete(ResetPanel);

    }

    private void ResetPanel()
    {
        backPanel.SetActive(false);
        creditPanel.SetActive(false);
        optionPanel.SetActive(false);
    }
}
