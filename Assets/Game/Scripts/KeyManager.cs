using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class KeySetting
{
    //<key code, skill index>
    public static Dictionary<KeyCode, int> skillKeyDic = new Dictionary<KeyCode, int>();
    public static KeyCode jumpKey;
}

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance { get; private set; }

    public InputAction inputAction;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        SetKeyCode(KeyCode.Z, 0);
        SetKeyCode(KeyCode.X, 1);
        SetKeyCode(KeyCode.C, 2);
        SetKeyCode(KeyCode.V, 3);
        SetJumpKey(KeyCode.Space);
    }

    public void SetKeyCode(KeyCode keyCode, int skillIndex)
    {
        if(KeySetting.skillKeyDic.ContainsKey(keyCode))
            KeySetting.skillKeyDic[keyCode] = skillIndex;
        else
            KeySetting.skillKeyDic.Add(keyCode, skillIndex);
    }

    public void SetJumpKey(KeyCode keyCode)
    {
        KeySetting.jumpKey = keyCode;
    }
}
