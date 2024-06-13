using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[CustomEditor(typeof(SkillSet))]
public class SkillSetEditor : Editor
{
    //SkillData skillData = null;

    private Dictionary<SkillCastType, Type> castDataDic;

    public SerializedProperty castTypeProperty;
    public SerializedProperty castDataProperty;

    private void OnEnable()
    {
        InitCastDataDic();
        castTypeProperty = serializedObject.FindProperty("castType");
        castDataProperty = serializedObject.FindProperty("castData");
    }

    private void InitCastDataDic()
    {
        castDataDic = new Dictionary<SkillCastType, Type>()
        {
            { SkillCastType.Basic, typeof(BasicCastData) },
            { SkillCastType.Charge, typeof(ChargeCastData) },
            { SkillCastType.Combo, typeof(ComboCastData) },
            { SkillCastType.OnOff, typeof(OnOffCastData) },
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        //castType이 변경되면
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(castTypeProperty); //이거 없으면 변경 감지가 안되네...

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            MapCastSet();
        }

        //if (castSetProperty.managedReferenceValue != null)
        //    EditorGUILayout.PropertyField(castSetProperty, true);

        serializedObject.ApplyModifiedProperties();
    }

    public void MapCastSet()
    {
        SkillCastType SelectedCastType = (SkillCastType)castTypeProperty.enumValueIndex;

        if (castDataDic.TryGetValue(SelectedCastType, out Type classType))
        {
            var createdInstance = (SkillCastData)Activator.CreateInstance(classType);
            castDataProperty.managedReferenceValue = createdInstance;

            serializedObject.ApplyModifiedProperties();

            Debug.Log($"{createdInstance} 생성됨");
        }
    }
}
