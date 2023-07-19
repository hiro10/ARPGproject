using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//ポーズ機能
public class PauseManager : MonoBehaviour
{
    private bool isPause = false;
    [SerializeField] GameObject pausePanel;
    // Start is called before the first frame update
    void Start()
    {
        pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isPause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // ゲームの時間を停止
        isPause = true;
    }

    void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // ゲームの時間を通常に戻す
        isPause = false;
    }
}
