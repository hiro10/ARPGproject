using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardHpGauge : MonoBehaviour
{
    void LateUpdate()
    {
        // ��]���J�����Ɠ���������
        transform.rotation = Camera.main.transform.rotation;
    }
}
