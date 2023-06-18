using UnityEngine;
using Cinemachine;

/// <summary>
/// CinemachineVirtualCamera�𐧌䂷��N���X
/// </summary>
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera freeLookCamera;
    [SerializeField] CinemachineVirtualCamera lockonCameral;

    readonly int LockonCameraActivePriority = 11;
    readonly int LockonCameraInactivePriority = 0;

    /// <summary>
    /// �J�����̊p�x���v���C���[����Ƀ��Z�b�g
    /// </summary>
    public void ResetFreeLookCamera()
    {
        // ������
    }


    /// <summary>
    /// ���b�N�I������VirtualCamera�؂�ւ�
    /// </summary>
    /// <param name="target"></param>
    public void ActiveLockonCamera(GameObject target)
    {
        lockonCameral.Priority = LockonCameraActivePriority;
        lockonCameral.LookAt = target.transform;
    }


    /// <summary>
    /// ���b�N�I����������VirtualCamera�؂�ւ�
    /// </summary>
    public void InactiveLockonCamera()
    {
        lockonCameral.Priority = LockonCameraInactivePriority;
        lockonCameral.LookAt = null;

        // ���O��LockonCamera�̊p�x�������p��
        var pov = freeLookCamera.GetCinemachineComponent<CinemachinePOV>();
        pov.m_VerticalAxis.Value = Mathf.Repeat(lockonCameral.transform.eulerAngles.x + 180, 360) - 180;
        pov.m_HorizontalAxis.Value = lockonCameral.transform.eulerAngles.y;
    }

}