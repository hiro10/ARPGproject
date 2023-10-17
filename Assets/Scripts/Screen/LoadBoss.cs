using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

// ボスシーンへの遷移用クラス
public class LoadBoss : MonoBehaviour
{
    [SerializeField] private RenderTexture renderTexture;
    private Camera renderCamera;
    [SerializeField] GameObject playerLader;
    [SerializeField] BattleSceneManager sceneManager;

    void Start()
    {
        renderCamera = Camera.main.transform.GetChild(0).GetComponent<Camera>();    //映像投影用のカメラを取得
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
