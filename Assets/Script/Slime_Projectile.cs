using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Projectile : MonoBehaviour
{
    public Vector3 force;
    private int Bounces = 0;
    public GameObject explosion;

    public Collider colider;

    private Color ExplosColour;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GoThrough());
        Rigidbody rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(transform.forward*force.z, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(other.transform.position, 10);

        foreach (Collider c in colliders)
        {
            Enemy e = c.GetComponent<Enemy>();

            if (e)
            {
                print(e.name);
                e.TakeDamage(100);
            }
        }

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
            gameObject.transform.localScale *= 1.3f;

            if(Bounces >= 10)
            {
                Exploud();
            }
        }
       
    }

    public IEnumerator GoThrough()
    {
        colider.enabled =false;
        yield return new WaitForSeconds(0.1f);
        colider.enabled = true;
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