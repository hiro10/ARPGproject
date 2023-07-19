using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadButtonInput : MonoBehaviour
{
    private InputAction buttonAction;
    public GameObject button;

    private void OnEnable()
    {
        // ゲームパッドのボタン入力アクションを取得
        buttonAction = new InputAction("ButtonAction", binding: "<Gamepad>/buttonSouth");
        buttonAction.Enable();

        // ボタンが押されたときのイベントハンドラを設定
        buttonAction.performed += OnButtonPressed;
    }

    private void OnDisable()
    {
        // イベントハンドラの解除とアクションの無効化
        buttonAction.performed -= OnButtonPressed;
        buttonAction.Disable();
    }

    public void OnButtonPressed(InputAction.CallbackContext context)
    {
        // ボタンが押されたときの処理をここに記述
        Debug.Log("Button Pressed");

        // UGUIのボタンを押す処理
        // 例えば、ボタンがアタッチされているオブジェクトに対してクリックイベントを発生させるなどの方法を使用します。
        // 以下は、UGUIのButtonコンポーネントがアタッチされているオブジェクトのクリックイベントを発生させる例です。
        button.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
    }
}