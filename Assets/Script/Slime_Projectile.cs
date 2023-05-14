using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Projectile : MonoBehaviour
{
    public Vector3 force;
    private int Bounces = 0;
    public GameObject explosion;

    private Color ExplosColour;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(force, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            Exploud();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint point = collision.contacts[0];
        
        if(point.point.sqrMagnitude >= 100)
        {
            Bounces++;
            ChangeColour();
            gameObject.transform.localScale *= 1.1f;

            if(Bounces >= 10)
            {
                Exploud();
            }
        }
       
    }
    
    private void ChangeColour()
    {

        MeshRenderer mesh = GetComponent<MeshRenderer>();
        ExplosColour = new(Random.Range(0.1f,1), Random.Range(0.1f, 1), Random.Range(0.1f, 1));
        mesh.material.SetColor("_Colour", ExplosColour);

    }
    private void Exploud()
    {
        GameObject Explos = Instantiate(explosion, transform.position, Quaternion.identity);
        ParticleSystem parts = Explos.GetComponentInChildren<ParticleSystem>();
        parts.startSize *= Bounces * 10;
        parts.startColor = ExplosColour;
        Destroy(Explos,4);
        Destroy(gameObject);
        
    }
}