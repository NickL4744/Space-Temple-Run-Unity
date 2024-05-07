using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravtrigger : MonoBehaviour
{
    public GameObject playerObject;
    private GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = playerObject.transform.Find("Main Camera").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("reversing gravity");
        PlayerMovement.reverseGravity = !PlayerMovement.reverseGravity;
    }
}
