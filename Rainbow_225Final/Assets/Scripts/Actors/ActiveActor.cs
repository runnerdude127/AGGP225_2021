using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveActor : Actor
{
    public bool controllable;
    bool inputAllowed;

    public bool actOneAuto;
    public bool actTwoAuto;
    public bool actThreeAuto;
    public bool actFourAuto;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    public virtual void Update()
    {
        inputAllowed = PlayerGUI.instance.playerInput;
        SpriteProcessing();

        if (controllable)
        {
            if (inputAllowed)
            {
                GetActions();
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
                GetMovement();
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
        if (actOneAuto)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                actionOne();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                actionOne();
            }
        }

        if (actTwoAuto)
        {
            if (Input.GetKey(KeyCode.X))
            {
                actionTwo();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                actionTwo();
            }
        }

        if (actThreeAuto)
        {
            if (Input.GetKey(KeyCode.C))
            {
                actionThree();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                actionThree();
            }
        }

        if (actFourAuto)
        {
            if (Input.GetKey(KeyCode.V))
            {
                actionFour();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                actionFour();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            leftAction();
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
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
