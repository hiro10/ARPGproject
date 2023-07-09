using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ゲームの制御用　役割変わるかも()

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    bool isPause;

    public string sceneName;
    public string nowSceneName;

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


    //public void OnButtonPressed()
    //{
    //    //if (context.started)
    //    {
    //        if (isPause == false)
    //        {
    //            isPause = true;
    //            Time.timeScale = 0f;
    //        }
    //        else
    //        {
    //            Time.timeScale = 1f;
    //            isPause = false;
    //        }
    //    }
    //}
}