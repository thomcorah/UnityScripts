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
    
    // We create a public camera variable so that we can point this script towards
    // the camera. This is so that we can rotate it. Make sure to drag the camera
    // object onto this variable in the Unity UI.
    public Camera cam;

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

        /* We handle rotation of the avatar by getting the mouse input. 
         * For the mouse, the X axis is horizontal, and the Y axis is vertical.
         *
         * We're going to use the X axis (horizontal) to change the rotation of the avatar
         * object around its Y axis. This means that as we look left and right with the mouse,
         * we are actually turning the avatar left or right. When we then move forward, this will
         * be in the direction we're looking. 
         *
         * The vertical movement of the mouse however (Y axis) will be used to rotate the camera
         * around its X axis, so that we can look up and down. We rotate the camera and not the 
         * avatar.
         * 
         * First up, the horizontal axis.
         */
        
        float rotAmountX = Input.GetAxis("Mouse X") * mouseSensitivity;
        
        /* To apply this rotation, we create a new Vector3. We set the z and x rotation to 0, and then 
         * set the y rotation based on what we got from the mouse.
         * This Vector3 then gets used to set the rotation of the object. 
         */
        Vector3 rotation;
        rotation.z = 0;
        rotation.x = 0;
        rotation.y = rotAmountX;

        transform.Rotate(rotation);
        
        /* Now to rotate the camera to be able to look up and down. 
         * This is the same kind of process, but approached a slightly 
         * different way.
         * A key think to note here is that we ADD the mouse movement
         * to the current rotation.
         */
        
        float rotAmountY = Input.GetAxis("Mouse Y") * mouseSensitivity;
 
        // Get the current camera rotation
        Vector3 camRotation = cam.transform.eulerAngles;
        
        // Add our rotation amount to the x axis
        camRotation.x += -rotAmountY;
        
        // Set the camera rotation to the updated vector3
        cam.transform.eulerAngles = camRotation;

      } 

    }

}
