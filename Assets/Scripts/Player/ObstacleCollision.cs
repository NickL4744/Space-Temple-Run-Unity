using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Madhur.InfoPopup;

public class ObstacleCollision : MonoBehaviour
{
    public GameObject thePlayer;
    public GameObject charModel;
    public GameObject camAnim;
    public GameObject levelControl;
    public GameObject alienObject;
    public GameObject panicCanvas;
    public AudioSource crashThud;
    public GameObject panicCounter;
     
    public bool inPanic = false;
    public int panicCount = 0;
    private bool gameOver = false;
    private Vector3 turnRight = new Vector3(0, 25f, 0);

    void Update()
    {
        if (inPanic || gameOver)
        {
            alienObject.transform.localPosition = Vector3.MoveTowards(alienObject.transform.localPosition, new Vector3(0f, -0.51f, -1.14f), Time.deltaTime * 3);
        }
        else
        {
            alienObject.transform.localPosition = Vector3.MoveTowards(alienObject.transform.localPosition, new Vector3(0f, -0.51f, -3f), Time.deltaTime * 3);
        }
    }

    void FixedUpdate()
    {
        if (gameOver) { alienObject.transform.Rotate(turnRight); }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SObstacle") && inPanic == false && panicCount < 3)
        {
            inPanic = true;
            panicCounter.transform.GetChild(panicCount).gameObject.SetActive(true);
            panicCount += 1;
            panicCanvas.SetActive(true);
            crashThud.Play();
            charModel.GetComponent<Animator>().Play("Astronaut_Jump");
            StartCoroutine(Panic());
        }
        else if (other.gameObject.CompareTag("LObstacle") || (other.gameObject.CompareTag("SObstacle") && (inPanic || panicCount == 3)))
        {
            bool jumping = PlayerMovement.jumping;
            gameOver = true;
            PlayerMovement.canMove = false;
            thePlayer.GetComponent<PlayerMovement>().enabled = false;
            StopCoroutine("JumpSequence");
            StopCoroutine("SlideSequence");
            StopCoroutine("Panic");
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            if (jumping)
            {
                charModel.GetComponent<Animator>().Play("Astronaut_AirCrash");
            }
            else
            {
                charModel.GetComponent<Animator>().Play("Astronaut_GroundCrash");
            }
            levelControl.GetComponent<LevelDistance>().enabled = false;
            crashThud.Play();
            camAnim.GetComponent<Animator>().enabled = false;
            levelControl.GetComponent<EndRunSequence>().enabled = true;
            alienObject.GetComponent<Animator>().Play("Idle");
            if (!PlayerMovement.unlockableMatrix.hasDiedOnce)
            {
                PlayerMovement.unlockableMatrix.hasDiedOnce = true;
                PlayerPrefs.SetInt("hasDiedOnce", 1);
                Debug.Log("You died for the first time!");
                InfoPopupUtil.ShowInformation("Achievement Unlocked: First Death!");

            }
        }
    }

    IEnumerator Panic()
    {
        yield return new WaitForSeconds(1);
        charModel.GetComponent<Animator>().Play("Astonaut_Run");
        yield return new WaitForSeconds(9);
        if (!gameOver && panicCount < 3)
        {
            panicCanvas.GetComponent<Animator>().Play("FadeIn");
            inPanic = false;
            yield return new WaitForSeconds(1);
            panicCanvas.SetActive(false);
        }
    }
}
