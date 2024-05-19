using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class GameManager
{


    #region Player

    #endregion

    #region Action

    private Vector2 _moveDir;
    public Vector2 MoveDir
    {
        get => _moveDir;
        set
        {
            _moveDir = value;
            OnMoveDirChanged?.Invoke(_moveDir);
        }
    }

    private EJoystickState _joystickState;
    public EJoystickState JoystickState
    {
        get => _joystickState;
        set
        {
            _joystickState = value;
            OnJoystickStateChanged?.Invoke(_joystickState);
        }
    }
    public EJoystickType JoystickType = EJoystickType.Flexible;

    private CameraController _cam;
    public CameraController Cam
    {
        get
        {
            if (_cam == null)
            {
                _cam = Object.FindObjectOfType<CameraController>();
            }

            return _cam;
        }
    }

    public event Action<Vector2> OnMoveDirChanged;
    public event Action<EJoystickState> OnJoystickStateChanged;
    public event Action<EBroadcastEventType, ECurrencyType, int> OnBroadcastEvent;

    #endregion
}
