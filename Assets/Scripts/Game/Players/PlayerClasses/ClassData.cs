using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Class Data", menuName = "Scriptable Object/Class Data")]
public class ClassData : ScriptableObject
{
    public Sprite classIcon;
    public string classDescription;

    public GameObject classRenderer;
    public PlayerAttack attack;
    public GameObject attackPrefab;
}
