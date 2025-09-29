using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{

    public int healthRestored;
    public Vector3 rotationSpeed = new Vector3(0,180,0);

    AudioSource pickupSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        pickupSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable)
        {
            bool wasHealed = damageable.Heal(healthRestored);
            if (wasHealed)
            {
                if (pickupSource)
                {
                    AudioSource.PlayClipAtPoint(pickupSource.clip, gameObject.transform.position, pickupSource.volume);
                }
                Destroy(gameObject);
            }
             
        }
    }

    private void Update()
    {
        transform.eulerAngles += rotationSpeed * Time.deltaTime;
    }
}
