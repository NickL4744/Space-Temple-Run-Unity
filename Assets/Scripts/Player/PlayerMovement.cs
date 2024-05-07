using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    static public float moveSpeed = 5;
    static public bool canMove = false;
    public static bool jumping = false;
    public static bool sliding = false;
    public static bool comingDown = false;
    public static bool gettingup = false;
    public static bool reverseGravity = false;
    public GameObject playerObject;
    public GameObject alienObject;
    public GameObject cameraObject;
    public GameObject mainPlayerObject;
    public GameObject unlockablePlayerObject;

    public static UnlockableMatrix unlockableMatrix = new UnlockableMatrix();

    public Vector3 LeftMiddleRight = new Vector3(-2.5f, 0f, 2.5f);
    private Vector3 turnLeft = new Vector3(0, -25f, 0);
    private Vector3 turnRight = new Vector3(0, 25f, 0);
    private Vector3 turnDirection;
    public static Vector3 floor = new Vector3(-0.6f, 1.25f, -0.5f); //Astronaut, Player, Alien
    public static Vector3 ceiling = new Vector3(-0.6f, 5.295f, -0.5f);
    private Vector3 curfloor;
    public float laneChangeSpeed = 3;
    public static float verticalSpeed = 2;
    [SerializeField]
    private int gravity = 2;
    [SerializeField]
    private bool gravityToggle = false;
    public bool deadToggle = false;

    private int moveTo = 1;
    private Vector3 newLane;
    private bool changinglanes = false;
    private bool jumpPressed = false;
    private bool slidePressed = false;
    // Start is called before the first frame update
    void Start(){
        AchievementControls();
        curfloor = floor; 
        GravityStrength(); 
        moveSpeed = 5; 
        laneChangeSpeed = 3;
        PowerupControls();
        if (unlockableMatrix.hasTurnPerk) { laneChangeSpeed = 6; }
        if (unlockableMatrix.hasModelPerk) { ChangeModel(); }
    }

    // Update is called once per frame
    void Update()
    {
        deadToggle = !canMove;
        if (!canMove) { return; }

        //Debugging button presses for certain things
        if (Input.GetKeyUp(KeyCode.Q) && unlockableMatrix.hasFastPerk) { moveSpeed = Mathf.Min(moveSpeed+1, 10); }
        if (Input.GetKeyUp(KeyCode.E) && unlockableMatrix.hasSlowPerk) { moveSpeed = Mathf.Max(moveSpeed - 1, 1); }
        if (Input.GetKeyUp(KeyCode.Z) && unlockableMatrix.hasGravPerk) { reverseGravity = !reverseGravity; }
        if (Input.GetKeyUp(KeyCode.R) && unlockableMatrix.hasReversePerk) { GravityStrength(); }



        // Move Player Forward
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.World);
        //Easy boolean for all the jump inputs
        jumpPressed = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space));
        //Easy boolean for all the slide inputs
        slidePressed = (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow));
        //If you can't move, don't go any further
        
        
        // Move left button pressed
        if (!changinglanes) { MoveSequence(); }
        // Jump
        if (jumpPressed && !sliding && !jumping) { StartCoroutine(JumpSequence()); }
        //Slide
        if (slidePressed && !jumping && !sliding) { StartCoroutine(SlideSequence()); }
        // If you said you're changing lanes before, do the work to see which way you move
        if (changinglanes) { ChangeLanes(); }
        // If jumping, fight against gravity, if sliding then slide.
        Vector3 mytmp = Vector3.up;
        if (reverseGravity) { mytmp = -mytmp; } 
        if (jumping && !comingDown) { transform.Translate(mytmp * Time.deltaTime * gravity, Space.World); }
        if (sliding) { transform.position = new Vector3(transform.position.x, 0.9f, transform.position.z); }
        // If you're still in the process of changing lanes, angle in that direction, otherwise no rotational angle.
        HandleGravity();
        if (changinglanes)
        {
            transform.position = Vector3.MoveTowards(transform.position, newLane, Time.deltaTime * laneChangeSpeed);
            playerObject.transform.localEulerAngles = turnDirection;
            alienObject.transform.localEulerAngles = turnDirection;
        }
        else {
            playerObject.transform.localEulerAngles = Vector3.zero;
            alienObject.transform.localEulerAngles = Vector3.zero;
        }

    }

    void HandleGravity()
    {
        if (reverseGravity && curfloor != ceiling)
        {
            curfloor = ceiling;
            transform.gameObject.transform.Rotate(0f, 0f, 180.0f, Space.World);
        } else if (!reverseGravity && curfloor != floor)
        {
            curfloor = floor;
            transform.gameObject.transform.Rotate(0f, 0f, 180.0f, Space.World);
        }
        if (comingDown || (!jumping && transform.position.y != curfloor.y))
        {
            Vector3 gotofloor = new Vector3(transform.position.x, curfloor.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, gotofloor, Time.deltaTime * verticalSpeed);
            if (transform.position.y == curfloor.y)
            {
                if (comingDown)
                {
                    comingDown = false;
                    jumping = false;
                }
                if (gettingup) { sliding = false; gettingup = false; }
            }
        }
    }

    void MoveSequence()
    {
        bool goLeft = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow));
        bool goRight = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow));
        if (goLeft && goRight) { return; }
        if (goLeft)
        {
            if (moveTo > 0)
            {
                moveTo--;
                changinglanes = true;
                turnDirection = turnLeft;
                if (reverseGravity) { turnDirection = turnRight; }
            }
            return;
        }
        // Move Right button pressed
        if (goRight)
        {
            if (moveTo <= 2)
            {
                moveTo++;
                changinglanes = true;
                turnDirection = turnRight;
                if (reverseGravity) { turnDirection = turnLeft; }
            }
            return;
        }
    }

    void ChangeLanes()
    {
        if (moveTo == 0 && transform.position.x != LeftMiddleRight.x)
        {
            newLane = new Vector3(LeftMiddleRight.x, transform.position.y, transform.position.z);
            return;
        }

        if (moveTo == 1 && transform.position.x != LeftMiddleRight.y)
        {
            newLane = new Vector3(LeftMiddleRight.y, transform.position.y, transform.position.z);
            return;
        }

        if (moveTo == 2 && transform.position.x != LeftMiddleRight.z)
        {
            newLane = new Vector3(LeftMiddleRight.z, transform.position.y, transform.position.z);
            return;
        }
        changinglanes = false;

    }

    IEnumerator JumpSequence()
    {
        jumping = true;
        float sec = 0.45f;
        float sectwo = 0.4f;
        if (gravityToggle) { sec = 0.8f; sectwo = 1.2f; }
        if (canMove) { playerObject.GetComponent<Animator>().Play("Astronaut_Jump"); }
        alienObject.GetComponent<Animator>().Play("Jump");
        yield return new WaitForSeconds(sec); 
        if (canMove && gravityToggle) { playerObject.GetComponent<Animator>().Play("Astronaut_Jump"); }
            //alienObject.GetComponent<Animator>().Play("Jump");
        comingDown = true;
        yield return new WaitForSeconds(0.4f);
        if (canMove && gravityToggle) { playerObject.GetComponent<Animator>().Play("Astonaut_Run"); }
        yield return new WaitForSeconds(sectwo);
        alienObject.GetComponent<Animator>().Play("Run");
        jumping = false;
        comingDown = false;
        if (canMove) { playerObject.GetComponent<Animator>().Play("Astonaut_Run"); }
    }

    IEnumerator SlideSequence()
    {
        sliding = true;
        transform.localScale = new Vector3(0.8f, 0.75f, 0.8f);
        transform.position = new Vector3(transform.position.x, 0.9f, transform.position.z);
        cameraObject.transform.localPosition = new Vector3(0f, 4f, -4.05f);
        playerObject.transform.localScale = new Vector3(1.5f, 2f, 1.5f);
        alienObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        if (canMove) { playerObject.GetComponent<Animator>().Play("Astronaut_Slide"); }
        alienObject.GetComponent<Animator>().Play("Slide");
        yield return new WaitForSeconds(0.6f);
        gettingup = true;
        yield return new WaitForSeconds(0.45f);
        alienObject.GetComponent<Animator>().Play("Run");
        if (canMove) { playerObject.GetComponent<Animator>().Play("Astonaut_Run"); }
        sliding = false;
        gettingup = false;
        transform.localScale = new Vector3(0.8f, 1.5f, 0.8f);
        transform.position = new Vector3(transform.position.x, 1.25f, transform.position.z);
        cameraObject.transform.localPosition = new Vector3(0f, 1.79f, -4.05f);
        playerObject.transform.localScale = new Vector3(1.5f, 1f, 1.5f);
        alienObject.transform.localScale = new Vector3(1.5f, 0.75f, 1.5f);
    }

    public void GravityStrength()
    {
        gravityToggle = !gravityToggle;
        gravity = 2;
        if (gravityToggle) { gravity = 3; }
    }

    private void PowerupControls()
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
        if(PlayerPrefs.GetInt("modelPerk") == 1)
        {
            unlockableMatrix.hasModelPerk = true;
        }
    }

    private void AchievementControls()
    {
        unlockableMatrix.hasDiedOnce = false;
        unlockableMatrix.hasRunHundredSteps = false;
        unlockableMatrix.hasCollectedHundredCoins = false;
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

    private void ChangeModel()
    {
        mainPlayerObject.SetActive(false);
        unlockablePlayerObject.SetActive(true);
    }
}
