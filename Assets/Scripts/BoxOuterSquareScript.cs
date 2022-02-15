using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOuterSquareScript : MonoBehaviour
{

    #region Properties
    private GameObject healthNumberGameObject;
    private int healthNumber;
    private GameObject boxGameObject;
    #endregion

    // Start is called before the first frame update
    void Start() {
        // save health number game object
        healthNumberGameObject = GetHealthNumberGameObject(gameObject);
        // set the health number information as the level number initially
        healthNumber = GameObject.FindWithTag("MainCamera").GetComponent<AdministratorScript>().Level;
        // set the health number as the level number initially
        healthNumberGameObject.GetComponent<UnityEngine.UI.Text>().text = healthNumber.ToString();
        // save box game object (parent of this outer box game object)
        boxGameObject = gameObject.GetComponent<Transform>().parent.gameObject;
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // if collision happened with a ball
        if (collision.gameObject.tag == "FirstBallTag" || collision.gameObject.tag == "BallTag" || collision.gameObject.tag == "LastBallTag") {
            // update the health number information
            healthNumber--;
            // decrease the health number
            healthNumberGameObject.GetComponent<UnityEngine.UI.Text>().text = healthNumber.ToString();
            // if the health is 0
            if (healthNumber == 0) {
                // destroy that box
                Destroy(boxGameObject);
            }
        }
    }

    public GameObject GetChildWithName(GameObject obj, string name) {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);    
        if (childTrans != null) {
            return childTrans.gameObject;
        }
        return null;
    }

    public GameObject GetHealthNumberGameObject(GameObject obj) {
        // find health number
        GameObject innerSquare = GetChildWithName(obj, "InnerSquare");
        GameObject canvas = GetChildWithName(innerSquare, "Canvas");
        GameObject healthNumber = GetChildWithName(canvas, "HealthNumber");
        // if health number could not be found
        if (healthNumber == null) {
            Debug.Log("HealthNumber game object could not be found!");
            return null;
        }
        // if health number could be found
        return healthNumber;
    }
}
