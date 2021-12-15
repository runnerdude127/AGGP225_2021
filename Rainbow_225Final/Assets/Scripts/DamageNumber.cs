using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public float force = 10;
    public float lifetime = 1;
    public int damageAmount = 0;
    public TMP_Text myText;

    Rigidbody2D rb;
    AudioSource source;
    public AudioClip sound;

    private void Awake()
    {
        source = gameObject.GetComponent<AudioSource>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        source.PlayOneShot(sound);
        rb.AddForce(new Vector2((Random.Range(-1f, 1f) * force), force), ForceMode2D.Impulse);
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        myText.text = damageAmount.ToString();     
    }
}
