using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// �^�C�g���{�^���̏���
    /// ���[�h�V�[��������Ń^�C�g���ɖ߂�
    /// </summary>
    public void  OnClickReturnButton()
    {
      //  if(GameManager.Instance.sceneName!= GameManager.Instance.nowSceneName)
        {
            SceneManager.LoadSceneAsync("LoadScene");
        }
    }
    /// <summary>
    /// ���g���C�{�^���̏���
    /// �V�[���������P�x�s��
    /// </summary>
    public void OnClickRetryButton()
    {
        //if (GameManager.Instance.sceneName != GameManager.Instance.nowSceneName)
        {
            SceneManager.LoadSceneAsync(GameManager.Instance.nowSceneName);
        }
    }
}
