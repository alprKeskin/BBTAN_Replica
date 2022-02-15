using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> This script administrates the events in the game <summary>
/// <summary> Main Camera <summary>

public class AdministratorScript : MonoBehaviour
{

    #region Properties
    // mouse button movements
    private Vector3 initialMousePosition;
    private Vector3 currentMousePosition;
    private Vector3 changeInMousePosition;
    private Vector3 directionOfArrow;
    public Vector3 DirectionOfArrow { get { return directionOfArrow; } }
    // main ball movement
    private Vector3 mainBallPosition;
    public Vector3 MainBallPosition { get {return mainBallPosition;} set {mainBallPosition = value;} }
    // dots
    private Transform arrowTransformComponent;
    private Transform aimDotsTransformComponent;
    // arrow appearance
    private const float scaleConstant = 0.01f;
    private const float minimumSizeOfArrow = 50f;
    private const float maximumSizeOfArrow = 400f;
    // balls
    [SerializeField]
    private GameObject ballPrefab;
    private int ballNumber = 51;
    public int BallNumber { get { return ballNumber; } set { ballNumber = value; } }
    private int ballNumberInTravel = 0;
    public int BallNumberInTravel {get {return ballNumberInTravel;} set {ballNumberInTravel = value;}}
    private bool ballsTravel = false;
    public bool BallsTravel { get{return ballsTravel;} set {ballsTravel = value;} }
    private bool firstBallTravel = false;
    public bool FirstBallTravel {get {return firstBallTravel;} set {firstBallTravel = value;}}
    private const float ballInstantiationTimeDelay = 0.1f;
    // boxes
    [SerializeField]
    private GameObject boxPrefab;
    private int newNumberOfBoxes = 3;
    public int NewNumberOfBoxes { get {return newNumberOfBoxes;} set {newNumberOfBoxes = value;} }
    private const float boxInstantiatePositionY = 4.5f;
    private const float boxInstantiatePositionZ = -174f;
    private const float boxInstantiatePositionXLeftThreshold = -8.5f;
    private const float boxInstantiatePositionXRightThreshold = 8.5f;
    private const float decimals = 0.5f;
    private const int numberOfIndexes = 18;
    private bool[] emptynessOfIndexes = new bool[numberOfIndexes];
    // level
    private int level = 1;
    public int Level { get {return level; } set { level = value; } }
    #endregion


    void Awake() {
        // save the arrow transform components
        arrowTransformComponent = GameObject.FindWithTag("ArrowTag").GetComponent<Transform>();

        // save the main ball position
        mainBallPosition = GameObject.FindWithTag("MainBallTag").GetComponent<Transform>().position;
    }

    void Start() {
        // balls should not collide with each other
        Physics2D.IgnoreLayerCollision(6, 6);
        
        // instantiate the boxes for this level
        InstantiateBoxesOfLevel();

        // move all boxes down
        // create an array to put all boxes
        GameObject[] boxes;
        // find all boxes in the scene
        boxes = GameObject.FindGameObjectsWithTag("BoxTag");
        // move down all boxes by 1 unit
        foreach (GameObject box in boxes) {
            // move the box down
            box.GetComponent<BoxScript>().MoveDown();
        }
    }

