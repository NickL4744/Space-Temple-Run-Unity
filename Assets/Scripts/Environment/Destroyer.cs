using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    GenerateLevel sn;

    void Start()
    {
        sn = GameObject.Find("LevelControl").GetComponent<GenerateLevel>();
    }
    void Update(){}

    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(DestroyClone());
    }

    IEnumerator DestroyClone()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.transform.parent.gameObject);
        sn.NextSection();
    }
}
