using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveActor : Actor
{
    public bool humanPlayer;
    public bool controllable;
    bool inputAllowed;
    public ControlActions controls;

    public bool actOneAuto;
    public bool actTwoAuto;
    public bool actThreeAuto;
    public bool actFourAuto;

    public override void Awake()
    {
        base.Awake();
        controls = new ControlActions();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        inputAllowed = PlayerGUI.instance.playerInput;
        SpriteProcessing();

        if (controllable)
        {
            if (inputAllowed)
            {
                if (stunned == false && dead == false)
                {
                    GetActions();
                }
            }
        }
        else
        {
            GetActions();
        }
    }

    public virtual void FixedUpdate()
    {
        if (controllable)
        {
            if (inputAllowed)
            {
                if (stunned == false && dead == false)
                {
                    GetMovement();
                }
            }
        }
        else
        {
            GetAutoMovement();
        }
    }

    public virtual void GetMovement()
    {
        // How does this Actor Move?
    }

    public void GetAutoMovement()
    {
        // How does this Actor Move?
    }

    private void GetActions()
    {
        if (controls.Base.Pause.triggered)
        {
            PlayerGUI.instance.PauseMenu();
        }

        if (actOneAuto)
        {
            if (controls.Base.Jump.ReadValue<bool>())
            {
                actionOne();
            }
        }
        else
        {
            if (controls.Base.Jump.triggered)
            {
                actionOne();
            }
        }

        if (actTwoAuto)
        {
            if (controls.Base.AutoShoot.ReadValue<float>() != 0)
            {
                actionTwo();
            }
        }
        else
        {
            if (controls.Base.Shoot.triggered)
            {
                actionTwo();
            }
        }

        if (actThreeAuto)
        {
            actionThree(controls.Base.AutoSkill.ReadValue<float>() != 0);
        }
        else
        {
            if (controls.Base.Skill.triggered)
            {
                actionThree();
            }
        }

        if (actFourAuto)
        {
            if (controls.Base.ActionB.ReadValue<bool>())
            {
                actionFour();
            }
        }
        else
        {
            if (controls.Base.ActionB.triggered)
            {
                actionFour();
            }
        }

        if (controls.Base.NextWep.triggered)
        {
            leftAction();
        }

        if (controls.Base.PrevWep.triggered)
        {
            rightAction();
        }
    }

    public virtual void actionOne()
    {
        // Z key
    }

    public virtual void actionTwo()
    {
        // X key
    }

    public virtual void actionThree(bool keyPressed)
    {
        // C key
    }

    public virtual void actionThree()
    {
        // C key
    }

    public virtual void actionFour()
    {
        // V key
    }

    public virtual void leftAction()
    {
        // left shift
    }

    public virtual void rightAction()
    {
        // right shift
    }

    public virtual void SpriteProcessing()
    {
        // animations
    }
}
