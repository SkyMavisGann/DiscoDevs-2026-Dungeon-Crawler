using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public CharacterController controller;
    public Transform Camera;
    public float movementSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float deadzone;
    public bool UsingKBAndMouse = true;
    public Vector3 RotateTo;
    public bool isMoving;
    public Vector3 MoveDirection;
    public Button.ButtonClickedEvent SpecialSkillEvent;

    [Header("Jumping")]
    public Transform groundCheck;
    public float jump;
    public bool grounded;
    public float groundCheckDist;
    public float originalStepOffset;
    public float jumpButtonGrace;
    public float? lastGroundedTime;
    public float? jumpButtonPressedTime;

    [Header("Falling")]
    public float fallSpeed;
    public float gravity;

    private bool rotationSet;
    [SerializeField]
    private Transform bankedTransform;

    private bool LockedMovement = false;
    public void LockMovement(bool value)
    {
        LockedMovement = value;
    }


    private float HorizontalAxis { get { return Player.Instance.MovementInputAction.ReadValue<Vector2>().x; } }

    private float VerticalAxis { get { return Player.Instance.MovementInputAction.ReadValue<Vector2>().y; } }

    private bool RunPressed { get { return Input.GetKeyDown(KeyCode.LeftShift); } }

    private bool RunUp { get { return Input.GetKeyUp(KeyCode.LeftShift); } }

    // Start is called before the first frame update
    void Start()
    {
        //SetUpDefaultPlayerControls();
        movementSpeed = walkSpeed;
        grounded = true;
        isMoving = false;
        originalStepOffset = controller.stepOffset;
        rotationSet = false;
        if (Camera == null)
        {
            Camera = UnityEngine.Camera.main.transform;
        }

        Player.Instance.SpecialSkillInput.performed += SpecialSkill;
    }
    private void OnEnable()
    {
        if (Player.Instance != null)
        {
            Player.Instance.SpecialSkillInput.performed += SpecialSkill;
        }
    }

    public void SpecialSkill(InputAction.CallbackContext context)
    {
        SpecialSkillEvent.Invoke();

        

    }

    void OnDisable() // Or OnDestroy if it's a persistent object
    {
        DOTween.KillAll();
        if (Player.Instance != null)
        {
            Player.Instance.SpecialSkillInput.performed -= SpecialSkill;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bankedTransform != null)
        {
            bankedTransform = null;
        }
        if (rotationSet)
        {
            rotationSet = false;
        }

        //Running
        if (RunPressed)
        {
            //Player.Instance.playerAnim.BodyAnim.SetBool("Run", true);
            movementSpeed = runSpeed;

        }
        else if (RunUp)
        {
            //Player.Instance.playerAnim.BodyAnim.SetBool("Run", false);
            movementSpeed = walkSpeed;
        }

        #region Gravity
        GroundCheck();
        Gravity();
        #endregion

            

        //Camera Controls for Third Person
            if (VerticalAxis + HorizontalAxis != 0 && !LockedMovement)
            {
                   

                Vector3 direction = new Vector3(HorizontalAxis, controller.velocity.y, VerticalAxis).normalized;

                if (direction.magnitude >= deadzone)
                {
                    float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.eulerAngles.y;
                    //CameraControls.Instance.angle = angle;
                    RotateTo = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);

                    if (transform != null && transform)
                    {
                        transform.DORotate(RotateTo, 0.25f);
                    }
                        

                    Vector3 TempDirect = transform.forward * movementSpeed;
                    MoveDirection.x = TempDirect.x;
                    MoveDirection.z = TempDirect.z;

                    //if (Player.Instance.playerAnim.BodyAnim.GetBool("Walk") == false)
                    //{
                    //    Player.Instance.playerAnim.BodyAnim.SetBool("Walk", true);
                    //}
                }
            }
            else if (!LockedMovement && ((HorizontalAxis > deadzone || HorizontalAxis < -deadzone) ||
                ((VerticalAxis > deadzone || VerticalAxis < -deadzone))))
            {

                Vector3 direction = new Vector3(HorizontalAxis, controller.velocity.y, VerticalAxis).normalized;

                if (direction.magnitude >= deadzone)
                {
                    float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.eulerAngles.y;
                    //CameraControls.Instance.angle = angle;
                    RotateTo = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
                    transform.eulerAngles = RotateTo;

                    Vector3 TempDirect = transform.forward * movementSpeed;
                    MoveDirection.x = TempDirect.x;
                    MoveDirection.z = TempDirect.z;

                    //if (Player.Instance.playerAnim.BodyAnim.GetBool("Walk") == false)
                    //{
                    //    Player.Instance.playerAnim.BodyAnim.SetBool("Walk", true);
                    //}
                }
            }
            else
            {
                if (isMoving != false)
                {
                    isMoving = false;
                }

                MoveDirection.x = 0f;
                MoveDirection.z = 0f;
                //RB.velocity = new Vector3(0, RB.velocity.y, 0);
                //Player.Instance.playerAnim.BodyAnim.SetBool("Walk", false);
                //Debug.Log("Not walking");
            }

            MoveDirection.y = fallSpeed;

            controller.Move(MoveDirection * Time.deltaTime);

    }

    public void Gravity()
    {
        if (grounded)
        {
            fallSpeed = -0.5f;
        }
        else
        {
            fallSpeed -= gravity * Time.deltaTime;

            //if (controller.velocity.y < 0f)
            //{
            //    if (Player.Instance.playerAnim.BodyAnim.GetBool("Jump") != false)
            //    {
            //        Player.Instance.playerAnim.BodyAnim.SetBool("Jump", false);
            //    }
            //    if (Player.Instance.playerAnim.BodyAnim.GetBool("Fall") != true)
            //    {
            //        Player.Instance.playerAnim.BodyAnim.SetBool("Fall", true);
            //    }
            //}

        }

    }

    public void GroundCheck()
    {

        if (controller.isGrounded)
        {
            lastGroundedTime = Time.time;
            //if (Player.Instance.playerAnim.BodyAnim.GetBool("Fall") != false)
            //{
            //    Player.Instance.playerAnim.BodyAnim.SetBool("Fall", false);
            //}
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    //public void SetUpDefaultPlayerControls()
    //{
    //    playerControls = PlayerActions.CreateWithPlayerMovementKeyBindings();

    //}
}
