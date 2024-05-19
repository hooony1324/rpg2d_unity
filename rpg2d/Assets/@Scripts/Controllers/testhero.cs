using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class testhero : MonoBehaviour
{
    Animator _anim;

    void Start()
    {
        _anim = Util.FindChild<Animator>(gameObject, "Animator", true);

        Managers.Resource.LoadAsync<RuntimeAnimatorController>("Knight", (anim) =>
        {
            RuntimeAnimatorController animController = Managers.Resource.Load<RuntimeAnimatorController>("Knight");

            _anim.runtimeAnimatorController = animController;
            _anim.Play(AnimName.ATTACK);
        });
    }


}
