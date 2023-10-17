using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

// �{�X�V�[���ւ̑J�ڗp�N���X
public class LoadBoss : MonoBehaviour
{
    [SerializeField] private RenderTexture renderTexture;
    private Camera renderCamera;
    [SerializeField] GameObject playerLader;
    [SerializeField] BattleSceneManager sceneManager;

    void Start()
    {
        renderCamera = Camera.main.transform.GetChild(0).GetComponent<Camera>();    //�f�����e�p�̃J�������擾
    }

    public void OnClickButton()
    {
        LoadBossScene();
    }

    private async void LoadBossScene()
    {
        await TraceOn();
    }

    private async UniTask TraceOn()
    {
        playerLader.SetActive(false);
        renderCamera.enabled = true;
        renderCamera.targetTexture = renderTexture;
        await UniTask.Delay(500);
        renderCamera.targetTexture = null;
        await SceneManager.LoadSceneAsync("Test");                                 
    }
}
