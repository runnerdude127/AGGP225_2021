using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameActor : MonoBehaviour
{
    #region Health Management

    public int health = 100;
    public int currentHealth;

    public bool takesColorDamage = false;

    #endregion

    #region NameTag Management

    public bool showNameTag;

    public GameObject nameTag;
    public GameObject myNameTag;
    public GameObject nameTagSlot;

    public TextMesh myName;
    public Meter headHealthBar;

    #endregion

    #region Sound and Animation Reference

    public AudioSource source;
    public Animator myAnim;

    #endregion

    public virtual void Awake()
    {
        myNameTag = Instantiate(nameTag, nameTagSlot.transform.position, Quaternion.identity);
        source = gameObject.GetComponent<AudioSource>();
        myAnim = gameObject.GetComponentInChildren<Animator>();
        
        myName = myNameTag.GetComponentInChildren<TextMesh>();
        headHealthBar = myNameTag.GetComponentInChildren<Meter>();

        currentHealth = health;

        headHealthBar.SetMainColor(new Color(0, 1, 0));
        headHealthBar.SetLossColor(new Color(1, 0, 0));
        headHealthBar.SetMax(health);

        if (!showNameTag)
        {
            myNameTag.GetComponentInChildren<Canvas>().enabled = false;
        }
    }

    public virtual void Start()
    {
        myName.text = gameObject.name;
    }

    public virtual void Update()
    {
        myNameTag.transform.rotation = Quaternion.LookRotation(myNameTag.transform.position - Camera.main.transform.position);

        AnimationHandler();
    }

    public void changeHealth(int change)
    {
        if (currentHealth + change < 0)                           // Hit below the minimum (zero)
        {
            currentHealth = 0;
        }
        else if (currentHealth + change > health)                 // Healed below the maximum ('health' variable)
        {
            currentHealth = health;
        }
        else                                                      // Normal change
        {
            currentHealth = currentHealth + change;
        }

        if (change <= 0)                                          // Damaged!
        {
            onDamaged();                                          // run damage processing effects, like damage sounds and animations
        }
        else if (change > 0)                                      // Healed!
        {
            onHealed();                                           // run heal processing effects, like heal sounds and animations
        }

        if (currentHealth <= 0)                                   // Dead!
        {
            healthDepleted();                                     // What we do when dead...
        }

        headHealthBar.SetCurrent(currentHealth);
    }


    //headHealthBar.SetCurrent(ch);

    #region Animation Virtuals
    public virtual void AnimationHandler()
    {
        // This can be filled by whatever inherits from this. Default is nothing.
    }

    #endregion

    #region Health Virtuals
    public virtual void onDamaged()
    {
        // This can be filled by whatever inherits from this. Default is nothing.
    }

    public virtual void onHealed()
    {
        // This can be filled by whatever inherits from this. Default is nothing.
    }

    public virtual void healthDepleted()
    {
        // This can be filled by whatever inherits from this. For now, we'll just have it go back to normal health.
        currentHealth = health;
    }

    #endregion
}
