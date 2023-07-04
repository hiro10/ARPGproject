using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GetSceneName : MonoBehaviour
{
    void Awake()
    {
        GameManager.Instance.nowSceneName = SceneManager.GetActiveScene().name;
    }

    private void OnDestroy()
    {
        GameManager.Instance.sceneName = SceneManager.GetActiveScene().name;
    }
}
