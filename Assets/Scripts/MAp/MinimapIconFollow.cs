using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIconFollow : MonoBehaviour
{
    [SerializeField] Transform player; // プレイヤーオブジェクトのTransformコンポーネントをアタッチ

    void Update()
    {
        // プレイヤーのy軸位置を無視し、x軸とz軸を追従
        Vector3 newPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.position = newPosition;
    }
}
