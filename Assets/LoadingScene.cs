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
    private void Start()
    {
        _loadingUI.SetActive(true);
        StartCoroutine(LoadScene());
        _text.text = "ì«Ç›çûÇ›íÜ";
    }
    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation async = SceneManager.LoadSceneAsync("DemoScene");
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
}