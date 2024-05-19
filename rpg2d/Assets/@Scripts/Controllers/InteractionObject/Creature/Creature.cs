using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static Define;

public class Creature : InteractionObject
{
    protected Animator _anim;
    protected SpriteRenderer _sprite;

    [SerializeField] protected ECreatureState _creatureState = ECreatureState.Idle;
    public virtual ECreatureState CreatureState
    {
        get => _creatureState;
        set
        {
            if (_creatureState != value)
            {
                _creatureState = value;
                CancelWait();

                UpdateAnimation();
            }
        }
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        _anim = Util.FindChild<Animator>(gameObject, "Animator", true);
        _sprite = Util.FindChild<SpriteRenderer>(gameObject, "Animator");

        SetInfo(0);

        return true;
    }

    public virtual void SetInfo(int templateId)
    {
        //InitPos = transform.position;
        //TemplateId = templateId;

        //if (ObjectType == EObjectType.Hero)
        //    CreatureData = Managers.Data.HeroDic[templateId];
        //else
        //    CreatureData = Managers.Data.MonsterDic[templateId];

        _anim.runtimeAnimatorController = Managers.Resource.Load<RuntimeAnimatorController>("Knight");

        CreatureState = ECreatureState.Idle;
    }
    #region Animation

    protected virtual void UpdateAnimation()
    {
        switch (CreatureState)
        {
            case ECreatureState.Idle:
                _anim.Play(AnimName.IDLE);
                break;
            case ECreatureState.Cooltime:
                //PlayAnimation(0, AnimName.IDLE, true);
                break;
            case ECreatureState.Skill:
                break;
            case ECreatureState.Move:
                _anim.Play(AnimName.MOVE);
                
                break;
            case ECreatureState.OnDamaged:
                //PlayAnimation(0, AnimName.IDLE, true);
                //if (Skills.CurrentSkill != null)
                //    Skills.CurrentSkill.CancelSkill();
                break;
            case ECreatureState.Dead:
                //PlayAnimation(0, AnimName.DEAD, false);
                //OnDead();
                break;
            default:
                break;
        }
    }

    public override void Flip(bool flag)
    {
        if (_sprite == null)
            return;

        _sprite.flipX = flag;
    }
    #endregion
    #region FSM
    protected Coroutine _coWait;

    protected void StartWait(float seconds)
    {
        CancelWait();
        _coWait = StartCoroutine(CoWait(seconds));
    }

    IEnumerator CoWait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _coWait = null;
    }

    protected void CancelWait()
    {
        if (_coWait != null)
            StopCoroutine(_coWait);
        _coWait = null;
    }

    //protected IEnumerator CoUpdateAI()
    //{
    //    while (true)
    //    {
    //        switch (CreatureState)
    //        {
    //            case ECreatureState.Idle:
    //                UpdateIdle();
    //                UpdateAITick = 0.1f;
    //                break;
    //            case ECreatureState.Cooltime:
    //                UpdateCooltime();
    //                UpdateAITick = 0.1f;
    //                break;
    //            case ECreatureState.Move:
    //                UpdateAITick = 0.0f;
    //                UpdateMove();
    //                break;
    //            case ECreatureState.Skill:
    //                UpdateAITick = 0.1f;
    //                UpdateSkill();
    //                break;
    //            case ECreatureState.Dead:
    //                UpdateAITick = 1f;
    //                UpdateDead();
    //                break;
    //        }

    //        if (UpdateAITick > 0)
    //            yield return new WaitForSeconds(UpdateAITick);
    //        else
    //            yield return null;
    //    }
    //}

    protected virtual void UpdateIdle()
    {
        
    }

    protected virtual void UpdateMove()
    {

    }

    protected virtual void UpdateSkill()
    {

    }

    protected virtual void UpdateDead()
    {

    }
    #endregion

    #region Battle
    protected virtual void OnDamaged(InteractionObject attacker, float damage)
    {

    }
    #endregion

}
