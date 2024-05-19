using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Define;

public class Hero : Creature
{
    private Vector3 _moveDir { get; set; }
    public float Speed { get; set; } = 5f;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;
        Managers.Game.OnJoystickStateChanged -= HandleOnJoystickStateChanged;
        Managers.Game.OnJoystickStateChanged += HandleOnJoystickStateChanged;

        return true;
    }

    private void Update()
    {
        Vector3 dir = _moveDir * (Time.deltaTime * Speed);
        Vector3 newPos = transform.position + dir;

        transform.position = newPos;
    }

    private void HandleOnMoveDirChanged(Vector2 newDir)
    {
        _moveDir = newDir;

        if (_moveDir.sqrMagnitude > 0)
        {
            CreatureState = ECreatureState.Move;
            LookLeft = _moveDir.x < 0;
        }
        else
        {
            CreatureState = ECreatureState.Idle;
        }
    }

    private void HandleOnJoystickStateChanged(EJoystickState joystickState)
    {

    }
}
