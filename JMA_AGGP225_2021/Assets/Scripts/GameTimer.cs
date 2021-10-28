using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Photon.Pun;

public class GameTimer : MonoBehaviour
{
    public int timeLimit = 100;
    [SerializeField]
    int currentTime;
    public TextMeshProUGUI myTimer;

    void Start()
    {
        myTimer = PlayerGUI.instance.timerText.GetComponent<TextMeshProUGUI>();

        currentTime = timeLimit;
        myTimer.text = currentTime.ToString();
        StartCoroutine(timerCycle());   
    }

    IEnumerator timerCycle()
    {
        yield return new WaitForSeconds(1f);
        if (PhotonNetwork.IsMasterClient)
        {
            currentTime = currentTime - 1;
            gameObject.GetPhotonView().RPC("timeSet", RpcTarget.AllBufferedViaServer, currentTime);
        }
        if (currentTime > 0)
        {
            StartCoroutine(timerCycle());
        }
    }

    [PunRPC]
    public void timeSet(int timeToSet)
    {
        currentTime = timeToSet;
        updateTime(timeToSet);
    }

    public void updateTime(int timeToSet)
    {
        if (myTimer)
        {
            myTimer.text = timeToSet.ToString();
        }
    }
}
