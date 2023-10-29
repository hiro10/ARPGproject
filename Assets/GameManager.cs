using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// ゲームの制御用　役割変わるかも()

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string sceneName;
    public string nowSceneName;

    // ムービー中判定
    public bool isMovePlaying;

    // ポーズ中か?
    public bool isPause;

    // ゲームオーバーしたか?
    public bool isGameOver = false;

    // ゲームクリアしたか?
    public bool isGameClear = false;

    // 村の名前を教えてくれるNpcと会話したかの判定
    public bool onTownName = false; 

    /// <summary>
    ///  シングルトン化
    /// </summary>
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        isPause = false;
    }

}