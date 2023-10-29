using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private GameObject _loadingUI;
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _text;
    private AsyncOperation async;
    
    private async void Start()
    {
        if(Time.timeScale!=1)
        {
            Time.timeScale = 1;
        }

        GameManager.Instance.nowSceneName = SceneManager.GetActiveScene().name;
        _loadingUI.SetActive(true);
        await LoadScene();
        _text.text = "�ǂݍ��ݒ�";
    }
    async UniTask LoadScene()
    {
        await UniTask.Yield(); // �R���[�`���̍ŏ���yield return null��Unitask�ɕϊ�

        if (GameManager.Instance.sceneName == "Title" || (GameManager.Instance.sceneName == "DemoScene" && GameManager.Instance.isGameClear))
        {
            async = SceneManager.LoadSceneAsync("Village");
        }
        else if (GameManager.Instance.sceneName == "DemoScene" || GameManager.Instance.sceneName == "Test" || GameManager.Instance.sceneName == "" || (GameManager.Instance.sceneName == "Village" && (GameManager.Instance.isGameOver == true) || (GameManager.Instance.isGameClear == true)))
        {
            GameManager.Instance.isGameOver = false;
            GameManager.Instance.isGameClear = false;
            GameManager.Instance.onTownName = false;
            async = SceneManager.LoadSceneAsync("Title");
        }
        else if (GameManager.Instance.sceneName == "Village")
        {
            async = SceneManager.LoadSceneAsync("DemoScene");
        }

        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            _slider.value = async.progress;
            if (async.progress >= 0.9f)
            {
                _text.text = "�ǂݍ��݊���";
                await UniTask.Delay(300); // �R���[�`����yield return new WaitForSeconds��Unitask�ɕϊ�
                async.allowSceneActivation = true;
            }
            await UniTask.Yield();
        }
    }

    private void OnDestroy()
    {

        GameManager.Instance.sceneName = SceneManager.GetActiveScene().name;

    }
}