using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSePlayer : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip footstepSound;
    [SerializeField]
    Transform footTransform;
    [SerializeField]
    float raycastDistance = 0.1f;
    [SerializeField]
    LayerMask groundLayers = 0;
    [SerializeField]
    float minFootstepInterval = 0.5f;
     [SerializeField]
    float maxFootstepInterval = 0.8f;

    bool timerIsActive = false;
    WaitForSeconds footstepWait;
    [SerializeField] PlayerController player;
    void Start()
    {
        footstepWait = new WaitForSeconds(minFootstepInterval);
    }

    private void LateUpdate()
    {
        if (player.isGrounded)
        {
            if (player.move.magnitude > 0)
            {
                
                CheckGroundStatus();
            }
        }
        else
        {
            timerIsActive = true;
        }
    }

    void CheckGroundStatus()
    {
        if (player.move.magnitude > 0.7f)
        {
            footstepWait = new WaitForSeconds(minFootstepInterval);
        }
        else if (player.move.magnitude <= 0.7f)
        {
            footstepWait = new WaitForSeconds(maxFootstepInterval);
        }
        if (timerIsActive)
        {
            return;
        }

        bool isGrounded = Physics.Raycast(footTransform.position, new Vector3(0,-0.1f,0), raycastDistance, groundLayers, QueryTriggerInteraction.Ignore);

        if (player.isGrounded)
        {
            PlayFootstepSound();
        }

        StartCoroutine(nameof(FootstepTimer));
        
    }

    void PlayFootstepSound()
    {
        audioSource.PlayOneShot(footstepSound);
    }

    IEnumerator FootstepTimer()
    {
        timerIsActive = true;

        yield return footstepWait;

        timerIsActive = false;
        yield return null;
    }
}