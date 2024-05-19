using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CameraController : MonoBehaviour
{
    public ECameraState State { get; set; }
    private bool isReady = false;
    private GameObject _target;
    //private InteractionObject _target;

    public GameObject Target
    {
        get { return _target; }
        set
        {
            _target = value;
            isReady = true;
        }
    }

    [SerializeField] public float smoothSpeed = 6f; // 스무딩 속도

    private int _targetOrthographicSize = 18;

    public void Start()
    {
        State = ECameraState.Following;
        Managers.Game.OnBroadcastEvent -= HandleOnBroadcast;
        Managers.Game.OnBroadcastEvent += HandleOnBroadcast;
    }
    public void SetCameraSize()
    {
        //if (Managers.Object.HeroCamp.CampState == ECampState.CampMode)
        //{
        //    _targetOrthographicSize = 27;
        //}
        //else
        //{
        //    _targetOrthographicSize = 18;
        //}
    }

    private void LateUpdate()
    {
        // Smoothly transition to the target camera size
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, _targetOrthographicSize, smoothSpeed * Time.deltaTime);

        HandleCameraPosition();
    }

    private void HandleCameraPosition()
    {
        if (isReady == false || State == ECameraState.Targeting)
            return;

        Vector3 targetPosition = new Vector3(Target.transform.position.x, Target.transform.position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.fixedDeltaTime);
    }
    public void TargetingCamera(GameObject dest)
    {
        //이미 진행중이면 리턴
        if (State == ECameraState.Targeting)
            return;

        State = ECameraState.Targeting;
        Vector3 targetPosition = new Vector3(Target.transform.position.x, Target.transform.position.y, -10f);
        Vector3 destPosition = new Vector3(dest.transform.position.x, dest.transform.position.y, -10f);

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(destPosition, 0.8f).SetEase(Ease.Linear))
            .AppendInterval(2f)
            .Append(transform.DOMove(targetPosition, 0.8f).SetEase(Ease.Linear))
            .OnComplete(() => { State = ECameraState.Following; });
    }

    private void HandleOnBroadcast(EBroadcastEventType type, ECurrencyType currencyType, int value)
    {
        switch (type)
        {
            case EBroadcastEventType.ChangeCampState:
                SetCameraSize();
                break;
        }
    }
}
