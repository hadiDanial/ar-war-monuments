using UnityEngine;

/// <summary>
/// A physical projectile representing artillery shells and rifle bullets.
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private int directHitDamage = 2, blastDamage = 1;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField, Range(0f, 10f)] private float blastRadius = 3;
    [SerializeField] private GameObject blowupParticles;
    [SerializeField] private AudioClipList bulletImpactAudioClips;
    private Rigidbody rb;
    private AudioSource audioSource;

    private float timer = 0;
    private float bulletLifespan = 7.5f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * bulletSpeed;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(rb.velocity);
        timer += Time.deltaTime;
        if(timer > bulletLifespan)
            Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Unit directHitUnit = collision.gameObject.GetComponent<Unit>();
        AudioManager.Instance.PlayFromList(transform.position, bulletImpactAudioClips);
        if(blowupParticles != null)
            Instantiate(blowupParticles, transform.position, Quaternion.identity);
        
        if(directHitUnit != null)
            directHitUnit.Damage(directHitDamage);
        if(blastDamage <= 0 || blastRadius <= 0)
        {
            Destroy(gameObject);
            return;
        }
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (Collider col in colliders)
        {
            Unit unit = col.GetComponent<Unit>();
            if(unit != null && unit != directHitUnit)
                unit.Damage(blastDamage);
        }
        Destroy(gameObject);
    }
}
