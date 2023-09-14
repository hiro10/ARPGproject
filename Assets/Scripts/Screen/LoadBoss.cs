using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//TODO Unitask��
public class LoadBoss : MonoBehaviour
{
    [SerializeField] private RenderTexture renderTexture;
    private Camera renderCamera;
    [SerializeField] GameObject playerLader;
    // Start is called before the first frame update
    void Start()
    {
        renderCamera = Camera.main.transform.GetChild(0).GetComponent<Camera>();    //�f�����e�p�̃J�������擾
    }

    // Update is called once per frame
    void Update()
    {// ��ʊ�����n�߂����^�C�~���O�ɒu��
    }
    public void OnClickButton()
    {
        StartCoroutine("TraceOn");
    }
    public IEnumerator TraceOn()
    {
        playerLader.SetActive(false);
        renderCamera.enabled = true;
        renderCamera.targetTexture = renderTexture;                                 // ���e�A�J�n
        yield return null;                                                          // 1f�҂��Ă�����ĉf���������_�[�e�N�X�`���ɓ��e����
        renderCamera.targetTexture = null;
        SceneManager.LoadSceneAsync("Test");                                 // �V�[����J�ڂ���
    }
}
