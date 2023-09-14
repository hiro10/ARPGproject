using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float curveHeight = 5f; // ベジェ曲線の高さを調整
    public float curveAmountX = 2f; // ベジェ曲線のx軸方向の曲がり具合を調整

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 controlPoint1;
    private Vector3 controlPoint2;

    private float t = 0f;

    private void Start()
    {
        startPos = transform.position;
        endPos = target.position;

        // ベジェ曲線の制御点を計算
        Vector3 direction = (endPos - startPos).normalized;
        float distance = Vector3.Distance(startPos, endPos);
        Vector3 midPoint = (startPos + endPos) / 2f;
        controlPoint1 = midPoint + Vector3.up * curveHeight + direction * curveAmountX;
        controlPoint2 = midPoint + Vector3.up * curveHeight - direction * curveAmountX;
    }

    private void Update()
    {
        t += Time.deltaTime * speed;

        if (t > 1f)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 currentPos = CalculateBezierPoint(startPos, controlPoint1, controlPoint2, endPos, t);
        transform.position = currentPos;
    }

    private Vector3 CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1f - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0; // (1-t)^3 * p0
        p += 3f * uu * t * p1; // 3 * t * (1-t)^2 * p1
        p += 3f * u * tt * p2; // 3 * t^2 * (1-t) * p2
        p += ttt * p3; // t^3 * p3

        return p;
    }
}