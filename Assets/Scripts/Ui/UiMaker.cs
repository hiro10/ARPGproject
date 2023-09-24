using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// 目的地にアイコンを表示
/// </summary>
public class UiMaker : MonoBehaviour
{
    // オブジェクトを映すカメラ
    [SerializeField] private Camera mainCamera;

    // UIを表示させる対象
    [SerializeField] public List<Transform> targets = new List<Transform>();

    // 表示するUI
    [SerializeField] private GameObject targetUIPrefab;

    // オフセット
    [SerializeField] Vector3 worldOffset;

    public RectTransform parent;

    // インスタンス化したUIを格納するリスト
    private List<Transform> instantiatedUIs = new List<Transform>();

    
    // TextMeshProUGUIオブジェクト
    public List<TextMeshProUGUI> distanceText=new List<TextMeshProUGUI>();
    [SerializeField] Transform playerTransform;


    /// <summary>
    /// 開始処理
    /// </summary>
    private void Awake()
    {
        // nullチェック
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        // 親のTransformを取得
        //parent = targetUIPrefab.transform.parent.GetComponent<RectTransform>();
    }

    private void Start()
    {
        // ターゲット数分だけUIをインスタンス化してリストに追加
        for (int i = 0; i < targets.Count; i++)
        {
            GameObject uiInstance = Instantiate(targetUIPrefab, parent);
            distanceText.Add( uiInstance.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>());
            instantiatedUIs.Add(uiInstance.transform);
        }
    }


    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        CheckTargets();
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null)
            {
                // ターゲットが null になった場合、それをリストから取り除く
                targets.RemoveAt(i);
                i--; // リストの要素を削除したので、インデックスをデクリメントする
                continue;
            }
            OnUpdatePosition(i);
        }

    }

    private void CheckTargets()
    {
        // ターゲットが減少した場合にUIも削除
        if (targets.Count < instantiatedUIs.Count)
        {
            for (int i = targets.Count; i < instantiatedUIs.Count; i++)
            {
                Destroy(instantiatedUIs[i].gameObject);
            }
            instantiatedUIs.RemoveRange(targets.Count, instantiatedUIs.Count - targets.Count);
        }
    }


    private void OnUpdatePosition(int index)
    {
        UpdateDistanceText();

        var cameraTransform = mainCamera.transform;

        // カメラの向きベクトル
        var cameraDir = cameraTransform.forward;

        // オブジェクトの位置
        var targetworldPos = targets[index].position + worldOffset;
        // カメラからターゲットへのベクトル
        var targetDir = targetworldPos - cameraTransform.position;

        // 内積を使ってカメラ前方かどうか
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // カメラが前方ならUI表示、後ろなら非表示
        instantiatedUIs[index].gameObject.SetActive(isFront);
        if (!isFront)
        {
            return;
        }

        // オブジェクトのワールド座標からスクリーン座標の変換
        var targetScreenPos = mainCamera.WorldToScreenPoint(targetworldPos);

        // スクリーン座標変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (parent, targetScreenPos, null, out var uiLocalPos);

        instantiatedUIs[index].localPosition = uiLocalPos;
        
    }

    private void UpdateDistanceText()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            // ターゲットオブジェクトが設定されている場合のみ処理を行う
            if (targets[i] != null)
            {

                // プレイヤー（このスクリプトがアタッチされているオブジェクト）からターゲットオブジェクトまでの距離を計算
                float distance = Vector3.Distance(playerTransform.position, targets[i].position);

                // 距離をメートルに変換して整数に変換
                int distanceInMeters = Mathf.RoundToInt(distance);

                // TextMeshProUGUIに距離を表示
                distanceText[i].text = distanceInMeters.ToString() + " m"; // 小数点以下2桁で表示
            }
        }
    }
}
