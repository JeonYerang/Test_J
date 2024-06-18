using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerAttack : MonoBehaviour
{
    public enum AttackState
    {
        Idle,
        Attack,
        BeAttacked,
        Died
    }
    public AttackState state;

    public int maxHp;
    protected int currentHp = 0;
    public int CurrentHp { get { return currentHp; } }
    public int HpAmount { 
        get {
            if (currentHp <= 0) return 0;
            else return currentHp / maxHp; 
        } 
    }

    public int attackPoint;
    public float attackSpeed;

    public bool CanAttack { get { return state == AttackState.Idle; } }

    PlayerClass playerClass;
    Skill[] skills;

    public event EventHandler<Player> onChangedHp;

    public Animator animator;

    protected void Init()
    {
        currentHp = maxHp;
        state = AttackState.Idle;

        onChangedHp += GameUIManager.Instance.UserInfo.SetHpBar;
    }

    public void SetClass(PlayerClass playerClass)
    {
        this.playerClass = playerClass;

    }

    public void SetAnimator(string name)
    {

    }

    #region Attack
    public int AttackCount { get; protected set; }
    
    public bool TryUsingSkill(int skillIndex)
    {
        if (!CanAttack)
            return false;

        UsingSkill(skills[skillIndex]);
        return true;
    }

    public void TryUsingSkill(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        var key = (KeyCode)((context.control as KeyControl).keyCode);
        print(key.ToString());
        //var keyInteraction = context.interaction

        if (!CanAttack)
            return;

        //UsingSkill(skills[skillIndex]);
        return;
    }

    private void UsingSkill(Skill skill)
    {
        state = AttackState.Attack;

        //animator.SetTrigger(animationName);
        skill.Shot();

        AttackCount++;
        SkillManager.Instance.AddCoolDic(skill);
    }

    protected void ReturnIdleState()
    {
        state = AttackState.Idle;
    }
    #endregion

    #region About Hp
    public void GetDamage(int damage)
    {
        if (state == AttackState.Died)
            return;

        //state = AttackState.BeAttacked;
        currentHp -= damage;

        if(currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }

        Player player = GetComponent<PlayerInfo>().player;
        onChangedHp?.Invoke(this, player);
    }

    public void TakeHeal(int amount)
    {
        if (state == AttackState.Died)
            return;

        currentHp += amount;

        if (currentHp > maxHp)
            currentHp = maxHp;

        Player player = GetComponent<PlayerInfo>().player;
        onChangedHp?.Invoke(this, player);
    }

    private void Die()
    {
        state = AttackState.Died;
        SpawnManager.Instance.DespawnCharacter();
    }
    #endregion
}
