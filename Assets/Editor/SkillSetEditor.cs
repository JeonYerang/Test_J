using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[CustomEditor(typeof(SkillData))]
public class SkillSetEditor : Editor
{
    private Dictionary<SkillCastType, Type> castDataDic 
        = new Dictionary<SkillCastType, Type>()
        {
            { SkillCastType.Basic, typeof(BasicTypeParam) },
            { SkillCastType.Charge, typeof(ChargeTypeParam) },
            { SkillCastType.Combo, typeof(ComboTypeParam) },
            { SkillCastType.OnOff, typeof(OnOffTypeParam) },
        };

    public SerializedProperty castTypeProperty;
    public SerializedProperty typeParamProperty;

    private void OnEnable()
    {
        castTypeProperty = serializedObject.FindProperty("castType");
        typeParamProperty = serializedObject.FindProperty("typeParam");
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
        typeParamProperty.managedReferenceValue = null;

        SkillCastType SelectedCastType = (SkillCastType)castTypeProperty.enumValueIndex;
        Type classType = castDataDic[SelectedCastType];
        var createdInstance = (ISkillTypeParam)Activator.CreateInstance(classType);

        typeParamProperty.managedReferenceValue = createdInstance;

        serializedObject.ApplyModifiedProperties();

        //Debug.Log($"{createdInstance} 생성됨");
    }
}