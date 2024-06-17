using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public static class KeySetting //inputAction�� ���ε� ����� ������ ��� ����?
{
    //<key code, skill index>
    public static Dictionary<Key, int> skillKeyDic;
    /*static KeySetting()
    {
        skillKeyDic = new Dictionary<Key, int>()
        {
            { Key.Z, 0 },
            { Key.X, 1 },
            { Key.C, 2 }
        };

        foreach (var key in KeySetting.skillKeyDic.Keys)
            skillAction.AddBinding($"<Keyboard>/{key.ToString().ToLower()}");
    }*/

}

public class KeyBinder : MonoBehaviour
{
    public InputActionAsset inputActionAsset;
    private InputAction skillAction;
    private Dictionary<Key, int> newSkillKeyDic;

    private void Awake()
    {
        skillAction = inputActionAsset.FindActionMap("Player").FindAction("Skill");
    }

    private void OnEnable()
    {
        newSkillKeyDic = KeySetting.skillKeyDic;
    }

    private void OnDisable()
    {
        newSkillKeyDic.Clear();
    }

    public void SetSkillKey(Key newKey, int skillIndex) //���� ��ų�� ����ϴ� ���
    {
        if (newSkillKeyDic.ContainsKey(newKey)) //�ش� Ű�� �ٸ� ��ų�� ��ϵǾ��ִ� ���
            newSkillKeyDic[newKey] = skillIndex;
        else
            newSkillKeyDic.Add(newKey, skillIndex);
    }

    public void ReSetSkillKey(Key prevKey, Key newKey) //��ϵ� ��ų�� �ٸ� Ű�� �ű�� ���
    {
        if (!newSkillKeyDic.ContainsKey(prevKey))
            return;

        if (newSkillKeyDic.ContainsKey(newKey)) //���ο� Ű�� �ٸ� ��ų�� ��ϵǾ��ִ� ���: ����
            (newSkillKeyDic[prevKey], newSkillKeyDic[newKey])
            = (newSkillKeyDic[newKey], newSkillKeyDic[prevKey]);
        else
        {
            newSkillKeyDic.Add(newKey, newSkillKeyDic[prevKey]);
            newSkillKeyDic.Remove(prevKey);
        }
    }

    public void RemoveSkillKey(Key prevKey) //��ϵ� ��ų�� �����ϴ� ���
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
            skillAction.AddBinding($"<Keyboard>/{key.ToString().ToLower()}");

        foreach(var key in RemovedKeyDic.Keys)
        {
            InputBinding RemovedKey = skillAction.bindings.FirstOrDefault(
                b => b.path == $"<Keyboard>/{key.ToString().ToLower()}");
            skillAction.RemoveBindingOverride(RemovedKey);
        }

        KeySetting.skillKeyDic.Clear();
        KeySetting.skillKeyDic = newSkillKeyDic;
    }
}
