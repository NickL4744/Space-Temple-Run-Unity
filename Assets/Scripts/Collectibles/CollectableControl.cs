using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableControl : MonoBehaviour
{
    public static int coinCount;
    public GameObject coinCountDisplay;
    public GameObject coinEndDisplay;
    private int hiscore;
    private int tCoins;

    // Start is called before the first frame update
    void Start()
    {
        hiscore = PlayerPrefs.GetInt("coinCount");
        tCoins = PlayerPrefs.GetInt("totalCoins");
        coinCount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        coinCountDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "" + coinCount;
        coinEndDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = (coinCount > hiscore) ? "" + coinCount + "  <b><color=red>High Score!</color></b>" : "" + coinCount;
    }

    void OnDisable()
    {
        if (coinCount > hiscore) { PlayerPrefs.SetInt("coinCount", coinCount); }
        PlayerPrefs.SetInt("totalCoins", tCoins + coinCount);
    }
}
