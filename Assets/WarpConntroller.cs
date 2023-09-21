using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;

// ���[�v�U������

public class WarpConntroller : MonoBehaviour
{
    // �v���C���[�R���g���[��
    [SerializeField] PlayerController playerController;
    // �A�j���[�^�[
    Animator animator;
    // ���[�v����ꏊ
    public Transform target;
    // ���[�v����
    public float warpDuration = .5f;
    private CinemachineImpulseSource impulse;

    // ���[�v�̔�������
    public bool isWarp;

    // ���[�v�U���̎��̃G�t�F�N�g
    [SerializeField] GameObject warpSlash;

    // ���I�u�W�F�N�g
    [SerializeField] Transform sword;

    // ���̏����ʒu�i�[�p
    private Vector3 swordOrigRot;
    private Vector3 swordOrigPos;

    // ���̐e(������)
    public Transform swordHand;

    // �}�e���A��
    [Space]
    public Material glowMaterial;
    public Material endMaterial;


    [SerializeField] GameObject gameObjectcam;

    // �e�G�t�F�N�g�p�p�[�e�B�N��
    [Header("Particles")]
    public ParticleSystem blueTrail;
    public ParticleSystem whiteTrail;
    public ParticleSystem swordParticle;

    [SerializeField] PlayerLockOn controller;
    [Header("Prefabs")]
    public GameObject hitParticle;

    private PostProcessVolume postVolume;
    private PostProcessProfile postProfile;
    GameObject player;

    public SkinnedMeshRenderer[] skinMeshList;

    public string targetTag = "Ground";
    public float rayLength = 10f;
    public Vector3 defaultPosition = Vector3.zero;
    Vector3 targetPos;
    [SerializeField] GameObject laderIcon;
    [SerializeField] GameObject laderCam;

    Rigidbody rigidbody;
    /// <summary>
    /// �J�n����
    /// </summary>
    void Start()
    {
        // �e�f�[�^�̏�����
        warpSlash.SetActive(false);
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        // ���̈ʒu���L��������
        swordOrigRot = sword.localEulerAngles;
        swordOrigPos = sword.localPosition;
        // ���[�v�̎d�l�����ݒ�
        isWarp = false;

        gameObjectcam.SetActive(true);
        rigidbody = GetComponent<Rigidbody>();
        sword.gameObject.SetActive(false);
        //postVolume = Camera.main.GetComponent<PostProcessVolume>();
        //postProfile = postVolume.profile;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    void Update()
    {

        // ���[�v��̌��̃��[�e�V���������������Ȃ鏈���̉��}���u
        if (sword.localEulerAngles != swordOrigRot)
        {
            sword.localEulerAngles = swordOrigRot;
        }

    }

    public void OnWarp(InputAction.CallbackContext context)
    {
        WarpPointChack();
        if (playerController.attack == false&&playerController.avoid==false)
        {
            if (context.started && !isWarp)
            {
                WarpStart();
            }
        }
    }

    /// <summary>
    /// ���[�v�J�n���̏���
    /// </summary>
    private void WarpStart()
    {
        isWarp = true;
      
        // ���[�v���͂�����͂��O��
        rigidbody.velocity = Vector3.zero;

        // plyercontroller�̊e������X�V
        playerController.MoveOff();
        playerController.RotaionOff();
        playerController.AttackOn();

        sword.gameObject.SetActive(true);
        // �v���C���[�̉�]��ۑ�
        Quaternion savedRotation = transform.rotation;

        // �^�[�Q�b�g�̕���������
        transform.LookAt(targetPos);

        Vector3 eulerRotation = transform.rotation.eulerAngles;
        eulerRotation.x = savedRotation.eulerAngles.x;
        transform.rotation = Quaternion.Euler(eulerRotation);

        swordParticle.Play();
        animator.SetTrigger("slash");
    }

    /// <summary>
    /// ���[�v�ʒu�̃`�F�b�N
    /// </summary>
    private void WarpPointChack()
    {
        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.forward);
        RaycastHit hit;
        // �G�l�~�[�����b�N���Ă���ꍇ
        if (controller.target != null)
        {
            // �^�[�Q�b�g�ʒu���G�l�~�[��
            targetPos = controller.target.transform.position;
        }
        // ���C�L���X�g�œ���̃^�O�̃I�u�W�F�N�g�Ƃ̓����蔻����s��
        else if (Physics.Raycast(ray, out hit, rayLength) && hit.collider.tag == targetTag)
        {
            // ���C�����������ʒu���擾
            Vector3 hitPoint = hit.point;

            // ������O�̈ʒu���v�Z
            Vector3 offsetPosition = hitPoint - (transform.forward.normalized * 0.5f);

            // �擾�����ʒu���^�[�Q�b�g�ʒu�Ƃ���
            targetPos = offsetPosition;
        }
        else
        {
            // ���C��������Ȃ������ꍇ�A���C�̍ő�l���擾
            Vector3 defaultPoint = ray.GetPoint(rayLength);

            targetPos = defaultPoint;
        }

    }

