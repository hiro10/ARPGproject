using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void  OnClickReturnButton()
    {
        if(GameManager.Instance.sceneName!= GameManager.Instance.nowSceneName)
        {
            SceneManager.LoadSceneAsync("LoadScene");
        }
    }

    public void OnClickRetryButton()
    {
        if (GameManager.Instance.sceneName != GameManager.Instance.nowSceneName)
        {
            SceneManager.LoadSceneAsync("DemoScene");
        }
    }
}
