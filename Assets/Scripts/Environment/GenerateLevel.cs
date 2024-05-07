using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    public GameObject outdoors;
    public GameObject indoors;
    public int zPos = 50;
    public bool creatingSection = false;
    private int secNum;
    private int totalsections = 0;
    private int countdownchange = 0;
    private bool worldState = false;
    private bool forceIndoors = false;
    private GameObject curObj;
    private int lastSec = 0;

    // Start is called before the first frame update
    void Start() 
    {
        worldState = false;
        totalsections = 0;
        countdownchange = 0;
        lastSec = 0;
        secNum = 0;
        forceIndoors = false;
        if (forceIndoors) { lastSec = -1; }
        GenerateSection();
    }

    // Update is called once per frame
    void Update()
    {
        if (creatingSection == false && totalsections < 5)
        {
            creatingSection = true;
            StartCoroutine(GenerateSection());
        }
    }

    public void NextSection()
    {
        if (creatingSection == false)
        {
            creatingSection = true;
            StartCoroutine(GenerateSection());
        }
    }

    IEnumerator GenerateSection()
    {
        // Will never generate last num, i.e. possible results with (0, 3) is 0, 1, 2
        totalsections++;
        countdownchange++;
        if ((countdownchange > 5 && Random.value > 0.5f) || forceIndoors)
        {
            worldState = !worldState;
            countdownchange = 0;
            forceIndoors = false;
        }
        curObj = outdoors;
        if (worldState)
        {
            curObj = indoors;
        }
        int max = curObj.transform.childCount;
        secNum = Random.Range(1, max);
        if (countdownchange == 0) { secNum = 0; }
        if (lastSec == secNum) { secNum = Mathf.Min(secNum + 1, max - 1); if (secNum == (max - 1)) { secNum = 1; } }
        lastSec = secNum;
        GameObject go;
        go = Instantiate(curObj.transform.GetChild(secNum).gameObject, new Vector3(0, 0, zPos), Quaternion.identity);
        go.transform.name = "Section " + totalsections + "(" + (worldState ? "Indoors " : "Outdoors ") + (secNum+1) + ")";
        go.transform.parent = GameObject.Find("Generated Sections").transform;
        zPos += 50;
        if (totalsections >= 8) { yield return new WaitForSeconds(3); }
        creatingSection = false;
    }
}
