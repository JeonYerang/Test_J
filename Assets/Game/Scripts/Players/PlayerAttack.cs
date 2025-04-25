using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

/*public enum AttackAnimation
{

}*/

//본인의 포톤뷰가 아닐 경우 return
public class PlayerAttack : MonoBehaviour
{
    PhotonView pv;
    public bool IsMine { get { return pv.IsMine; } }

    public enum AttackState
    {
        Idle,
        Charge,
        Attack,
        BeAttacked,
        Died
    }
    public AttackState state;

    public int maxHp;
    private int currentHp = 0;
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

    public Animator animator;

    public ChargeSkill testSkill;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    protected void Init()
    {
        currentHp = maxHp;
        state = AttackState.Idle;

        onChangedHp += GameUIManager.Instance.UserInfo.SetHpBar;
        
    }

    public void SetClass(PlayerClass playerClass)
    {
        this.playerClass = playerClass;
        SkillData[] skillSets = ClassManager.Instance.GetSkillSets(playerClass);

        SkillManager.Instance.InitSkillUI(skillSets);
        skills = SkillManager.Instance.GetSkillList(skillSets);
    }

    public void SetAnimator(string name)
    {

    }

    #region Using SKill
    int currentSkill = -1;
    public void TryUsingSkill(int skillIndex)
    {
        print($"OnClick {skillIndex}");

        if (!CanAttack)
            return;

        if (currentSkill != -1)
            return;

        Skill skill = skills[skillIndex];

        if (skill.castType == SkillCastType.Basic)
        {
            ShotSkill(skillIndex);
        }
        else if (skill.castType == SkillCastType.Charge)
        {
            currentSkill = skillIndex;
            StartCharge(skillIndex);
        }
    }
    public void EndSKill(int skillIndex)
    {
        print($"OnCancel {skillIndex}");

        if (currentSkill != skillIndex)
            return;

        Skill skill = skills[skillIndex];

        if (skill.castType == SkillCastType.Charge)
        {
            EndCharge(skillIndex);
        }
        currentSkill = -1;
    }

    [PunRPC]
    public void ShotSkill(int skillIndex)
    {
        state = AttackState.Attack;

        Skill skill = skills[skillIndex];

        print($"Shot!: {skill.name}");
        skill.Shot();

        //animator.SetTrigger(animationName);

        ReturnIdleState();
    }

    string animationName;
    SkillObject skillPrefab;

    #region Charging
    ChargeSkill currentChargeSkill = null;
    [PunRPC]
    public void StartCharge(int skillIndex)
    {
        print($"Start Charging: {skillIndex}");

        currentChargeSkill = skills[skillIndex] as ChargeSkill;

        if( currentChargeSkill != null)
        {
            state = AttackState.Charge;

            currentChargeSkill.StartCharge();
        }
    }

    [PunRPC]
    public void EndCharge(int skillIndex)
    {
        if(currentChargeSkill == null)
        {
            return;
        }

        print($"End Charging: {skillIndex}");

        currentChargeSkill?.EndCharge();
        currentChargeSkill = null;

        state = AttackState.Attack;
    }
    #endregion

    private void ReturnIdleState() //애니메이터 key event
    {
        state = AttackState.Idle;
    }
    #endregion

    #region About Hp
    public event EventHandler<Player> onChangedHp;
    [PunRPC]
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

    public void GetHeal(int amount)
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
