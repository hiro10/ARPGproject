using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Playables;
//using UnityEngine.Rendering.PostProcessing;

public class WarpConntroller : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    Animator animator;
    public Transform target;

    public float warpDuration = .5f;
    private CinemachineImpulseSource impulse;
    public CinemachineVirtualCamera camera;
    // ワープの発動判定
    public bool isWarp;

    [SerializeField] GameObject warpSlash;

    // 剣
    [SerializeField] Transform sword;

    // 剣の初期位置格納用
    private Vector3 swordOrigRot;
    private Vector3 swordOrigPos;

    // 剣の親(持ち手)
    public Transform swordHand;

    // マテリアル
    [Space]
    public Material glowMaterial;
    public Material endMaterial;
    //ThirdPersonMovement thirdPerson;

    [SerializeField] GameObject gameObjectcam;

    [Header("Particles")]
    public ParticleSystem blueTrail;
    public ParticleSystem whiteTrail;
    public ParticleSystem swordParticle;

    [SerializeField]CameraController controller;

   // private PostProcessVolume postVolume;
   // private PostProcessProfile postProfile;

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        impulse = camera.GetComponent<CinemachineImpulseSource>();
        warpSlash.SetActive(false);
        // thirdPerson = GetComponent<ThirdPersonMovement>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        // 剣の位置を記憶させる
        swordOrigRot = sword.localEulerAngles;
        swordOrigPos = sword.localPosition;
        //controller = GetComponent<CameraController>();
        // ワープの仕様判定を設定
        isWarp = false;

        gameObjectcam.SetActive(true);

        sword.gameObject.SetActive(false);
        //postVolume = Camera.main.GetComponent<PostProcessVolume>();
        //postProfile = postVolume.profile;
    }

    // Update is called once per frame
    void Update()
    {
        // ワープ後の剣のローテションがおかしくなる処理の応急処置
        if (sword.localEulerAngles != swordOrigRot)
        {
            sword.localEulerAngles = swordOrigRot;
        }

    }
    public void OnWarp(InputAction.CallbackContext context)
    {
        target = controller.rockonTarget.transform;
        //if (playerController.attack == false)
        {
            if (context.started)
            {
                if (target == null)
                {
                    return;
                }
                
                playerController.MoveOff();
                playerController.RotaionOff();
                playerController.AttackOn();

                sword.gameObject.SetActive(true);
                this.transform.LookAt(target.position);
                //animator.applyRootMotion = false;
                // gameObjectcam.SetActive(false);
                isWarp = true;
                swordParticle.Play();
                animator.SetTrigger("slash");
            }
        }
        
        
        
    }

/// <summary>
/// ワープ処理(Domove)
/// </summary>
public void Warp()
    {
        
        // 残像用
        // コンポーネントを外したクローンを表示
        GameObject clone = Instantiate(this.gameObject, transform.position, transform.rotation);
        Destroy(clone.GetComponent<WarpConntroller>().sword.gameObject);
        Destroy(clone.GetComponent<Animator>());
        Destroy(clone.GetComponent<PlayerController>());
        Destroy(clone.GetComponent<WarpConntroller>());
        Destroy(clone.GetComponent<Rigidbody>());
        Destroy(clone.GetComponent<BoxCollider>());

        SkinnedMeshRenderer[] skinMeshList = clone.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in skinMeshList)
        {
            smr.material = glowMaterial;
            smr.material.DOFloat(2, "_AlphaThreshold", 5f).OnComplete(() => Destroy(clone));
        }

        // 体を消す
        ShowBody(false);
        //thirdPerson.SetGravityOnTrigger(false);
        // アニメーションを止める
        animator.speed = 0f;

        Vector3 targetPos = new Vector3(target.position.x, target.position.y-1f , target.position.z);
        // シフトする際にレイを飛ばして当たった位置を取得して、その位置の手前にシフトする
        // ワープ処理：イーじんぐ処理後で理解
        transform.DOMove(targetPos, warpDuration).SetEase(Ease.InExpo).OnComplete(() => FinshWarp());

        // 親をnullにする
        sword.parent = null;
        sword.DOMove(target.position, warpDuration / 2);
        sword.DOLookAt(target.position, .2f, AxisConstraint.None);
        //sword.DORotate(new Vector3(0, 90, 0), 0.3f);

        //Particles
        blueTrail.Play();
        whiteTrail.Play();

        //Lens Distortion
        //DOVirtual.Float(0, -80, .2f, DistortionAmount);
        // DOVirtual.Float(1, 2f, .2f, ScaleAmount);
    }

    //void DistortionAmount(float x)
    //{
    //    postProfile.GetSetting<LensDistortion>().intensity.value = x;
    //}
    //void ScaleAmount(float x)
    //{
    //    postProfile.GetSetting<LensDistortion>().scale.value = x;
    //}


    /// <summary>
    /// ワープ中に姿を消す処理
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
    /// ワープ終了時の処理
    /// </summary>
    void FinshWarp()
    {
        ShowBody(true);
        // 剣の親と位置の再設定
        sword.parent = swordHand;
        sword.localPosition = swordOrigPos;
        sword.localEulerAngles = swordOrigRot;

        SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in skinMeshList)
        {
            GlowAmount(30);
            DOVirtual.Float(30, 0, .5f, GlowAmount);
        }
        target.DOMove(target.position + transform.forward,1f);

        animator.speed = 1f;
        warpSlash.SetActive(true);
        gameObjectcam.SetActive(true);

        

        StartCoroutine(StopParticles());

        sword.gameObject.SetActive(false);
       
         impulse.GenerateImpulse(Vector3.right);
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
        SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in skinMeshList)
        {
            smr.material = endMaterial;
            smr.material.SetVector("_FresnelAmount", new Vector4(x, x, x, x));
        }
    }

    /// <summary>
    /// パーティ黒を止める
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
