using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// �Q�[���̐���p�@�����ς�邩��()

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    bool isPause;

    /// <summary>
    ///  �V���O���g����
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

    private void Start()
    {
        isPause = false;
    }

    public void OnButtonPressed()
    {
        //if (context.started)
        {
            if (isPause == false)
            {
                isPause = true;
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
                isPause = false;
            }
        }
    }
}