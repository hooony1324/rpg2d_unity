using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{

    UI_Joystick _joystick;
    public UI_Joystick Joystick { get { return _joystick; } }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;


        Managers.Resource.Instantiate("TestMap");
        GameObject hero = Managers.Resource.Instantiate("Hero");

        Managers.Game.Cam.transform.position = hero.transform.position;
        Managers.Game.Cam.Target = hero;

        _joystick = Managers.UI.ShowSceneUI<UI_Joystick>();



        return true;
    }


    public override void Clear()
    {
        
    }
}
