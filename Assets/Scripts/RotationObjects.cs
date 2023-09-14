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
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerBoost;
    [Header("Prefabs")]
    public GameObject particle;

    // �J�����ɃA�^�b�`����Ă���Post Processing Volume
    [SerializeField]private Volume volume;

    // Vignette�̐ݒ�
    private Vignette vignette;

    // �ύX����Vignette�̐F
    public Color targetColor;
    public Color defaultColor;

    // Start is called before the first frame update
    void Start()
    {
        // Volume���擾
        volume = Camera.main.GetComponent<Volume>();
        // Volume����Vignette�̐ݒ���擾
        volume.profile.TryGet(out vignette);

        rotationObj = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if(volume==null)
        //{
        //    // Volume���擾
        //    volume = Camera.main.GetComponent<Volume>();
        //    // Volume����Vignette�̐ݒ���擾
        //    volume.profile.TryGet(out vignette);
        //}
    }

    public void OnROtationObj(InputAction.CallbackContext context)
    {
        Debug.Log("�����ꂽ");
        if (context.started)
        {
            Instantiate(particle, player.transform.position, Quaternion.identity);
            if (rotationObj == false)
            {
                // Vignette�̐F��ύX
                
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
    }
}