using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARPG.Dialogue;

public class ChengDialog : MonoBehaviour
{
    [SerializeField] AIConversant aIConversant;

    // ��b�p�_�C�A���O�i�����p�j
    [SerializeField] Dialogue dialogue = null;

    // �N���A��b�p�_�C�A���O�i�����p�j
    [SerializeField] Dialogue dialogueClear = null;

    private void Start()
    {
        if(GameManager.Instance.isGameClear)
        {
            aIConversant.dialogue = dialogueClear;
        }
    }

    public void  Changelog()
    {
        aIConversant.dialogue = dialogue;
    }
}