    /// <summary>
    /// ���[�v����(Domove)
    /// </summary>
    public void Warp()
    {
        // �c���p
        // �R���|�[�l���g���O�����N���[����\��
        GameObject clone = Instantiate(this.gameObject, transform.position, transform.rotation);
        Destroy(clone.GetComponent<WarpConntroller>().sword.gameObject);
        Destroy(clone.GetComponent<Animator>());
        Destroy(clone.GetComponent<PlayerController>());
        Destroy(clone.GetComponent<PlayerInput>());
        Destroy(clone.GetComponent<WarpConntroller>().laderIcon);
        Destroy(clone.GetComponent<WarpConntroller>().laderCam);
        Destroy(clone.GetComponent<WarpConntroller>());
        Destroy(clone.GetComponent<Rigidbody>());
        Destroy(clone.GetComponent<BoxCollider>());
        Destroy(clone.GetComponent<AudioFadeController>());
        

        SkinnedMeshRenderer[] skinMeshList = clone.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in skinMeshList)
        {
            smr.material = glowMaterial;
            smr.material.DOFloat(2, "_AlphaThreshold", 5f).OnComplete(() => Destroy(clone));
        }

        // �̂�����
        ShowBody(false);
      
        // �A�j���[�V�������~�߂�
        animator.speed = 0f;

        // �V�t�g����ۂɃ��C���΂��ē��������ʒu���擾���āA���̈ʒu�̎�O�ɃV�t�g����
        // ���[�v�����F�C�[���񂮏�����ŗ���
        transform.DOMove(targetPos, warpDuration).SetEase(Ease.InExpo).OnComplete(() => FinshWarp());

        // �e��null�ɂ���
        SoundManager.instance.PlaySE(SoundManager.SE.ShiftThrow);
        sword.parent = null;
        sword.DOMove(targetPos, warpDuration / 2);
        sword.DOLookAt(targetPos, .2f, AxisConstraint.None);

        // �e�p�[�e�B�N��
        blueTrail.Play();
        whiteTrail.Play();
        
        Time.timeScale = 0.8f;
       
        DOVirtual.Float(0, -80, .2f, DistortionAmount);
        DOVirtual.Float(1, 2f, .2f, ScaleAmount);
    }

    void DistortionAmount(float x)
    {
        postProfile.GetSetting<LensDistortion>().intensity.value = x;
    }
    void ScaleAmount(float x)
    {
        postProfile.GetSetting<LensDistortion>().scale.value = x;
    }


    /// <summary>
    /// ���[�v���Ɏp����������
    /// </summary>
    /// <param name="state"></param>
    private void ShowBody(bool state)
    {
        SkinnedMeshRenderer[] skinnedList = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer smr in skinnedList)
        {
            smr.enabled = state;
        }
    }

    /// <summary>
    /// ���[�v�I�����̏���
    /// </summary>
    void FinshWarp()
    {
        ShowBody(true);
        // ���̐e�ƈʒu�̍Đݒ�
        sword.parent = swordHand;
        sword.localPosition = swordOrigPos;
        sword.localEulerAngles = swordOrigRot;

        SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in skinMeshList)
        {
            GlowAmount(30);
            DOVirtual.Float(30, 0, .5f, GlowAmount);
        }
        Time.timeScale = 1f;
        Instantiate(hitParticle, sword.position, Quaternion.identity);

        gameObjectcam.SetActive(true);



        StartCoroutine(StopParticles());
        animator.speed = 1f;
        sword.gameObject.SetActive(false);

        if (controller.target != null)
        {
            SoundManager.instance.PlaySE(SoundManager.SE.Close);
            target.DOMove(targetPos + transform.forward, .1f);

            warpSlash.SetActive(true);
            impulse.GenerateImpulse(Vector3.right);
        }
        else
        {
            SoundManager.instance.PlaySE(SoundManager.SE.RotOff);
        }
    }

    public void WrapAnimationEnd()
    {
        isWarp = false;
        playerController.AttackOff();
        playerController.MoveOn();
        playerController.RotaionOn();
    }

    void GlowAmount(float x)
    {

        foreach (SkinnedMeshRenderer smr in skinMeshList)
        {
            smr.material = endMaterial;
            smr.material.SetVector("_FresnelAmount", new Vector4(x, x, x, x));
        }
    }

    /// <summary>
    /// �p�[�e�B�N�����~�߂�
    /// </summary>
    /// <returns></returns>
    IEnumerator StopParticles()
    {
        yield return new WaitForSeconds(.2f);
        warpSlash.SetActive(false);
        blueTrail.Stop();
        whiteTrail.Stop();
    }

}
