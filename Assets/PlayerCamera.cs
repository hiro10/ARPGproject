using UnityEngine;
using Cinemachine;

/// <summary>
/// CinemachineVirtualCameraを制御するクラス
/// </summary>
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera freeLookCamera;
    [SerializeField] CinemachineVirtualCamera lockonCameral;

    readonly int LockonCameraActivePriority = 11;
    readonly int LockonCameraInactivePriority = 0;

    /// <summary>
    /// カメラの角度をプレイヤーを基準にリセット
    /// </summary>
    public void ResetFreeLookCamera()
    {
        // 未実装
    }


    /// <summary>
    /// ロックオン時のVirtualCamera切り替え
    /// </summary>
    /// <param name="target"></param>
    public void ActiveLockonCamera(GameObject target)
    {
        lockonCameral.Priority = LockonCameraActivePriority;
        lockonCameral.LookAt = target.transform;
    }


    /// <summary>
    /// ロックオン解除時のVirtualCamera切り替え
    /// </summary>
    public void InactiveLockonCamera()
    {
        lockonCameral.Priority = LockonCameraInactivePriority;
        lockonCameral.LookAt = null;

        // 直前のLockonCameraの角度を引き継ぐ
        var pov = freeLookCamera.GetCinemachineComponent<CinemachinePOV>();
        pov.m_VerticalAxis.Value = Mathf.Repeat(lockonCameral.transform.eulerAngles.x + 180, 360) - 180;
        pov.m_HorizontalAxis.Value = lockonCameral.transform.eulerAngles.y;
    }

}