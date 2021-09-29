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


    // menonaktifkan gravity pickup item ketika menyentuh tanah dan menyalakan trigger colldiernya
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.tag == "Floor")
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;

            GetComponent<Collider>().isTrigger = true;
        }
    }

    // mengecek kalau trigger pickup item dimasuki oleh collider player yang tidak memiliki trigger(capsule), 
    // maka akan memberikan player buff/heal
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
                playerHealth.Heal(20); // shouldn't be hardcode this
                break;
            case ItemType.Soda:
                playerMovement.Buff(6f, 1.8f); // this too
                break;
        }

    }
}
