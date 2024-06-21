using Photon.Pun;
using Photon.Realtime;
using System;
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

    public event EventHandler<Player> onChangedHp;

    public Animator animator;

    public ChargeSkill testSkill;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        /*chargeSkillAction = GetComponent<PlayerInput>().actions.FindAction("Skill");
        chargeSkillAction.started +=
            ctx =>
            {
                if (ctx.interaction is SlowTapInteraction)
                {
                    isCharging = true;
                    //testSkill.StartCharge();
                    print("Start Charge");
                }
            };
        chargeSkillAction.canceled +=
            ctx =>
            {
                isCharging = false;
            };
        chargeSkillAction.performed +=
            ctx =>
            {
                if (isCharging)
                {
                    isCharging = false;
                    //testSkill.EndCharge();
                    print("Charging Shot");
                }
            };

        skillAction = GetComponent<PlayerInput>().actions.FindAction("Skill1");
        skillAction.performed +=
            ctx =>
            {
                print("Shot");
            };*/
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
        skills = SkillManager.Instance.GetSkillList(skillSets);
    }

    public void SetAnimator(string name)
    {

    }

    #region Using SKill
    public int AttackCount { get; protected set; }
    public void OnPressSkillKey(InputAction.CallbackContext context)
    {
        KeySetting.skillKeyPathDic.TryGetValue(context.control.path, out int skillIndex);

        if (context.performed)
        {
            if (CanAttack)
            {
                if (context.interaction is HoldInteraction)
                {
                    StartCharge(skillIndex);
                }
                else
                {
                    UsingSkill(skillIndex);
                }
            }
        }
        else if (context.canceled)
        {
            if (state == AttackState.Charge)
            {
                if(skillIndex == chargeIndex)
                    EndCharge();
            }
        }
    }

    public bool TryUsingSkill(int skillIndex)
    {
        if (!CanAttack)
            return false;

        UsingSkill(skillIndex);
        return true;
    }

    [PunRPC]
    private void UsingSkill(int skillIndex)
    {
        state = AttackState.Attack;

        print($"Shot!: {skillIndex}");

        /*Skill skill = skills[skillIndex];

        //animator.SetTrigger(animationName);
        skill.Shot();
        print($"Shot!: {skill.name}");

        AttackCount++;
        SkillManager.Instance.AddCoolDic(skill);*/

        ReturnIdleState();

        pv.RPC("InstantiateSkillEffect", RpcTarget.All, transform.position, transform.forward);
    }
    string animationName;
    SkillObject skillPrefab;

    [PunRPC]
    private void InstantiateSkillEffect(Vector3 shotPos, Vector3 shotDir, int damage, PhotonMessageInfo info)
    {
        Player owner = info.Sender;

        //지연(시차)
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);

        var skillEffect = Instantiate(skillPrefab, shotPos, Quaternion.identity);
        skillEffect.InitAndShot(owner, damage);

        //지연 보상
    }

    #region Charging
    int chargeIndex = -1;
    ChargeSkill chargeSkill = null;
    [PunRPC]
    public void StartCharge(int skillIndex)
    {
        print($"Start Charging: {skillIndex}");

        chargeSkill = skills[skillIndex] as ChargeSkill;

        if( chargeSkill != null)
        {
            state = AttackState.Charge;

            chargeIndex = skillIndex;
            chargeSkill.StartCharge();
        }
    }

    [PunRPC]
    public void EndCharge()
    {
        print($"End Charging: {chargeIndex}");

        state = AttackState.Attack;

        chargeIndex = -1;

        if(chargeSkill != null)
        {
            chargeSkill?.EndCharge();
            UsingSkill(chargeIndex);
        }
    }
    #endregion

    private void ReturnIdleState() //애니메이터 key event
    {
        state = AttackState.Idle;
    }
    #endregion

    #region About Hp
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
