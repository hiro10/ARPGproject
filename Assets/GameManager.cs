using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Q�[���̐���p�@�����ς�邩��()

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
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
}
