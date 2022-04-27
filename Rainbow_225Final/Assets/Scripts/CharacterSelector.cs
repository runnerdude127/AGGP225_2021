using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    List<CharClass> classList;
    public int currentClass;
    public GameObject classDisplayChar;

    Image classimg;
    public TMP_Text NAME;
    public TMP_Text HP;
    public TMP_Text SPD;
    public TMP_Text JMP;
    public TMP_Text MYS;

    private void Awake()
    {
        classList = RainbowNetwork.instance.classList;
        currentClass = RainbowNetwork.instance.classID;
        classimg = classDisplayChar.GetComponent<Image>();
    }

    private void Start()
    {
        UpdateAppearance();
    }

    public void clickRight()
    {
        currentClass++;
        if (currentClass == classList.Count)
        {
            currentClass = 0;
        }
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
        HP.text = classList[currentClass].health.ToString();
        SPD.text = classList[currentClass].speed.ToString();
        JMP.text = classList[currentClass].jumpHeight.ToString();
        MYS.text = classList[currentClass].unknownStat.ToString();
    }

    public int GetClass()
    {
        return currentClass;
    }
}
