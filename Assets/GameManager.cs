using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲームの制御用　役割変わるかも()

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    /// <summary>
    ///  シングルトン化
    /// </summary>
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
