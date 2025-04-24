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
    private Dictionary<Key, int> newSkillKeyDic;

    private void Awake()
    {
        skillAction = inputActionAsset.FindActionMap("Player").FindAction("Skill");
        InitSkillKey();
    }

    private void OnEnable()
    {
        newSkillKeyDic = KeySetting.skillKeyDic;
    }

    private void OnDisable()
    {
        //newSkillKeyDic.Clear();
    }

    public void InitSkillKey()
    {
        /*KeySetting.skillKeyDic = new Dictionary<Key, int>()
        {
            { Key.Z, 0 },
            { Key.X, 1 },
            { Key.C, 2 }
        };

        foreach (var key in KeySetting.skillKeyDic.Keys)
            skillAction.AddBinding($"<Keyboard>/{key.ToString().ToLower()}");*/

        //action.AddBinding("<Gamepad>/leftStick").WithInteractions("tap(duration=0.8)");
    }

    #region 스킬키 지정
    public void SetSkillKey(Key newKey, int skillIndex) //새로 스킬을 등록하는 경우
    {
        if (newSkillKeyDic.ContainsKey(newKey)) //해당 키에 다른 스킬이 등록되어있는 경우
            newSkillKeyDic[newKey] = skillIndex;
        else
            newSkillKeyDic.Add(newKey, skillIndex);
    }

    public void ReSetSkillKey(Key prevKey, Key newKey) //등록된 스킬을 다른 키로 옮기는 경우
    {
        if (!newSkillKeyDic.ContainsKey(prevKey))
            return;

        if (newSkillKeyDic.ContainsKey(newKey)) //새로운 키에 다른 스킬이 등록되어있는 경우: 스왑
            (newSkillKeyDic[prevKey], newSkillKeyDic[newKey])
            = (newSkillKeyDic[newKey], newSkillKeyDic[prevKey]);
        else
        {
            newSkillKeyDic.Add(newKey, newSkillKeyDic[prevKey]);
            newSkillKeyDic.Remove(prevKey);
        }
    }

    public void RemoveSkillKey(Key prevKey) //등록된 스킬을 삭제하는 경우
    {
        if (newSkillKeyDic.ContainsKey(prevKey))
            newSkillKeyDic.Remove(prevKey);
    }

    public void SaveSkillAction()
    {
        Dictionary<Key, int> AddedKeyDic
            = newSkillKeyDic.Where(entry => !KeySetting.skillKeyDic.ContainsKey(entry.Key))
            .ToDictionary(entry => entry.Key, entry => entry.Value);

        Dictionary<Key, int> RemovedKeyDic
            = KeySetting.skillKeyDic.Where(entry => !newSkillKeyDic.ContainsKey(entry.Key))
            .ToDictionary(entry => entry.Key, entry => entry.Value);

        foreach (var key in AddedKeyDic.Keys)
        {
            skillAction.AddBinding($"<Keyboard>/{key.ToString().ToLower()}");
        }

        foreach (var key in RemovedKeyDic.Keys)
        {
            InputBinding RemovedKey = skillAction.bindings.FirstOrDefault(
                b => b.path == $"<Keyboard>/{key.ToString().ToLower()}");
            skillAction.RemoveBindingOverride(RemovedKey);
        }

        KeySetting.skillKeyDic.Clear();
        KeySetting.skillKeyDic = newSkillKeyDic;
    }

    public void OnPressSkillKey(InputAction.CallbackContext context)
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
            if (context.started || context.performed)
            {
                print($"OnClick {skillIndex}");
            }
            else if (context.canceled)
            {
                print($"OnCancel {skillIndex}");
            }
        }
    }
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
