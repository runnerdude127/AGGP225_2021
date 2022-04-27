using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class DamagingElement : NetworkBehaviour
{
    public int damage;
    public float hitStun;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject hit = collision.gameObject;
        NetworkIdentity isOnline = hit.GetComponent<NetworkIdentity>();
        if (isOnline == true)
        {
            string hitID = hit.name;
            Debug.Log("online element hit (" + hitID + ")");
            Actor act = collision.gameObject.GetComponentInParent<Actor>();
            if (act)
            {
                Debug.Log("is an actor");
                PlayerMIRROR ply = collision.gameObject.GetComponent<PlayerMIRROR>();
                if (ply)
                {
                    Debug.Log("is a player");
                    if (ply.hasAuthority)
                    {
                        Skill skl = ply.gameObject.GetComponent<Skill>();
                        if (skl)
                        {
                            if (skl.invulnerable == false)
                            {
                                Vector3 knockDir = transform.position - collision.gameObject.transform.position;
                                ply.CmdDoPlayerDamage(knockDir, damage, GameManager.instance.LEVEL, hitStun);
                            }
                        }
                        else
                        {
                            Vector3 knockDir = transform.position - collision.gameObject.transform.position;
                            ply.CmdDoPlayerDamage(knockDir, damage, GameManager.instance.LEVEL, hitStun);
                        }
                    }
                }
                else
                {
                    Vector3 knockDir = transform.position - collision.gameObject.transform.position;
                    StartCoroutine(act.Hurt(knockDir, damage, GameManager.instance.LEVEL, hitStun));
                }
            }
        }
        else
        {
            Actor act = collision.gameObject.GetComponent<Actor>();
            if (act)
            {
                Debug.Log("world element hit");
                Debug.Log("is an actor");

                Vector3 knockDir = transform.position - collision.gameObject.transform.position;
                StartCoroutine(act.Hurt(knockDir, damage, GameManager.instance.LEVEL, hitStun));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hit = collision.gameObject;
        NetworkIdentity isOnline = hit.GetComponent<NetworkIdentity>();
        if (isOnline == true)
        {
            string hitID = hit.name;
            Debug.Log("online element hit (" + hitID + ")");
            Actor act = collision.gameObject.GetComponentInParent<Actor>();
            if (act)
            {
                Debug.Log("is an actor");
                PlayerMIRROR ply = collision.gameObject.GetComponent<PlayerMIRROR>();
                if (ply)
                {
                    Debug.Log("is a player");
                    if (ply.hasAuthority)
                    {
                        Skill skl = ply.gameObject.GetComponent<Skill>();
                        if (skl)
                        {
                            if (skl.invulnerable == false)
                            {
                                Vector3 knockDir = transform.position - collision.gameObject.transform.position;
                                ply.CmdDoPlayerDamage(knockDir, damage, GameManager.instance.LEVEL, hitStun);
                            }
                        }
                        else
                        {
                            Vector3 knockDir = transform.position - collision.gameObject.transform.position;
                            ply.CmdDoPlayerDamage(knockDir, damage, GameManager.instance.LEVEL, hitStun);
                        }
                    }
                }
                else
                {
                    Vector3 knockDir = transform.position - collision.gameObject.transform.position;
                    StartCoroutine(act.Hurt(knockDir, damage, GameManager.instance.LEVEL, hitStun));
                }
            }
        }
        else
        {
            Actor act = collision.gameObject.GetComponent<Actor>();
            if (act)
            {
                Debug.Log("world element hit");
                Debug.Log("is an actor");

                Vector3 knockDir = transform.position - collision.gameObject.transform.position;
                StartCoroutine(act.Hurt(knockDir, damage, GameManager.instance.LEVEL, hitStun));
            }
        }
    }
}
