using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Q�[���I�����̏���
public class CloseGame : MonoBehaviour
{
    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
    }
}
