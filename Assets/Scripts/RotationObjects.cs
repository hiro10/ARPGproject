using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// ��]�I�u�W�F�N�g�̏����i�����I�ɂ͊o���@�\�̏����j
/// </summary>
public class RotationObjects : MonoBehaviour
{
    [SerializeField] List<GameObject> rotObjects;
    [SerializeField]bool rotationObj;
    [SerializeField] Transform EffectGeneratePos;
    [SerializeField] GameObject playerBoost;
    [SerializeField] GameObject player;
    [Header("Prefabs")]
    public GameObject particle;

    // �J�����ɃA�^�b�`����Ă���Post Processing Volume
    [SerializeField]private Volume volume;

    // Vignette�̐ݒ�
    private Vignette vignette;

    // �ύX����Vignette�̐F
    public Color targetColor;
    public Color defaultColor;

   
    void Start()
    {
        // Volume���擾
        volume = Camera.main.GetComponent<Volume>();
        // Volume����Vignette�̐ݒ���擾
        volume.profile.TryGet(out vignette);

        rotationObj = false;
    }
   
    public void OnROtationObj(InputAction.CallbackContext context)
    {
        Debug.Log("�����ꂽ");
        if (context.started&& player.GetComponent<PlayerData>().PlayerCurrentAwake>=100)
        {
           
            if (rotationObj == false)
            {
                // �����G�t�F�N�g�̍Đ�
                Instantiate(particle, EffectGeneratePos.transform.position, Quaternion.identity);
                // Vignette�̐F��ύX
                // �v���C���[�̔�����o����Ԃ�
                player.GetComponent<PlayerController>().IsAwakening = true;
                playerBoost.SetActive(true);
                SoundManager.instance.PlaySE(SoundManager.SE.RotOn);
                rotationObj = true;
                vignette.color.Override(targetColor);
                vignette.smoothness.Override(1f);
                for (int i = 0; i < rotObjects.Count; i++)
                {
                    rotObjects[i].GetComponent<RotateUnit>().OnRoitationWepons();
                }


            }
            else if (rotationObj == true)
            {
                OffRotationObj();
            }
        }
    }

    public void OffRotationObj()
    {
        // �����G�t�F�N�g�̍Đ�
        Instantiate(particle, EffectGeneratePos.transform.position, Quaternion.identity);
        // �v���C���[�̔�����o����Ԃ�
        player.GetComponent<PlayerController>().IsAwakening = false;
        playerBoost.SetActive(false);
        SoundManager.instance.PlaySE(SoundManager.SE.RotOff);
        rotationObj = false;
        vignette.color.Override(defaultColor);
        vignette.smoothness.Override(0.35f);
        for (int i = 0; i < rotObjects.Count; i++)
        {
            rotObjects[i].GetComponent<RotateUnit>().OffRoitationWepons();
        }
    }
}