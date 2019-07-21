using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaster : MonoBehaviour {

    [Tooltip("Side or Top")]
    public string PlayerMode = "Side"; 

    Rigidbody myRB;
    public float MoveSpeedSide = 1.5f;
    public float MoveSpeedTop = 1.5f;

    //RigidbodyConstraints ResetConstraints, SideConstraint,TopConstraint;

    CharacterController MyContr;

    //public Transform groundCheck;
    //public bool grounded = false;
    //public float groundRadius = 0.1f;
    //public LayerMask whatisground;

    // Use this for initialization
    void Start () {
        myRB = GetComponent<Rigidbody>();
        MyContr = GetComponent<CharacterController>();
        /*ResetConstraints = myRB.constraints;
        SideConstraint = RigidbodyConstraints.FreezeRotationZ;
        SideConstraint = RigidbodyConstraints.FreezeRotationY;
        SideConstraint = RigidbodyConstraints.FreezeRotationX;
        TopConstraint = RigidbodyConstraints.FreezeRotationX;
        TopConstraint = RigidbodyConstraints.FreezeRotationZ;
        TopConstraint = RigidbodyConstraints.FreezePositionY;*/
    }
	
	// Update is called once per frame
	void Update () {
        {//--- Generic ---
            if (Input.GetButtonDown("Camera"))
            {
                PerspectiveSwap();
            }

            //--- Side View Relevant ---
            if (PlayerMode == "Side")
            {
                myRB.useGravity = true;
                //movement SideView
                float move = Input.GetAxis("Horizontal");
                //myRB.constraints = ResetConstraints;
                //myRB.constraints = SideConstraint;
                myRB.velocity = new Vector2(move * MoveSpeedSide, myRB.velocity.y);
                

                //jump related
                //grounded = Physics.over(groundCheck.position, groundRadius, whatisground);



            }
            else if (PlayerMode == "Top")
            {
                myRB.useGravity = false;
                float moveHori = Input.GetAxis("Horizontal");
                float moveVert = Input.GetAxis("Vertical");
                float StayY = transform.position.z;
                //myRB.constraints = ResetConstraints;
                //myRB.constraints = TopConstraint;
                //Vector3 MovementTopDown = new Vector3(moveHori, StayY, moveVert);
                //myRB.velocity = new Vector3(MoveSpeedTop * moveHori, StayY, moveVert * MoveSpeedTop);
                //CharacterRotation(moveHori, moveVert);
                float upVelocity = 0f;
                upVelocity += Physics.gravity.y * Time.deltaTime;
                Vector3 movementVec = new Vector3(Input.GetAxis("Vertical"), 0f, Input.GetAxis("Horizontal")).normalized * MoveSpeedTop;
                movementVec = new Vector3(movementVec.x, upVelocity, movementVec.z);

                MyContr.Move(transform.rotation * movementVec * Time.deltaTime);

                

                /*float horizRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
            transform.Rotate(0f, horizRotation, 0f);

            vertRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            vertRotation = Mathf.Clamp(vertRotation, -upDownRange, upDownRange);
            myCamera.transform.localRotation = Quaternion.Euler(vertRotation, 0f, 0f);*/

            }
        }

    }

    void PerspectiveSwap()
    {
        if (PlayerMode == "Side")
        {
            PlayerMode = "Top";
            CameraMaster.Instance.CamSide.SetActive(false);
            CameraMaster.Instance.CamTop.SetActive(true);
        }
        else if (PlayerMode == "Top")
        {
            PlayerMode = "Side";
            CameraMaster.Instance.CamSide.SetActive(true);
            CameraMaster.Instance.CamTop.SetActive(false);
        }
    }

    void CharacterRotation(float Hori, float Vert)
    {
        if (Hori > 0.1 && Vert < .1 && Vert > -.1)
        {
            transform.Rotate(0, 90, 0);
        }
        else if (Hori < -0.1 && Vert < .1 && Vert > -.1)
        {
            transform.Rotate(0, -90, 0);
        }
        else if (Hori > -.1 && Hori < .1 && Vert > .1)
        {
            transform.Rotate(0, 0, 0);
        }
        else if (Hori > -.1 && Hori < .1 && Vert < -.1)
        {
            transform.Rotate(0, 180, 0);
        }
    }
}
