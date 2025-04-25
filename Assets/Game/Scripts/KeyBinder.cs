using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Interactions;

public static class KeySetting
{
    //<key code, skill index>
    public static Dictionary<Key, int> skillKeyDic;
}

public class KeyBinder : MonoBehaviour
{
    public InputActionAsset inputActionAsset;
    private InputAction skillAction;
    private Dictionary<Key, int> editSkillKeyDic;

    private void Awake()
    {
        skillAction = inputActionAsset.FindActionMap("Player").FindAction("Skill");
        InitSkillKey();
    }

    private void OnEnable()
    {
        editSkillKeyDic = KeySetting.skillKeyDic;
    }

    private void OnDisable()
    {
        editSkillKeyDic.Clear();
    }

    #region 스킬키 바인딩
    public void InitSkillKey()
    {
        KeySetting.skillKeyDic = new Dictionary<Key, int>() //기본값
        {
            { Key.Z, 0 },
            { Key.X, 1 },
            { Key.C, 2 }
        };

        foreach (var key in KeySetting.skillKeyDic.Keys)
            skillAction.AddBinding($"<Keyboard>/{key.ToString().ToLower()}", "Hold");

        //action.AddBinding("<Gamepad>/leftStick").WithInteractions("tap(duration=0.8)");

        //둘 중 뭐지?
        skillAction.started += OnStartSkillKey;
        skillAction.canceled += OnCancelSkillKey;
    }

    public void SaveSkillAction() //저장
    {
        Dictionary<Key, int> AddedKeyDic
            = editSkillKeyDic.Where(entry => !KeySetting.skillKeyDic.ContainsKey(entry.Key))
            .ToDictionary(entry => entry.Key, entry => entry.Value);

        Dictionary<Key, int> RemovedKeyDic
            = KeySetting.skillKeyDic.Where(entry => !editSkillKeyDic.ContainsKey(entry.Key))
            .ToDictionary(entry => entry.Key, entry => entry.Value);

        foreach (var key in AddedKeyDic.Keys)
        {
            skillAction.AddBinding($"<Keyboard>/{key.ToString().ToLower()}", "Hold");
        }

        foreach (var key in RemovedKeyDic.Keys)
        {
            InputBinding RemovedKey = skillAction.bindings.FirstOrDefault(
                b => b.path == $"<Keyboard>/{key.ToString().ToLower()}");
            skillAction.RemoveBindingOverride(RemovedKey);
        }

        KeySetting.skillKeyDic.Clear();
        KeySetting.skillKeyDic = editSkillKeyDic;
    }

    public void OnStartSkillKey(InputAction.CallbackContext context)
    {
        int skillIndex = -1;

        var control = context.control;
        if (control is KeyControl keyControl)
        {
            Key key = keyControl.keyCode;
            KeySetting.skillKeyDic.TryGetValue(key, out skillIndex);
        }

        if (skillIndex != -1)
        {
            print($"OnStarted {skillIndex}");
        }
    }

    public void OnCancelSkillKey(InputAction.CallbackContext context)
    {
        int skillIndex = -1;

        var control = context.control;
        if (control is KeyControl keyControl)
        {
            Key key = keyControl.keyCode;
            KeySetting.skillKeyDic.TryGetValue(key, out skillIndex);
        }

        if (skillIndex != -1)
        {
            print($"OnCancel {skillIndex}");
        }
    }

    #region 스킬키 세팅 편집 딕셔너리
    public void SetSkillKey(Key newKey, int skillIndex) //새로 스킬을 등록하는 경우
    {
        if (editSkillKeyDic.ContainsKey(newKey)) //해당 키에 다른 스킬이 등록되어있는 경우
            editSkillKeyDic[newKey] = skillIndex;
        else
            editSkillKeyDic.Add(newKey, skillIndex);
    }

    public void MoveSkillKey(Key prevKey, Key newKey) //등록된 스킬을 다른 키로 옮기는 경우
    {
        if (!editSkillKeyDic.ContainsKey(prevKey))
            return;

        if (editSkillKeyDic.ContainsKey(newKey)) //새로운 키에 다른 스킬이 등록되어있는 경우: 스왑
            (editSkillKeyDic[prevKey], editSkillKeyDic[newKey])
            = (editSkillKeyDic[newKey], editSkillKeyDic[prevKey]);
        else
        {
            editSkillKeyDic.Add(newKey, editSkillKeyDic[prevKey]);
            editSkillKeyDic.Remove(prevKey);
        }
    }

    public void RemoveSkillKey(Key prevKey) //등록된 스킬을 삭제하는 경우
    {
        if (editSkillKeyDic.ContainsKey(prevKey))
            editSkillKeyDic.Remove(prevKey);
    }
    #endregion
    #endregion

    #region 키 바인딩
    InputActionRebindingExtensions.RebindingOperation oper;
    public void StartKeyBinding()
    {
        skillAction.Disable();

        oper = skillAction.PerformInteractiveRebinding().Start();
    }

    public void CancelKeyBinding()
    {
        oper.Cancel();
    }

    public void ConfirmKeyBinding()
    {
        oper.Dispose();

        skillAction.Enable();
    }
    #endregion
}
