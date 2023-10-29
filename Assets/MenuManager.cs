using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// タイトルボタンの処理
    /// ロードシーンを挟んでタイトルに戻る
    /// </summary>
    public void  OnClickReturnButton()
    {
      //  if(GameManager.Instance.sceneName!= GameManager.Instance.nowSceneName)
        {
            SceneManager.LoadSceneAsync("LoadScene");
        }
    }
    /// <summary>
    /// リトライボタンの処理
    /// シーンをもう１度行う
    /// </summary>
    public void OnClickRetryButton()
    {
        //if (GameManager.Instance.sceneName != GameManager.Instance.nowSceneName)
        {
            if(GameManager.Instance.isGameClear)
            {
                GameManager.Instance.isGameClear = false;
            }
            SceneManager.LoadSceneAsync(GameManager.Instance.nowSceneName);
        }
    }
}
