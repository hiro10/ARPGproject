using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//TODO Unitask化
public class LoadBoss : MonoBehaviour
{
    [SerializeField] private RenderTexture renderTexture;
    private Camera renderCamera;
    [SerializeField] GameObject playerLader;
    [SerializeField] BattleSceneManager sceneManager;
    // Start is called before the first frame update
    void Start()
    {
        renderCamera = Camera.main.transform.GetChild(0).GetComponent<Camera>();    //映像投影用のカメラを取得
    }

    // Update is called once per frame
    void Update()
    {// 画面割れを始めたいタイミングに置く
        
    }
    public void OnClickButton()
    {
        StartCoroutine("TraceOn");
    }
    public IEnumerator TraceOn()
    {
        playerLader.SetActive(false);
        renderCamera.enabled = true;
        renderCamera.targetTexture = renderTexture;                                 // 投影、開始
        yield return null;                                                          // 1f待ってもらって映像をレンダーテクスチャに投影する
        renderCamera.targetTexture = null;
        SceneManager.LoadSceneAsync("Test");                                 // シーンを遷移する
    }
}
