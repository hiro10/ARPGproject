using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARPG.Dialogue;

public class ChengDialog : MonoBehaviour
{
    [SerializeField] AIConversant aIConversant;

    // 会話用ダイアログ（交換用）
    [SerializeField] Dialogue dialogue = null;

    // クリア会話用ダイアログ（交換用）
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
