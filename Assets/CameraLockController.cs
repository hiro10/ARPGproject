using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLockController : MonoBehaviour
{
    public Transform player;
    public Transform enemy;
    [SerializeField]private CinemachineVirtualCamera virtualCamera;
    private CinemachineComposer composer;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
    }

    private void Update()
    {
        if (player != null && enemy != null)
        {
            Vector3 midpoint = (player.position + enemy.position) / 2f;
            composer.m_TrackedObjectOffset = midpoint - virtualCamera.transform.position;
        }
    }
}
