using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ミニマップ用のカメラの位置
public class MinimapCam : MonoBehaviour
{
    [SerializeField] private PlayerController player;
   
    void Update()
    {
        var pos = player.transform.position;
        pos.y = transform.position.y;
        transform.position = pos;
    }
}
