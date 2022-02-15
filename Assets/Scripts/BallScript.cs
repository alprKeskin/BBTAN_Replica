using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> This script manages the movements of balls <summary>
/// <summary> Ball <summary>

public class BallScript : MonoBehaviour
{

    private const float ballVelocityCoefficient = 800;
    private Vector3 forceDirection;
    private GameObject mainBallGameObject;
    private const float x_deviation = 0.2f;
    private const int minNumberOfBoxesInLevel = 3;
    private const int maxNumberOfBoxesInLevel = 15;

    // Start is called before the first frame update
    void Start() {
        // save the force direction
        forceDirection = GameObject.FindWithTag("MainCamera").GetComponent<AdministratorScript>().DirectionOfArrow;
        // exert the force to the ball
        gameObject.GetComponent<Rigidbody2D>().AddForce(forceDirection * ballVelocityCoefficient);
        // save main ball game object
        mainBallGameObject = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update() {
        // if this ball returned to its initial line
        if (gameObject.GetComponent<Transform>().position.y < mainBallGameObject.GetComponent<AdministratorScript>().MainBallPosition.y) {
            // get administrator script
            AdministratorScript adminScript = GameObject.FindWithTag("MainCamera").GetComponent<AdministratorScript>();
            // increase returned ball number
            adminScript.BallNumberInTravel--;
            // if all balls returned to initial line
            // if ball number in travel is 0 and balls were just in travel
            if (adminScript.BallNumberInTravel == 0 && adminScript.BallsTravel == true) {
                // increase the level
                adminScript.Level++;

                // determine the new number of boxes in this level randomly
                adminScript.NewNumberOfBoxes = Random.Range(minNumberOfBoxesInLevel, maxNumberOfBoxesInLevel);                
                // instantiate boxes of new level
                adminScript.InstantiateBoxesOfLevel();

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

                // balls are not in travel
                adminScript.BallsTravel = false;
            }
    
            // if this ball is the first ball returning to the initial line
            // if only one ball has returned and first ball is in travel
            if (adminScript.BallNumberInTravel == adminScript.BallNumber - 1 && adminScript.FirstBallTravel == true) {
                // move the main ball to that place
                GameObject.FindWithTag("MainBallTag").GetComponent<MainBallScript>().Move(gameObject.GetComponent<Transform>().position.x);
                
                // destroy the ball
                Destroy(gameObject);

                // update the first ball travel information
                adminScript.FirstBallTravel = false;
            }
            
            // if this ball is not the first ball returning to the initial line
            else {
                // set its velocity to 0
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                // if the ball does not intersect with the main ball, and in the right side of the main ball
                if (gameObject.GetComponent<Transform>().position.x > mainBallGameObject.GetComponent<AdministratorScript>().MainBallPosition.x + x_deviation) {
                    gameObject.GetComponent<Transform>().Translate(new Vector3(-0.05f, 0, 0));
                }
                // if the ball does not intersect with the main ball, and in the left side of the main ball
                else if (gameObject.GetComponent<Transform>().position.x < mainBallGameObject.GetComponent<AdministratorScript>().MainBallPosition.x - x_deviation) {
                    gameObject.GetComponent<Transform>().Translate(new Vector3(0.05f, 0, 0));
                }
                // if the ball intersects with the main ball
                else {
                    // destroy the ball
                    Destroy(gameObject);
                }
            }
        }
    }
}
