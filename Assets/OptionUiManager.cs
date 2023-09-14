using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class OptionUiManager : MonoBehaviour
{
    // �I�v�V������ʗp(DoTween)
    [SerializeField] private GameObject optionPanel;
    [SerializeField] GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        // �I�v�V�����p�l����null�`�F�b�N�Ə����X�P�[���ݒ�
        if (optionPanel != null)
        {
            optionPanel.SetActive(false);
            //optionPanel.transform.localScale = Vector3.zero;
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// �`���[�g���A���E�C���h�E����
    /// </summary>
    public void OnClickStartOptionsButton(GameObject panels)
    {

        pauseMenu.SetActive(false);
        //SoundManager.instance.PlaySE(SoundManager.SE.Decision);
        panels.SetActive(true);
        // �I�v�V�����E�B���h�E�����񂾂�g��
        //panels.gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetLink(gameObject);
    }

    public void OnClickCloseButton(GameObject panels)
    {
        //SoundManager.instance.PlaySE(SoundManager.SE.Close);
        // backPanel.SetActive(false);
        ResetPanel();
        // �I�v�V�����E�B���h�E�����񂾂�g��
        //panels.gameObject.transform.DOScale(new Vector3(0f, 0f, 0f), 0.2f).SetLink(gameObject).OnComplete(ResetPanel);
        pauseMenu.SetActive(true);
    }

    private void ResetPanel()
    {
       
        optionPanel.SetActive(false);
       
    }

}
