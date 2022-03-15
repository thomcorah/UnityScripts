using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Avatar : MonoBehaviour
{

    /* This script provides simple mouse and keyboard control of our avatar. */

    // We can set the mouse sensitivity and movement speed.
    [SerializeField]
    float mouseSensitivity = 1.0f;

    [SerializeField]
    float movementSpeed =10.0f;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

      /* This allows us to lock the cursor in the game window. When activated (by clicking
       * in the game window), the cursor will be invisible and we will have full direction 
       * control using the mouse or trackpad. 
       */
      Cursor.lockState = CursorLockMode.Locked;

      if(Application.isEditor) {
        /* Unity has built in methods for getting default keyboard control. The user can use
         * either the arrow keys or the WASD keys. In either case, this gets picked up and 
         * communicated via Input.GetAxis. Each of these will have a value of 1, 0, or -1
         * depending on which key is being pressed.
         * Vertical picks up the up and down keys, as well as W and S. Horizontal picks up the 
         * left, right, A and D keys. 
         */
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        /* We've got two tranform lines here. The first will move the avatar forward or
         * backward depending on the value of the vertical input. 
         * We multiple this (1, 0, or -1) by transform.forward to get a normalised vector that
         * represents a forward or backward direction in the object space. 
         * We multiply by Time.deltaTime to account for variations in frame rate. 
         * We multiply by movementSpeed to scale according to our set movement speed.
         */
        transform.position += vertical * transform.forward * Time.deltaTime * movementSpeed;
        transform.position += horizontal * transform.right * Time.deltaTime * movementSpeed;

        // We handle rotation of the avatar by getting the mouse input. 
        float rotAmountX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float rotAmountY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        /* To apply this rotation, we create a new Vector3. We set the z rotation to 0, and then 
         * set the x and y rotation based on what we got from the mouse.
         * This Vector3 then gets used to set the rotation of the object. 
         */
        Vector3 rotation;
        rotation.z = 0;
        rotation.x = -rotAmountY;
        rotation.y = rotAmountX;

        transform.Rotate(rotation);

      } 

    }

}
