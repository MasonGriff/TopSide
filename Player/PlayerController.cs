using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float MoveSpeed, distance = .1f, jumpVelocity;
    Rigidbody myRB;
    Vector3 moveInput;
    Vector3 moveVelocity;
    Quaternion CurrentFacing;
    Quaternion lastFacing;

    public GameObject PlayerModel;
    Animator myAnim, RBAnim;

    public bool isGrounded;
    public bool isCrouching;

    RigidbodyConstraints ResetConstraints, TopConstr, SideConstr;

	// Use this for initialization
	void Start () {
        myRB = GetComponent<Rigidbody>();
        PlayerModel = transform.Find("PlayerModel").gameObject;
        CurrentFacing = transform.rotation;
        lastFacing = CurrentFacing;
        myAnim = PlayerModel.GetComponent<Animator>();
        RBAnim = GetComponent<Animator>();
        ResetConstraints = myRB.constraints;
        SideConstr = ResetConstraints;
        TopConstr = ResetConstraints | RigidbodyConstraints.FreezePositionY;
        
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y <= -15)
        {
            DeathGo();
        }
        if (Input.GetButtonDown("Escape"))
        {
            SceneManager.LoadScene("Title");
        }
        LockModelPosition();
        GroundCheck();
        //upVelocity += Physics.gravity.y * Time.deltaTime;
        if (isGrounded)
        {
            myAnim.SetBool("Falling", false);
            myAnim.SetBool("Jump", false);
        }
        else
        {
            isCrouching = false;
        }
        
        //---Topdown---
        if (CameraMaster.Instance.TopDown)
        {
            if (Input.GetButtonDown("Camera"))
            {
                CameraSwap();
            }
            CurrentFacing = PlayerModel.transform.rotation;
            myRB.useGravity = false;
            myRB.constraints = TopConstr;
            moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
            moveVelocity = moveInput * MoveSpeed;
            isCrouching = false;
            myAnim.SetBool("Crouch", false);

            PlayerModel.transform.rotation = Quaternion.LookRotation(moveInput);
            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                PlayerModel.transform.rotation = CurrentFacing;
            }
           
            
        }
        //---SideScroll---
        else
        {
            myRB.useGravity = true;
            myRB.constraints = SideConstr;
            //myRB.constraints = ResetConstraints;
            //myRB.constraints = SideConstraint;
            
            myRB.velocity = new Vector2(Input.GetAxis("Horizontal") * MoveSpeed, myRB.velocity.y);
            if (Input.GetAxis("Horizontal") > 0)
            {
                Quaternion Rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                PlayerModel.transform.rotation = Rotation;
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                Quaternion Rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                PlayerModel.transform.rotation = Rotation;
            }
            if (isGrounded)
            {

                if (Input.GetButtonDown("Camera"))
                {
                    CameraSwap();
                }

                if (Input.GetAxis("Vertical") < 0)
                {
                    isCrouching = true;
                }
                else
                {
                    isCrouching = false;
                }
                if (Input.GetButtonDown("Jump"))
                {
                    Jump();
                }

            }
        }
        AnimateModel(CameraMaster.Instance.TopDown);
        ColliderAnimation();
	}
    private void FixedUpdate()
    {
        if(CameraMaster.Instance.TopDown)
        {
            myRB.velocity = moveVelocity;
        }
    }


    void AnimateModel(bool IsTop)
    {
        if (IsTop)
        {
            myAnim.SetBool("Falling", false);
            myAnim.SetBool("Crouch", false);
            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                myAnim.SetBool("Walking", false);
            }
            else
            {
                myAnim.SetBool("Walking", true);
            }
            
        }
        else
        {
            if (isGrounded)
            {
                myAnim.SetBool("Falling", false);
                if (Input.GetAxis("Horizontal") == 0)
                {
                    myAnim.SetBool("Walking", false);
                }
                else
                {
                    myAnim.SetBool("Walking", true);
                }
                if (isCrouching)
                {
                    myAnim.SetBool("Crouch", true);

                }
                else
                {
                    myAnim.SetBool("Crouch", false);
                }
            }
            else
            {
                bool IsJump = myAnim.GetBool("Jump");
                if (!IsJump && !myAnim.GetBool("Jump"))
                {
                    myAnim.SetBool("Falling", true);
                }
            }
        }
    }

    void CameraSwap()
    {
        CameraMaster.Instance.TopDown = !CameraMaster.Instance.TopDown;
    }
    void GroundCheck()
    {
        RaycastHit hit;
        //float distance = .1f;
        Vector3 dir = new Vector3(0, -1);

        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void LockModelPosition()
    {
        Vector3 ZeroPos = new Vector3(0, 0, 0);
        PlayerModel.transform.localPosition = ZeroPos;
    }

    void ColliderAnimation()
    {
        if (isCrouching)
        {
            RBAnim.SetBool("Crouch", true);

        }
        else
        {
            RBAnim.SetBool("Crouch", false);
        }
    }
    void Jump()
    {
        myRB.AddForce(transform.up * jumpVelocity);
        myAnim.SetBool("Jump", true);
    }

    public void DeathGo()
    {
        Scene myScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(myScene.name);
    }

}
