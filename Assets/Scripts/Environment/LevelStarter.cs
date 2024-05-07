using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStarter : MonoBehaviour
{
    public GameObject countDown3;
    public GameObject countDown2;
    public GameObject countDown1;
    public GameObject countDownGo;
    public GameObject fadeIn;

    public AudioSource readyFX;
    public AudioSource goFX;

    void Start(){
        StartCoroutine(CountSequence());
    }

    void Update(){}

    IEnumerator CountSequence(){
        yield return new WaitForSeconds(1);  //FadeIn delay
        countDown3.SetActive(true);
        readyFX.Play();
        yield return new WaitForSeconds(1);
        countDown3.SetActive(false);
        countDown2.SetActive(true);
        readyFX.Play();
        yield return new WaitForSeconds(1);
        countDown2.SetActive(false);
        countDown1.SetActive(true);
        readyFX.Play();
        yield return new WaitForSeconds(1);
        countDown1.SetActive(false);
        countDownGo.SetActive(true);
        goFX.Play();
        PlayerMovement.canMove = true;
        yield return new WaitForSeconds(1);
        countDownGo.SetActive(false);
        
    }
}
