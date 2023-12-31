using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create CharacterStatus")]
public class StatuaData : ScriptableObject
{
    public string NAME; //キャラ・敵名
    public int MAXHP; //最大HP
    public int MAXMP; //最大MP
    public int ATK; //攻撃力
    public int DEF; //防御力
    public int LV; //レベル
}