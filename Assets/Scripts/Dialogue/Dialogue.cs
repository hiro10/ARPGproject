using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ARPG.Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "New Dialogue", order = 0)]
    public class Dialogue : ScriptableObject,ISerializationCallbackReceiver
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();

        [SerializeField] Vector2 newNodeOffset = new Vector2(250, 0);

        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

        void Awake()
        {
            OnValidate();
        }
        private void OnValidate()
        {
            nodeLookup.Clear();
            foreach (DialogueNode node in GetAllNodes())
            {
                nodeLookup[node.name] = node;
            }
        }

        /// <summary>
        /// すべてのノードの取得用関数
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        /// <summary>
        /// すべての子ノードの取得
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {

            foreach (string childID in parentNode.GetChildren())
            {
                if (nodeLookup.ContainsKey(childID))
                {
                    yield return nodeLookup[childID];
                }
            }

        }
        public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode currentNode)
        {
            foreach (DialogueNode node in GetAllChildren(currentNode))
            {
                if (node.IsPlayerSpeaking())
                {
                    yield return node;
                }
            }
        }


        public IEnumerable<DialogueNode> GetAIChildren(DialogueNode currentNode)
        {
            foreach (DialogueNode node in GetAllChildren(currentNode))
            {
                if (!node.IsPlayerSpeaking())
                {
                    yield return node;
                }
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// 新しいノード作成処理
        /// </summary>
        /// <param name="parent"></param>
        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newNode = MakeNode(parent);
            Undo.RegisterCreatedObjectUndo(newNode, "Create Node");
            Undo.RecordObject(this, "ノードを作成しました");

            nodes.Add(newNode);
            OnValidate();
           
        }

      

        /// <summary>
        /// ノードを削除する処理
        /// </summary>
        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "ノードを消しました");
            nodes.Remove(nodeToDelete);
            OnValidate();
            CleanDandlingChildren(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
        }

        private  DialogueNode MakeNode(DialogueNode parent)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();

            if (parent != null)
            {
                parent.AddChild(newNode.name);
                newNode.SetPlaySpeaking(!parent.IsPlayerSpeaking());
                newNode.SetPosition(parent.GetRect().position + newNodeOffset);
            }
            return newNode;
        }
        private void AddNode(DialogueNode newNode)
        {
            nodes.Add(newNode);
            OnValidate();
        }

        private void CleanDandlingChildren(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.RemoveChild(nodeToDelete.name);
            }
        }
#endif
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR

            if(nodes.Count == 0)
            {
                DialogueNode newNode = MakeNode(null);
                nodes.Add(newNode);
            }
            if (AssetDatabase.GetAssetPath(this)!="")
            {
                foreach(DialogueNode node in GetAllNodes())
                {
                    if(AssetDatabase.GetAssetPath(node)=="")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {
            
        }
    }

}
