using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class SlimeBullet : MonoBehaviour
{
    float bulletDamage;
    new Rigidbody rigidbody;
    AudioSource source;
    bool hitSomething = false;

    public void InitialiseBullet(float bulletSpeed, float bulletDamage)
    {
        this.bulletDamage = bulletDamage;
        rigidbody.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
    }

    float hitTimer = 0;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        hitTimer += Time.deltaTime;
        if (hitTimer > 10)
            Destroy(gameObject);

        if (!hitSomething)
            transform.eulerAngles += 1000F * Time.deltaTime * Vector3.forward; //Spin the slime bullet.
        else Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        hitSomething = true;

        PlaySplatSound();

        Character character = collision.collider.GetComponent<Character>();

        if (character && character is Enemy enemy)
        {
            if (enemy.IsDead)
                Destroy(gameObject);
            else
                enemy.TakeDamage(bulletDamage);

            return;
        }
    }
    private void PlaySplatSound()
    {
        print("should be playing splat sound!");
    }
}