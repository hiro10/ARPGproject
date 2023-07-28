using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMaker : MonoBehaviour
{
    // オブジェクトを映すカメラ
    [SerializeField] private Camera mainCamera;

    // UIを表示させる対象
    public Transform target;

    // 表示するUI
    [SerializeField] private Transform targetUI;

    // オフセット
    [SerializeField] Vector3 worldOffset;

    private RectTransform parent;

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
        parent = targetUI.parent.GetComponent<RectTransform>();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        OnUpdatePosition();
    }

    private void OnUpdatePosition()
    {
        var cameraTransform = mainCamera.transform;

        // カメラの向きベクトル
        var cameraDir = cameraTransform.forward;
        // オブジェクトの位置
        var targetworldPos = target.position + worldOffset;
        // カメラからターゲットへのベクトル
        var targetDir = targetworldPos - cameraTransform.position;

        // 内積を使ってカメラ前方かどうか
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // カメラが前方ならUI表示、後ろなら非表示
        targetUI.gameObject.SetActive(isFront);
        if(!isFront)
        {
            return;
        }

        // オブジェクトのワールド座標からスクリーン座標の変換
        var targetScreenPos = mainCamera.WorldToScreenPoint(targetworldPos);

        // スクリーン座標変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (parent, targetScreenPos, null, out var uiLocalPos);

        targetUI.localPosition = uiLocalPos;
    }

}
