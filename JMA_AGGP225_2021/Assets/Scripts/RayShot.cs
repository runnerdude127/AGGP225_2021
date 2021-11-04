using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;

public class RayShot : MonoBehaviour
{
    public int damage = 10;

    public float lifetime;
    public float currentLife;

    public GameObject hitEffect;
    GameObject myFirer;

    public Material bulletColor;
    public Color color;

    void Start()
    {
        StartCoroutine(ShootHold());
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

    IEnumerator ShootHold()
    {
        yield return new WaitUntil(() => myFirer != null);
        RaycastHit hit;
        currentLife = lifetime;
        Debug.Log("SHOOT");

        if (Physics.Raycast(transform.position, this.transform.forward, out hit, Mathf.Infinity))
        {
            gameObject.transform.localScale = new Vector3(lifetime, lifetime, hit.distance);
            transform.localPosition += (transform.forward * hit.distance / 2);
            Debug.DrawRay(transform.position, this.transform.forward * hit.distance, Color.yellow, 10);
            Instantiate(hitEffect, hit.point, Quaternion.identity);
            
            InGameActor hitActor = hit.transform.gameObject.GetComponentInParent<InGameActor>();
            if (hitActor)
            {
                Debug.Log("Hit an Actor!");
                Character hitCharacter = hit.transform.gameObject.GetComponentInParent<Character>();
                if (hitCharacter)
                {
                    Debug.Log("It's a Character!");
                    PlayerManager playerHit = hit.transform.gameObject.GetComponentInParent<PlayerManager>();
                    if (playerHit)
                    {
                        Debug.Log("A player Character!");

                        //PlayerManager playerHit = hit.transform.gameObject.GetComponent<PlayerManager>();
                        PlayerManager shooter = myFirer.transform.gameObject.GetComponent<PlayerManager>();

                        //playerHit.gameObject.GetPhotonView().RPC("playerHurt", RpcTarget.All, 10, color.r, color.g, color.b);
                        playerHit.playerHurt(damage, color.r, color.g, color.b);
                        if (shooter.gameObject.GetPhotonView().IsMine)
                        {
                            shooter.playHitConfirmSound();
                        }
                    }
                    else
                    {
                        PlayerManager shooter = myFirer.transform.gameObject.GetComponent<PlayerManager>();
                        hitCharacter.changeHealth(damage * -1);
                        if (shooter.gameObject.GetPhotonView().IsMine)
                        {
                            shooter.playHitConfirmSound();
                        }
                    }
                }
                else
                {
                    hitActor.changeHealth(damage * -1);
                }
            }
            Debug.Log("Hit This: " + hit.transform.gameObject.name);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(.5f, .5f, 100);
            Debug.DrawRay(transform.position, this.transform.forward * 1000, Color.white, 10);
            Debug.Log("Did not Hit");
        }
        StartCoroutine(StartLifetime());
    }

    public void parentSet(GameObject parent)
    {
        myFirer = parent;
    }

    public void colorSet(float r, float g, float b)
    {
        bulletColor = gameObject.GetComponent<MeshRenderer>().material;
        Color c = new Color(r, g, b);
        bulletColor.color = c;
        color = c;
    }
}
