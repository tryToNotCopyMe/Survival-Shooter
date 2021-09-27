using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    private static PickupManager _instance;    

    public static PickupManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PickupManager>();
            }

            return _instance;
        }
    }

    private float baseChance = 1f;
    public GameObject[] pickupPrefabs;
    public float spawnItemChance = 1f;

    public void SpawnPickupItem(Transform spawnPos)
    {
        float rand = Random.Range(0f, 1f);
        if (rand <= spawnItemChance)
        {
            spawnItemChance = baseChance;
            int pickupIndex = Random.Range(0, pickupPrefabs.Length);
            GameObject pickupItem = Instantiate(pickupPrefabs[pickupIndex], spawnPos.position + Vector3.up, spawnPos.rotation);
            pickupItem.GetComponent<Rigidbody>().velocity = Vector3.one;
        } else
        {
            spawnItemChance += 0.02f;
        }
        
    }
}
