using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{

    #region Properties
    private bool isMoveDown = false;
    private Vector3 initialPosition;
    private Vector3 endPosition;
    private float lerp = 0;
    private const float moveDuration = 0.2f;
    #endregion

    public void MoveDown() {

        // set the information about whether the box is moving down right now
        isMoveDown = true;
        // set initial position as that position
        initialPosition = gameObject.transform.position;
        // set end position as the position that is 1 unit bottom of the box's position
        endPosition = new Vector3(initialPosition.x, initialPosition.y - 1, initialPosition.z);
        // set lerp to 0 for a smooth and complete move
        lerp = 0;
    }

    void Update() {
        if (isMoveDown == true) {
            // if move has finished
            if (gameObject.transform.position == endPosition) {
                // stop moving
                isMoveDown = false;
            }
            // if move has not finished
            else {
                // update lerp
                lerp += Time.deltaTime / moveDuration;
                // move the box
                gameObject.transform.position = Vector3.Lerp(initialPosition, endPosition, lerp);
            }
        }
    }
}
