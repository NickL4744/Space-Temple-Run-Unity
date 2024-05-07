using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Madhur.InfoPopup;

public class CollectCoin : MonoBehaviour
{
    public AudioSource coinFX;
    private int addCoin = 1;
    // Start is called before the first frame update
    void Start() {
        if (!coinFX)
        {
            coinFX = this.transform.parent.GetComponent<CollectCoin>().coinFX;
        }
        if (PlayerPrefs.GetInt("doubleCoins") == 1) { addCoin = 2; }
    }
    // Update is called once per frame
    void Update() { }

    void OnTriggerEnter(Collider other)
    {
        coinFX.Play();
        CollectableControl.coinCount += addCoin;
        if (!PlayerMovement.unlockableMatrix.hasCollectedHundredCoins && CollectableControl.coinCount >= 100)
        {
            PlayerMovement.unlockableMatrix.hasCollectedHundredCoins = true;
            PlayerPrefs.SetInt("hasCollectedHundredCoins", 1);
            Debug.Log("Collected 100 Coins!");
            InfoPopupUtil.ShowInformation("Achievement Unlocked: Collected 100 Coins!");

        }
        this.gameObject.SetActive(false);
    }
}
