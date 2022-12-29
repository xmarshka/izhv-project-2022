using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float force;
    public float damage;

    private GameObject player;
    private Rigidbody rb;

    public Vector3 target;

    public void FireAtPlayer()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(force * (player.transform.position - transform.position), ForceMode.Impulse);
    }

    public void FireForward()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(force * (transform.forward), ForceMode.Impulse);
    }

    public void FireAtTarget(Vector3 target)
    {
        RotateTowards(target);
        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(force * (transform.forward), ForceMode.Impulse);
    }

    public void RotateTowards(Vector3 target)
    {
        transform.LookAt(target);
    }

    // Update is called once per frame
    void Start()
    {
        player = GameObject.Find("Player");
        FireAtTarget(target);
        Destroy(gameObject, 2.0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            player.GetComponent<PlayerStatus>().ApplyDamage(damage);
        }
        Destroy(gameObject);  
    }
}
