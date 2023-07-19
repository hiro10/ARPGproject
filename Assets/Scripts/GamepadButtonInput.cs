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
        // �Q�[���p�b�h�̃{�^�����̓A�N�V�������擾
        buttonAction = new InputAction("ButtonAction", binding: "<Gamepad>/buttonSouth");
        buttonAction.Enable();

        // �{�^���������ꂽ�Ƃ��̃C�x���g�n���h����ݒ�
        buttonAction.performed += OnButtonPressed;
    }

    private void OnDisable()
    {
        // �C�x���g�n���h���̉����ƃA�N�V�����̖�����
        buttonAction.performed -= OnButtonPressed;
        buttonAction.Disable();
    }

    public void OnButtonPressed(InputAction.CallbackContext context)
    {
        // �{�^���������ꂽ�Ƃ��̏����������ɋL�q
        Debug.Log("Button Pressed");

        // UGUI�̃{�^������������
        // �Ⴆ�΁A�{�^�����A�^�b�`����Ă���I�u�W�F�N�g�ɑ΂��ăN���b�N�C�x���g�𔭐�������Ȃǂ̕��@���g�p���܂��B
        // �ȉ��́AUGUI��Button�R���|�[�l���g���A�^�b�`����Ă���I�u�W�F�N�g�̃N���b�N�C�x���g�𔭐��������ł��B
        button.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
    }
}