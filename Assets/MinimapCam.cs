using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ミニマップ用のカメラの位置
public class MinimapCam : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var pos = player.transform.position;
        pos.y = transform.position.y;
        transform.position = pos;
    }
}
