using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLineDrawer : MonoBehaviour
{
    public Terrain terrain;
    public Material lineMaterial;
    public float lineWidth = 0.1f;
    public int numberOfPoints = 10; // ���̕�����

    private LineRenderer lineRenderer;
    private Vector3[] linePoints;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        // �G���A�̒��_��ݒ�
        linePoints = new Vector3[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = i / (float)(numberOfPoints - 1); // t��0����1�ɐ��K��
            float x = Mathf.Lerp(10f, 20f, t); // x���W�����ɔz�u
            float z = Mathf.Lerp(10f, 20f, t); // z���W�����ɔz�u
            float y = terrain.SampleHeight(new Vector3(x, 0f, z)); // �e���C���̍������T���v�����O
            linePoints[i] = new Vector3(x, y, z);
        }

        // ���̒��_��ݒ�
        lineRenderer.positionCount = linePoints.Length;
        lineRenderer.SetPositions(linePoints);
    }
}
