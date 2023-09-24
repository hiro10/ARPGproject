using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create CharacterStatus")]
public class StatuaData : ScriptableObject
{
    public string NAME; //�L�����E�G��
    public int MAXHP; //�ő�HP
    public int MAXMP; //�ő�MP
    public int ATK; //�U����
    public int DEF; //�h���
    public int LV; //���x��
}