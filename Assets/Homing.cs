using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public Transform controlPoint; // �x�W�F�Ȑ��̐���_

    private float t = 0f;

    private void Update()
    {
        if (target == null)
        {
            // �^�[�Q�b�g���Ȃ��ꍇ�A�~�T�C����j��
            Destroy(gameObject);
            return;
        }

        // �x�W�F�Ȑ���̍��W���v�Z
        Vector3 p0 = transform.position;
        Vector3 p1 = controlPoint.position;
        Vector3 p2 = target.position;

        // t�̒l�𑝉�������
        t += Time.deltaTime * speed;

        // �x�W�F�Ȑ����v�Z
        Vector3 position = CalculateBezierPoint(p0, p1, p2, t);

        // �~�T�C���̈ړ�
        transform.position = position;
    }

    private Vector3 CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        // �x�W�F�Ȑ����v�Z���鎮
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 point = uu * p0; // p0 * (1-t)^2
        point += 2 * u * t * p1; // 2 * (1-t) * t * p1
        point += tt * p2; // t^2 * p2

        return point;
    }
}