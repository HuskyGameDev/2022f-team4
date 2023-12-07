/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionEnemy : MonoBehaviour
{   
    [SerializeField] public int bossHealthToMove;
    public float TeleportToX;
    public float TeleportToZ;
    private Vector3 TeleportLocation;
    public GameObject bossPrefab;
    private Destructible bossDestructable;
    // Start is called before the first frame update
    void Start()
    {
        TeleportLocation = new Vector3(TeleportToX, 0, TeleportToZ);
        bossDestructable = Instantiate(GameObject.Find("Destructable"));
    }

    // Update is called once per frame
    void Update()
    {
        if(bossDestructable.getCurrHealth() <= bossHealthToMove) {
            transform.SetPositionAndRotation(TeleportLocation, transform.rotation);
        }
    }
}
*/