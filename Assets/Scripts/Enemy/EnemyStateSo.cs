using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterStats", menuName = "Character Stats")]
public class EnemyStateSo : ScriptableObject
{
    public int maxHealth;
    public int attackDamage;
}
