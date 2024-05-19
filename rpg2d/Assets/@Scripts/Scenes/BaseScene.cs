using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : InitBase
{
    public Define.EScene SceneType { get; protected set; } = Define.EScene.Unknown;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";

        return true;
    }

    public abstract void Clear();
}
