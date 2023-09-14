using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

namespace ARPG.Dialogue.Editor
{
    // ダイアログエディター
    public class DialogueEditor : EditorWindow
    {

        Dialogue selectedDialogue = null;

        [NonReorderable] GUIStyle nodeStyle;

        [NonReorderable] GUIStyle playerNodeStyle;

        // 今ドラックしているノード
        [NonReorderable] DialogueNode draggingNode = null;

        // ドラック位置
        [NonReorderable] Vector2 draggingOffset;

        // 新しく作るノード
        [NonReorderable] DialogueNode creatingNode = null;
        
        // 削除するノード
        [NonReorderable] DialogueNode deletingNode = null;

        // ノードをリンクさせる
        [NonReorderable] DialogueNode linkingParentNode = null;

        // スクロール用
        Vector2 scrollPosition;
        [NonReorderable] bool draggingCanvas = false;
        [NonReorderable] Vector2 draggingCanvasOffset;

        const float canvasSize = 4000f;
        const float backgroundSize = 50f;

        // windowメニューから選択できるように
        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor"); 
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceId,int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceId) as Dialogue;
            if(dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 表示するノードUIの設定
        /// </summary>
        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;
            
            nodeStyle = new GUIStyle();

            nodeStyle.normal.background = EditorGUIUtility.Load("node0")as Texture2D;

            nodeStyle.normal.textColor = Color.white;

            nodeStyle.padding = new RectOffset(20, 20, 20, 20);

            nodeStyle.border = new RectOffset(12, 12, 12, 12);

            // プレイヤー用
            playerNodeStyle = new GUIStyle();

            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;

            playerNodeStyle.normal.textColor = Color.white;

            playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);

            playerNodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        private void OnSelectionChanged()
        {
            Dialogue newDialogue = Selection.activeObject as Dialogue;
            if(newDialogue != null)
            {
                selectedDialogue = newDialogue;
                Repaint();
            }
        }

        /// <summary>
        // 表示したエディターのUi設定
        /// </summary>
        private void OnGUI()
        {
            if(selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected");
            }
            else
            {
                ProcessEvents();

                // スクロール処理の開始
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                // スクロール範囲
                Rect canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);
                Texture2D backgroundTex = Resources.Load("background") as Texture2D;
                Rect texCoords = new Rect(0, 0, canvasSize/backgroundSize, canvasSize / backgroundSize);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTex,texCoords);

                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawConnection(node);
                }
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrowNode(node);
                }

                // スクロール処理の終了
                EditorGUILayout.EndScrollView();

                if (creatingNode != null)
                {
                    // 新しくノードの作成
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }
                if(deletingNode != null)
                {
                    // ノードの削除
                    selectedDialogue.DeleteNode(deletingNode);
                    deletingNode = null;
                }
            }         
        }

        /// <summary>
        /// ドラッグ判定
        /// </summary>
        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null) 
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                if(draggingNode != null)
                {
                    draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;
                }
                else
                {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                    Selection.activeObject = selectedDialogue;
                }
            }
            else if(Event.current.type == EventType.MouseDrag&& draggingNode!=null)
            {
                draggingNode.SetPosition( Event.current.mousePosition + draggingOffset);
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && draggingCanvas) 
            {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;
            }
            else if(Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                // ドラックしているノードはない状態にする
                draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }
        }

        /// <summary>
        /// ノードの表示処理
        /// </summary>
        /// <param name="node"></param>
        private void DrowNode(DialogueNode node)
        {
            GUIStyle style = nodeStyle;
            if(node.IsPlayerSpeaking())
            {
                style = playerNodeStyle;
            }
            GUILayout.BeginArea(node.GetRect(),style);
            EditorGUI.BeginChangeCheck();

            string newText = EditorGUILayout.TextField(node.GetText());

            // ノードのレイアウト(+,-ボタンを水平に)
            GUILayout.BeginHorizontal();

            if( GUILayout.Button("-"))
            {
                deletingNode = node;

            }
            DrawLinkButtons(node);
            if (GUILayout.Button("+"))
            {
                creatingNode = node;

            }

            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        /// <summary>
        /// リンクボタンの表示
        /// </summary>
        /// <param name="node"></param>
        void DrawLinkButtons(DialogueNode node)
        {
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("Link"))
                {
                    linkingParentNode = node;
                }
            }
            else if(linkingParentNode == node)
            {
                if (GUILayout.Button("Cancel"))
                {
                    linkingParentNode = null;
                }
            }
            else if(linkingParentNode.GetChildren().Contains(node.name))
            {
                if (GUILayout.Button("unLink"))
                { 
                    linkingParentNode.RemoveChild(node.name);
                    linkingParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("child"))
                {
                    linkingParentNode.AddChild(node.name);
                    linkingParentNode = null;
                }
            }
        }
        /// <summary>
        /// ノードをつなげる線の処理
        /// </summary>
        /// <param name="node"></param>
        private void DrawConnection(DialogueNode node)
        {
            Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPosition = new Vector2(childNode.GetRect().xMin,childNode.GetRect().center.y);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;
                Handles.DrawBezier(startPosition, endPosition,startPosition + controlPointOffset, 
                    endPosition - controlPointOffset,Color.white, null, 4f);
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;
            foreach(DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if(node.GetRect().Contains(point))
                {
                    foundNode = node;
                }
            }
            return foundNode;
        }
    }
}