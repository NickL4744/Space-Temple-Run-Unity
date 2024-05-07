using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementControl : MonoBehaviour
{
    public UnlockableMatrix unlockableMatrix;
    public GameObject diedButton, hundredCoinButton, hundredStepButton;
    // Start is called before the first frame update
    void Start()
    {
        PopulateMatrix();
        RerenderAchievements();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        RerenderAchievements();
    }

    private void RerenderAchievements()
    {
        if (!diedButton) { return;  }
        checkButtons(unlockableMatrix.hasDiedOnce, diedButton);
        checkButtons(unlockableMatrix.hasRunHundredSteps, hundredStepButton);
        checkButtons(unlockableMatrix.hasCollectedHundredCoins, hundredCoinButton);
    }

    private void checkButtons(bool chk, GameObject obj)
    {
        Button btn = obj.transform.Find("Button").GetComponent<Button>();
        if (chk)
        {
            btn.enabled = true;
            btn.interactable = false;
            btn.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            btn.interactable = true;
            btn.enabled = false;
            btn.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.5f);
        }
    }

    public void WipeMatrix()
    {
        unlockableMatrix = new UnlockableMatrix();
        Debug.Log("I wiped it too");
        RerenderAchievements();
    }

    private void PopulateMatrix()
    {
        if (PlayerPrefs.GetInt("hasDiedOnce") == 1)
        {
            unlockableMatrix.hasDiedOnce = true;
        }
        if (PlayerPrefs.GetInt("hasRunHundredSteps") == 1)
        {
            unlockableMatrix.hasRunHundredSteps = true;
        }
        if (PlayerPrefs.GetInt("hasCollectedHundredCoins") == 1)
        {
            unlockableMatrix.hasCollectedHundredCoins = true;
        }
    }
}
