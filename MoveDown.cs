using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{

    // This very simply moves the object down at a constant rate.
    
    [SerializeField]
    float movementSpeed = 1.0f;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

      if(Application.isEditor) {
        /* We calculate a new position for the object on each frame. 
         * we use transform.up to get a vector that represents one unit
         * up from the object, and subtract this to go down. 
         * We need to multiply this by Time.deltaTime to get a value that
         * takes into account changes in frame rate, and by the movementSpeed
         * in order to scale to different speeds. 
         */
        transform.position -= transform.up * Time.deltaTime * movementSpeed;
      } 

    }

}
