using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreControl : MonoBehaviour
{
    public UnlockableMatrix unlockableMatrix;
    public GameObject coinButton, turnButton, slowButton, fastButton, gravButton, revButton, modelButton;

    private int coins = 0;
    public GameObject store_Coin;
    private TMP_Text store_CoinText;

    public GameObject achievementcontrol;
    // Start is called before the first frame update
    void Start()
    {
        PopulateMatrix();
        RerenderShop();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1)) { coins += 1000; PlayerPrefs.SetInt("totalCoins", coins); Debug.Log("Adding 1k"); }
    }

    void FixedUpdate()
    {
        store_CoinText.text = "" + coins;
        RerenderShop();
    }

    public void BuyCoin()
    {
        if (coins >= 500)
        {
            coins -= 500;
            PlayerPrefs.SetInt("totalCoins", coins);
            PlayerPrefs.SetInt("doubleCoins", 1);
            unlockableMatrix.hasDoubleCoinPerk = true;
        }
        RerenderShop();
    }

    public void BuyTurn()
    {
        if (coins >= 1000)
        {
            coins -= 1000;
            PlayerPrefs.SetInt("totalCoins", coins);
            PlayerPrefs.SetInt("turnPerk", 1);
            unlockableMatrix.hasTurnPerk = true;
        }
        RerenderShop();
    }

    public void BuySlow()
    {
        if (coins >= 2000)
        {
            coins -= 2000;
            PlayerPrefs.SetInt("totalCoins", coins);
            PlayerPrefs.SetInt("slowPerk", 1);
            unlockableMatrix.hasSlowPerk = true;
        }
        RerenderShop();
    }

    public void BuyFast()
    {
        if (coins >= 3000)
        {
            coins -= 3000;
            PlayerPrefs.SetInt("totalCoins", coins);
            PlayerPrefs.SetInt("fastPerk", 1);
            unlockableMatrix.hasFastPerk = true;
        }
        RerenderShop();
    }

    public void BuyGrav()
    {
        if (coins >= 5000)
        {
            coins -= 5000;
            PlayerPrefs.SetInt("totalCoins", coins);
            PlayerPrefs.SetInt("gravPerk", 1);
            unlockableMatrix.hasGravPerk = true;
        }
        RerenderShop();
    }

    public void BuyRev()
    {
        if (coins >= 10000)
        {
            coins -= 10000;
            PlayerPrefs.SetInt("totalCoins", coins);
            PlayerPrefs.SetInt("revPerk", 1);
            unlockableMatrix.hasReversePerk = true;
        }
        RerenderShop();
    }
    public void BuyModel()
    {
        if(coins >= 100000)
        {
            coins -= 100000;
            PlayerPrefs.SetInt("totalCoins", coins);
            PlayerPrefs.SetInt("modelPerk", 1);
            unlockableMatrix.hasModelPerk = true;
        }
    }

    private void RerenderShop()
    {
        store_CoinText = store_Coin.GetComponent<TMP_Text>();
        coins = PlayerPrefs.GetInt("totalCoins");
        checkButtons(unlockableMatrix.hasDoubleCoinPerk, coinButton);
        checkButtons(unlockableMatrix.hasTurnPerk, turnButton);
        checkButtons(unlockableMatrix.hasSlowPerk, slowButton);
        checkButtons(unlockableMatrix.hasFastPerk, fastButton);
        checkButtons(unlockableMatrix.hasGravPerk, gravButton);
        checkButtons(unlockableMatrix.hasReversePerk, revButton);
        checkButtons(unlockableMatrix.hasModelPerk, modelButton);
    }

    public void WipeMatrix()
    {
        unlockableMatrix = new UnlockableMatrix();
        achievementcontrol.GetComponent<AchievementControl>().WipeMatrix();
        RerenderShop();
    }

    public void PopulateMatrix()
    {
        if (PlayerPrefs.GetInt("doubleCoins") == 1)
        {
            unlockableMatrix.hasDoubleCoinPerk = true;
        }
        if (PlayerPrefs.GetInt("turnPerk") == 1)
        {
            unlockableMatrix.hasTurnPerk = true;
        }
        if (PlayerPrefs.GetInt("slowPerk") == 1)
        {
            unlockableMatrix.hasSlowPerk = true;
        }
        if (PlayerPrefs.GetInt("fastPerk") == 1)
        {
            unlockableMatrix.hasFastPerk = true;
        }
        if (PlayerPrefs.GetInt("gravPerk") == 1)
        {
            unlockableMatrix.hasGravPerk = true;
        }
        if (PlayerPrefs.GetInt("revPerk") == 1)
        {
            unlockableMatrix.hasReversePerk = true;
        }
        if (PlayerPrefs.GetInt("modelPerk") == 1)
        {
            unlockableMatrix.hasReversePerk = true;
        }
    }

    private void checkButtons(bool chk, GameObject obj)
    {
        Button btn = obj.transform.Find("Button").GetComponent<Button>();
        Transform go = obj.transform.Find("Button").transform;
        if (chk)
        {
            btn.interactable = false;
            go.Find("CoinSprite").gameObject.SetActive(false);
            go.Find("CoinText").gameObject.SetActive(false);
        } else
        {
            btn.interactable = true;
            go.Find("CoinSprite").gameObject.SetActive(true);
            go.Find("CoinText").gameObject.SetActive(true);
        }
    }
}
