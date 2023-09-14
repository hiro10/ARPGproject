using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLineDrawer : MonoBehaviour
{
    public Terrain terrain;
    public Material lineMaterial;
    public float lineWidth = 0.1f;
    public int numberOfPoints = 10; // 線の分割数

    private LineRenderer lineRenderer;
    private Vector3[] linePoints;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        // エリアの頂点を設定
        linePoints = new Vector3[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = i / (float)(numberOfPoints - 1); // tを0から1に正規化
            float x = Mathf.Lerp(10f, 20f, t); // x座標を線上に配置
            float z = Mathf.Lerp(10f, 20f, t); // z座標を線上に配置
            float y = terrain.SampleHeight(new Vector3(x, 0f, z)); // テレインの高さをサンプリング
            linePoints[i] = new Vector3(x, y, z);
        }

        // 線の頂点を設定
        lineRenderer.positionCount = linePoints.Length;
        lineRenderer.SetPositions(linePoints);
    }
}
