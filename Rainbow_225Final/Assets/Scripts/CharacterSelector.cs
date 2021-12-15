using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterSelector : MonoBehaviour
{
    List<CharacterClass> classList;
    public int currentClass;
    public GameObject classDisplayChar;

    SpriteRenderer classimg;
    public TMP_Text NAME;
    public TMP_Text HP;
    public TMP_Text SPD;
    public TMP_Text JMP;
    public TMP_Text MYS;

    private void Awake()
    {
        classList = PhotonManager.instance.classList;
        currentClass = PhotonManager.instance.classID;
    }

    private void Start()
    {
        classimg = classDisplayChar.GetComponent<SpriteRenderer>();
        UpdateAppearance();
    }

    public void clickRight()
    {
        currentClass++;
        if (currentClass == classList.Count)
        {
            currentClass = 0;
        }
        classimg.sprite = classList[currentClass].sprite;
        UpdateAppearance();
    }

    public void clickLeft()
    {
        currentClass--;
        if (currentClass < 0)
        {
            currentClass = (classList.Count - 1);
        }
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        classimg.sprite = classList[currentClass].sprite;
        NAME.text = classList[currentClass].name;
        HP.text = classList[currentClass].hp.ToString();
        SPD.text = classList[currentClass].speed.ToString();
        JMP.text = classList[currentClass].jumpHeight.ToString();
        MYS.text = classList[currentClass].mystery.ToString();
    }

    public int GetClass()
    {
        return currentClass;
    }
}
