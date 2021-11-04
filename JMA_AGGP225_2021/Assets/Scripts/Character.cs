using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : InGameActor
{
    #region Movement Stats

    public float speed = 5f;
    public float jumpSpeed = 1f;

    #endregion

    #region Movement Management

    public CharacterController characterController;
    public float horizontal;
    public float vertical;
    float velocity;

    #endregion

    #region Charge Management

    public bool usesCharge;

    public int charge = 100;
    public int currentCharge;
    float chargeSpeed = 1;
    public bool cooldownCharge = false;

    int chargeUsage = 2;                                  // For players, this should be driven by the type of weapon used!

    #endregion

    public override void Awake()
    {
        base.Awake();

        characterController = gameObject.GetComponent<CharacterController>();
        if (usesCharge)
        {
            StartCoroutine(ChargeBack());
            currentCharge = charge;
        }    
    }

    public override void Update()
    {
        base.Update();
        velocity += Physics.gravity.y * Time.deltaTime;
        characterController.Move((gameObject.transform.right * horizontal + gameObject.transform.forward * vertical + (new Vector3(0, 1, 0) * velocity)) * Time.deltaTime);
    }

    public void moveBy()
    {
        horizontal = Input.GetAxis("Horizontal") * speed;
        vertical = Input.GetAxis("Vertical") * speed;
        if (characterController.isGrounded)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                onJumping();
                velocity = jumpSpeed;
            }
            else
            {
                velocity = 0;
            }
        }
    }

    public void killMovement()
    {
        horizontal = 0;
        vertical = 0;
    }

    public void chargeAction()
    {
        if (!cooldownCharge)                             // If we're not in cooldown...
        {
            commitChargeAction();                        // This runs if the action can be done. For example, shooting with enough ammo. It shoots. Amazing!

            if ((currentCharge - chargeUsage) >= 0)      // If committing this action doesn't cause the charge go into cooldown...
            {
                currentCharge = currentCharge - chargeUsage;
                chargeSpeed = 1;
            }
            else                                         // Otherwise, this is the cooldown-inducer.
            {
                currentCharge = 0;
                cooldownCharge = true;

                chargeDepleted();                        // Function for the depletion of charge.
            }
        }
        else                                             // If the action is attempted while in cooldown, this happens.
        {
            invalidChargeAction();
        }
    }

    IEnumerator ChargeBack()
    {
        yield return new WaitForSeconds(chargeSpeed);                       // Wait 'chargeSpeed' time between increments to the charge meter.
        currentCharge = currentCharge + 1;                                  // Increment!
        if (currentCharge < charge)                                         // If there's still charge left to charge...
        {
            if (chargeSpeed <= .02f)                                        // If the chargeSpeed gets low enough, cap it at .02 seconds.
            {
                chargeSpeed = .02f;
            }
            else                                                            // Otherwise, we'll keep dividing chargeSpeed by 1.5 every increment.
            {
                chargeSpeed = chargeSpeed / 1.5f;
            }
            StartCoroutine(ChargeBack());                                   // Loop this to go back to waiting until the next increment.
        }
        else
        {
            if (currentCharge > charge)                                     // Making sure that the charge doesn't go over its limit.
            {
                currentCharge = charge;
            }
            cooldownCharge = false;                                         // If we were on cooldown, we're not anymore.

            chargeFilled();                                                 // This function is called in case we want any effects for fully charging.

            yield return new WaitUntil(() => currentCharge < charge);       // Wait until the need to charge is present again...
            chargeSpeed = 1;                                                // We need to charge again! Reset the speed.
            StartCoroutine(ChargeBack());                                   // Loop this back. Here we go again.
        }
    }

    #region Movement Virtuals
    public virtual void onJumping()
    {
        // This can be filled by whatever inherits from this. Default does nothing.
    }

    #endregion

    #region Charge Virtuals
    public virtual void chargeDepleted()
    {
        // This can be filled by whatever inherits from this. Default does nothing.
    }

    public virtual void chargeFilled()
    {
        // This can be filled by whatever inherits from this. Default does nothing.
    }

    public virtual void commitChargeAction()
    {
        // This can be filled by whatever inherits from this. Default does nothing.
    }

    public virtual void invalidChargeAction()
    {
        // This can be filled by whatever inherits from this. Default does nothing.
    }

    #endregion
}
