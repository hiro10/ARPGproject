using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class LoadingScene : MonoBehaviour
{
    [SerializeField] private GameObject _loadingUI;
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _text;
    private AsyncOperation async;
    
    private void Start()
    {
        if(Time.timeScale!=1)
        {
            Time.timeScale = 1;
        }

        GameManager.Instance.nowSceneName = SceneManager.GetActiveScene().name;
        _loadingUI.SetActive(true);
        StartCoroutine(LoadScene());
        _text.text = "ì«Ç›çûÇ›íÜ";
    }
    IEnumerator LoadScene()
    {
        yield return null;
        if (GameManager.Instance.sceneName == "Title"
            || (GameManager.Instance.sceneName == "DemoScene"&&GameManager.Instance.isGameClear))
        {
            async = SceneManager.LoadSceneAsync("Village");
        }
        else if(GameManager.Instance.sceneName == "DemoScene"
            || GameManager.Instance.sceneName == "Test" 
            || GameManager.Instance.sceneName==""
            ||(GameManager.Instance.sceneName == "Village"
            && (GameManager.Instance.isGameOver == true)|| (GameManager.Instance.isGameClear == true)))
        {
            GameManager.Instance.isGameOver = false;
            GameManager.Instance.isGameClear = false;
            async = SceneManager.LoadSceneAsync("Title");
        }
        else if(GameManager.Instance.sceneName == "Village")
        {
            async = SceneManager.LoadSceneAsync("DemoScene");
        }
        
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            _slider.value = async.progress;
            if (async.progress >= 0.9f)
            {
                _text.text = "ì«Ç›çûÇ›äÆóπ";
                yield return new WaitForSeconds(0.3f);
                    async.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    private void OnDestroy()
    {

        GameManager.Instance.sceneName = SceneManager.GetActiveScene().name;

    }
}