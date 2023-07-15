using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;

// ワープ攻撃処理

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


    [SerializeField] GameObject gameObjectcam;

    [Header("Particles")]
    public ParticleSystem blueTrail;
    public ParticleSystem whiteTrail;
    public ParticleSystem swordParticle;

    [SerializeField]PlayerLockOn controller;
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
    Rigidbody rigidbody;
    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        rigidbody=GetComponent<Rigidbody>();
        impulse = camera.GetComponent<CinemachineImpulseSource>();
        warpSlash.SetActive(false);
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        // 剣の位置を記憶させる
        swordOrigRot = sword.localEulerAngles;
        swordOrigPos = sword.localPosition;
        // ワープの仕様判定を設定
        isWarp = false;

        gameObjectcam.SetActive(true);

        sword.gameObject.SetActive(false);
        //postVolume = Camera.main.GetComponent<PostProcessVolume>();
        //postProfile = postVolume.profile;
        player = GameObject.FindGameObjectWithTag("Player");
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
        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y+1.5f, transform.position.z), transform.forward);
        RaycastHit hit;
        // エネミーをロックしている場合
        if(controller.target!=null)
        {
            //target = controller.target.transform;
            targetPos = controller.target.transform.position;
        }
        // レイキャストで特定のタグのオブジェクトとの当たり判定を行う
        else if (Physics.Raycast(ray, out hit, rayLength) && hit.collider.tag == targetTag)
        {
            // レイが当たった位置を取得する
            Vector3 hitPoint = hit.point;

            // 取得した位置を利用する（例えば、何か処理を行う）
            targetPos = hitPoint;
        }
        else
        {
            // レイが当たらなかった場合、デフォルトの位置を取得する
            Vector3 defaultPoint = ray.GetPoint(rayLength);


            targetPos = defaultPoint;
        }
    
      
        if (playerController.attack == false)
        {
            if (context.started && !isWarp)
            {
                
                if (target == null)
                {
                    Transform playerTransform = player.transform;
                    Vector3 targetPosition = playerTransform.position + playerTransform.forward * 100f;
                    target = playerTransform;
                }
                playerController.MoveOff();
                playerController.RotaionOff();
                playerController.AttackOn();

                sword.gameObject.SetActive(true);
                // プレイヤーの回転を保存
                Quaternion savedRotation = transform.rotation;
               
                this.transform.LookAt(targetPos);
                

                // x軸回転を元に戻す
                Vector3 eulerRotation = transform.rotation.eulerAngles;
                eulerRotation.x = savedRotation.eulerAngles.x;
                transform.rotation = Quaternion.Euler(eulerRotation);
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
        Destroy(clone.GetComponent<PlayerInput>());
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

        // 体を消す
        ShowBody(false);
        //thirdPerson.SetGravityOnTrigger(false);
        // アニメーションを止める
        animator.speed = 0f;

        // Vector3 targetPos = new Vector3(target.position.x, target.position.y-0.5f , target.position.z);
        
        // シフトする際にレイを飛ばして当たった位置を取得して、その位置の手前にシフトする
        // ワープ処理：イーじんぐ処理後で理解
        transform.DOMove(targetPos, warpDuration).SetEase(Ease.InExpo).OnComplete(() => FinshWarp());

        // 親をnullにする
        SoundManager.instance.PlaySE(SoundManager.SE.ShiftThrow);
        sword.parent = null;
        sword.DOMove(targetPos, warpDuration / 2);
        sword.DOLookAt(target.position, .2f, AxisConstraint.None);

        //Particles
        blueTrail.Play();
        whiteTrail.Play();
        Time.timeScale = 0.8f;
        //Lens Distortion
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
    /// パーティクルを止める
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
