using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { MedKit, Soda }

public class PickupItem : MonoBehaviour
{    
    private Rigidbody rb;
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    public ItemType itemType;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.tag == "Floor")
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;

            GetComponent<Collider>().isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !other.isTrigger)
        {
            playerHealth = other.GetComponent<PlayerHealth>();
            playerMovement = other.GetComponent<PlayerMovement>();
            PickItem();
            Destroy(gameObject);
        }
    }

    public void PickItem()
    {
        switch (itemType) {
            case ItemType.MedKit:
                playerHealth.Heal(20);
                break;
            case ItemType.Soda:
                playerMovement.Buff(6f, 1.8f);
                break;
        }

    }
}
