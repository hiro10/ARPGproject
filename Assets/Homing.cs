using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public Transform controlPoint; // ベジェ曲線の制御点

    private float t = 0f;

    private void Update()
    {
        if (target == null)
        {
            // ターゲットがない場合、ミサイルを破棄
            Destroy(gameObject);
            return;
        }

        // ベジェ曲線上の座標を計算
        Vector3 p0 = transform.position;
        Vector3 p1 = controlPoint.position;
        Vector3 p2 = target.position;

        // tの値を増加させる
        t += Time.deltaTime * speed;

        // ベジェ曲線を計算
        Vector3 position = CalculateBezierPoint(p0, p1, p2, t);

        // ミサイルの移動
        transform.position = position;
    }

    private Vector3 CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        // ベジェ曲線を計算する式
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 point = uu * p0; // p0 * (1-t)^2
        point += 2 * u * t * p1; // 2 * (1-t) * t * p1
        point += tt * p2; // t^2 * p2

        return point;
    }
}