    // Update is called once per frame
    void Update() {
        // check if the mouse button is clicked down
        if (Input.GetMouseButtonDown(0)) {
            // save the position of the mouse on the screen (In pixel coordinates)
            initialMousePosition = Input.mousePosition;
        }

        // If mouse button is clicking currently
        if (Input.GetMouseButton(0)) {
            // save the current mouse position on the screen (In pixel coordinates)
            currentMousePosition = Input.mousePosition;

            // calculate the change in mouse position (In pixel coordinates)
            changeInMousePosition = new Vector3(initialMousePosition.x - currentMousePosition.x, initialMousePosition.y - currentMousePosition.y, 0);
            
            // calculate the length of the changeInMousePosition
            float lengthOfArrow = Mathf.Sqrt(Mathf.Pow(changeInMousePosition.x, 2) + Mathf.Pow(changeInMousePosition.y, 2));

            // if the size of the arrow is less than the minimum threshold value
            // there will not be an arrow
            if (lengthOfArrow <= minimumSizeOfArrow || lengthOfArrow == 0) {
                // convert the arrow into its initial situtation
                arrowTransformComponent.eulerAngles = new Vector3(0, 0, 0);
                arrowTransformComponent.localScale = new Vector3(2, 0, 1);
            }



            // If the user has sliced his/her hand greater than the minimum threshold value
            // there will be an arrow
            else {
                // DETERMINE THE ROTATION ANGLE
                float rotationAngle;
                // if the arrow will not look under the line
                if (changeInMousePosition.y >= 0) {
                    // if changeInMousePosition is not horizontal
                    if (changeInMousePosition.y != 0) {
                        // find the rotation of mouse position with respect to the y axis (In radians)
                        rotationAngle = Mathf.Atan(changeInMousePosition.x / changeInMousePosition.y);
                        // convert rotationAngle to degrees
                        rotationAngle = 180 * (rotationAngle / Mathf.PI);
                    }
                    // if changeInMousePosition is horizontal (changeInMousePosition.y == 0)
                    else {
                        // if the arrow is looking towards right
                        if (changeInMousePosition.x > 0) {
                            // rotationAngle is 90
                            rotationAngle = 90;
                        }
                        // if the arrow is looking towards left
                        else {
                            // rotationAngle is -90
                            rotationAngle = -90;
                        }
                    }
                }
                // if the arrow will try to look under the line
                else {
                    rotationAngle = 0;
                    lengthOfArrow = 0;
                }
                // CREATE THE ARROW
                // do the rotation
                arrowTransformComponent.eulerAngles = new Vector3(0, 0, -rotationAngle);                    

                // scale the arrow
                // if the size of the arrow is greater than the maximum threshold value
                // the arrow will have a fixed size
                if (lengthOfArrow >= maximumSizeOfArrow) {
                    arrowTransformComponent.localScale = new Vector3(2, maximumSizeOfArrow * scaleConstant, 1);
                }
                else {
                    arrowTransformComponent.localScale = new Vector3(2, lengthOfArrow * scaleConstant, 1);
                }
            }
        }

        // If mouse button is clicked up
        if (Input.GetMouseButtonUp(0)) {
            // Save the direction of arrow
            directionOfArrow = changeInMousePosition;
            directionOfArrow.z = 0;
            directionOfArrow = directionOfArrow.normalized;
            // save the position of the main ball before strike
            Vector3 mainBallPositionBeforeStrike = mainBallPosition;
            // update the ball number
            ballNumber = level;
            // start instantiating balls
            StartCoroutine(BallInstantiator(mainBallPositionBeforeStrike));
            // convert the arrow into its initial situtation
            arrowTransformComponent.eulerAngles = new Vector3(0, 0, 0);
            arrowTransformComponent.localScale = new Vector3(2, 0, 1);
        }
    }

    private IEnumerator BallInstantiator(Vector3 mainBallPositionBeforeStrike) {

        WaitForSeconds wait = new WaitForSeconds(ballInstantiationTimeDelay);
        
        for (int i = 0; i < ballNumber; i++) {
            // instantiate a ball
            Instantiate(ballPrefab, mainBallPositionBeforeStrike, Quaternion.identity);
            // if this is the first ball
            if (i == 0) {
                // update the information about whether the first ball is in travel
                firstBallTravel = true;
            }
            // delay
            yield return wait;
        }
        // Since balls are now in travel, update that information
        ballsTravel = true;
        // since there are ballNumber balls in travel set its data
        ballNumberInTravel = ballNumber;
    }

    public void InstantiateBoxesOfLevel() {
        GameObject newBox;
        int i = 0;
        // put initial number of boxes in random locations on the starting line
        while (i < newNumberOfBoxes) {
            // determine an appropriate random location
            int x_position_int = Random.Range((int)(boxInstantiatePositionXLeftThreshold - decimals), (int)(boxInstantiatePositionXRightThreshold - decimals));
            float boxInstantiatePositionX = (float)x_position_int + decimals;
            int x_position_index = x_position_int + (numberOfIndexes / 2);
            // if that position is not empty
            if (emptynessOfIndexes[x_position_index] == true) {
                // try it one more time
                continue;
            }
            // if that position is empty
            else {
                // instantiate box at that location
                newBox = Instantiate(boxPrefab, new Vector3(boxInstantiatePositionX, boxInstantiatePositionY, boxInstantiatePositionZ), Quaternion.identity);
                // set its tag as "BoxTag" (It may be unnecessary since prefab already has this tag)
                newBox.tag = "BoxTag";
                // add its location to emptyness of indexes array
                emptynessOfIndexes[x_position_index] = true;
                // increase i by one
                i++;
            }
        }
        // since all balls has been instantiated at this level, we can clear emptyness of indexes
        for (int j = 0; j < numberOfIndexes; j++) {
            emptynessOfIndexes[j] = false;
        }
    }
}
