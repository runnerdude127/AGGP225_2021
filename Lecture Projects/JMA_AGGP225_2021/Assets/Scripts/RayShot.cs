using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;

public class RayShot : MonoBehaviour
{
    public float lifetime;
    public float currentLife;

    public GameObject hitEffect;

    public Material bulletColor;
    public Color color;

    void Start()
    {
        RaycastHit hit;
        currentLife = lifetime;
        Debug.Log("SHOOT");
        
        if (Physics.Raycast(transform.position, this.transform.forward, out hit, Mathf.Infinity))
        {
            gameObject.transform.localScale = new Vector3(lifetime, lifetime, hit.distance);
            transform.localPosition += (transform.forward * hit.distance/2);
            Debug.DrawRay(transform.position, this.transform.forward * hit.distance, Color.yellow, 10);
            Instantiate(hitEffect, hit.point, Quaternion.identity);
            Debug.Log("Did Hit: " + hit.transform.gameObject.name);
            if (hit.transform.gameObject.tag == "Player")
            {
                Debug.Log("BINGO");
                //source.PlayOneShot(hitConfirm);
                PlayerManager playerHit = hit.transform.gameObject.GetComponent<PlayerManager>();
                {
                    if (playerHit)
                    {
                        //playerHit.gameObject.GetPhotonView().RPC("playerHurt", RpcTarget.All, 10, 0, 0, 0);
                        playerHit.playerHurt(10, color.r, color.g, color.b);
                    }
                    else
                    {
                        Debug.Log("This guy has no PlayerManager.");
                    }
                }
            }
        }
        else
        {
            gameObject.transform.localScale = new Vector3(.5f, .5f, 100);
            Debug.DrawRay(transform.position, this.transform.forward * 1000, Color.white, 10);
            Debug.Log("Did not Hit");
        }
        StartCoroutine(StartLifetime());
    }

    IEnumerator StartLifetime()
    {
        yield return new WaitForSeconds(.01f);
        if (currentLife <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            currentLife = currentLife - .01f;
            gameObject.transform.localScale = new Vector3(currentLife, currentLife, transform.localScale.z);
            StartCoroutine(StartLifetime());
        }       
    }

    [PunRPC]

    void colorSet(float r, float g, float b)
    {
        bulletColor = gameObject.GetComponent<MeshRenderer>().material;
        Color c = new Color(r, g, b);
        bulletColor.color = c;
        color = c;
    }
}
