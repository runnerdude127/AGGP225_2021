using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class CameraFocus : MonoBehaviour
{
    GameObject currentTarget;
    public Vector3 offset;
    public float smoothing = 0.125f;

    public static CameraFocus instance { get; private set; } // SINGLETON INSTANCE

    void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void FixedUpdate()
    {
        if (currentTarget)
        {
            Vector3 destination = new Vector3(currentTarget.transform.position.x + offset.x, currentTarget.transform.position.y + offset.y, -10);
            Vector3 smooth = Vector3.Lerp(transform.position, destination, smoothing);
            transform.position = smooth;
        }  
    }

    public IEnumerator CameraShake(float time, float power)
    {
        Vector3 origin = transform.localPosition;
        float elapsed = 0.0f;

        if (power > .8)
        {
            power = .8f;
        }

        while (elapsed < time)
        {
            float x = Random.Range(-1f, 1f) * power;
            float y = Random.Range(-1f, 1f) * power;

            transform.localPosition += new Vector3(x, y, 0);
            yield return null;
            elapsed += Time.deltaTime;
            transform.localPosition = origin;
        }
        transform.localPosition = origin;
    }

    public void setTarget(GameObject target)
    {
        currentTarget = target;
    }
}
