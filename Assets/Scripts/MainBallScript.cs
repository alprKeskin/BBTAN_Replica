using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> This script manages the position of the main ball <summary>
/// <summary> MainBall <summary>

public class MainBallScript : MonoBehaviour
{

    private Transform mainBallTransformComponent;
    private Vector3 mainBallPosition;

    // Start is called before the first frame update
    void Start() {
        // assign the transform component of the ball
        mainBallTransformComponent = gameObject.GetComponent<Transform>();
        // initialize the position of the ball at the middle of the screen horizontally
        mainBallPosition = new Vector3(0, mainBallTransformComponent.position.y, 0);
        mainBallTransformComponent.position = mainBallPosition;
    }

    public void Move(float apsis) {
        // save the new position of the main ball
        Vector3 newPosition = new Vector3(apsis, mainBallPosition.y, 0);
        // move the main ball to its new position
        gameObject.GetComponent<Transform>().position = newPosition;
        // change the main ball position information in the administrator script
        GameObject.FindWithTag("MainCamera").GetComponent<AdministratorScript>().MainBallPosition = newPosition;
    }
}
