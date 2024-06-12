using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[CustomEditor(typeof(SkillData))]
public class SOCollectionEditor : Editor
{
    //private SerializedProperty castType;

    private Dictionary<SkillCastType, Type> castSetDic;

    //private SerializedProperty castSetProperty;

    private void OnEnable()
    {
        InitCastDataDic();
        //castType = serializedObject.FindProperty("castType");
        //castSetProperty = serializedObject.FindProperty("castSet");
    }

    private void InitCastDataDic()
    {
        castSetDic = new Dictionary<SkillCastType, Type>()
        {
            { SkillCastType.Basic, typeof(BasicCastSet) },
            { SkillCastType.Charge, typeof(ChargeCastSet) },
            { SkillCastType.Combo, typeof(ComboCastSet) },
            { SkillCastType.OnOff, typeof(OnOffCastSet) },
        };
    }

    public override void OnInspectorGUI()
    {
        SkillData skillData = (SkillData)target;

        DrawDefaultInspector();

        //castType이 변경되면
        EditorGUI.BeginChangeCheck();
        //EditorGUILayout.PropertyField(castType);
        SkillCastType castTypeValue = (SkillCastType)EditorGUILayout.EnumPopup("Cast Type", skillData.castType);

        if (EditorGUI.EndChangeCheck())
        {
            skillData.castType = castTypeValue;
            MapCastSet(skillData);
        }

        /*if(skillData.castSet != null)
        {
            EditorGUILayout.LabelField("Cast Set", EditorStyles.boldLabel);
            SerializedObject castSetObject = new SerializedObject(castSetProperty.objectReferenceValue);

            SerializedProperty property = castSetObject.GetIterator();
            property.NextVisible(true);

            while (property.NextVisible(false))
            {
                EditorGUILayout.PropertyField(property, true);
            }

            castSetObject.ApplyModifiedProperties();
        }*/

        //변경 사항 저장
        if (GUI.changed)
        {
            EditorUtility.SetDirty(skillData);
            serializedObject.ApplyModifiedProperties();
        }
    }

    public void MapCastSet(SkillData skillData)
    {
        if (castSetDic.TryGetValue(skillData.castType, out Type classType))
        {
            skillData.castSet = (SkillCastSet)Activator.CreateInstance(classType);
        }

        Debug.Log($"{skillData.castSet} 생성됨");
    }
}

[CreateAssetMenu(fileName = "Skill Data", menuName = "Scriptable Object/Skill Data")]
public class SkillData : ScriptableObject
{
    public string _name;
    public Sprite icon;

    public int damage;
    public float coolTime;

    [HideInInspector]
    public SkillCastType castType;
    [SerializeField]
    public SkillCastSet castSet;
}

public class SkillCastSet
{

}

[Serializable]
public class BasicCastSet : SkillCastSet
{
    public string skillAnimation;
    public SkillObject skillPrefab;
}

public class ComboCastSet : SkillCastSet
{
    public int maxComboCount;

    public string[] skillAnimation;
    public SkillObject[] skillPrefab;
}

public class ChargeCastSet : SkillCastSet
{
    public float maxChargeCount;

    public string skillAnimation;
    public SkillObject skillPrefab;

    public GameObject ChargingEffect;
}

public class OnOffCastSet : SkillCastSet
{
    public string skillAnimation;
    public SkillObject skillPrefab;
}