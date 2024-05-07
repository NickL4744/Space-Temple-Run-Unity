using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideGravity : MonoBehaviour
{
    public GameObject playerObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("changing gravity value");
        PlayerMovement.reverseGravity = false;
        playerObject.GetComponent<PlayerMovement>().GravityStrength();
    }
}
