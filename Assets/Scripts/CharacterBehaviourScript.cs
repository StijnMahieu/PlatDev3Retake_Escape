using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviourScript : MonoBehaviour
{
    public bool AllowMovement = true;
    public bool AllowCameraRotation = true;

    //animations
    private Animator _animator;

    //charactercontroller
    private CharacterController _characterController;

    [SerializeField]
    private GameObject _playerCamera;

    //gameObjects
    [SerializeField]
    private GameObject _camPivot;

    //velocity
    private Vector3 _velocity = Vector3.zero;

    //movement
    private Vector3 _movement = Vector3.zero;

    //jump
    private bool _jump;
    [SerializeField]
    private float _jumpHeight;

    //LocomotionParameters
    [SerializeField]
    public float Acceleration;
    [SerializeField]
    private float _dragOnGround;
    [SerializeField]
    private float _maxRunningSpeed;

    //Cameramultiplier
    private float _cameraMultiplier = 2;

    //Dependencies
    [SerializeField]
    private Transform _absoluteForward;

    //falling
    private bool _isFalling;

    //animation
    private int _horizontalVelocityParameter = Animator.StringToHash("HorizontalVelocity");
    private int _verticalVelocityParameter = Animator.StringToHash("VerticalVelocity");

    private bool _isJumping;

    //health for player
    public int PlayerHealth = 3;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();

        _animator.SetBool("isFalling", false);
    }

    void Update()
    {
        _movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (Input.GetButtonDown("Jump") && !_isJumping)
        {
            _jump = true;
        }

        if(_characterController.isGrounded)
        {
            _animator.SetBool("isJumping", false);
        }

        if (AllowCameraRotation)
        {
            RotateCamera();
        }

        PlayerFalling();
    }

    void FixedUpdate()
    {
        ApplyGravity();
        ApplyGround();
        ApplyMovement();
        ApplyGroundDrag();
        LimitMaximumRunningSpeed();

        if (_characterController.isGrounded)
        {
            ApplyJump();
        }

        if (AllowMovement)
        {
            _characterController.Move(_velocity * Time.deltaTime);
        }

        //animations
        MovementAnimations();
    }

    private void ApplyJump()
    {
        if (_jump && _characterController.isGrounded)
        {
            _velocity += -Physics.gravity.normalized * Mathf.Sqrt(2 * Physics.gravity.magnitude * _jumpHeight);
            _jump = false;
            _animator.SetBool("isJumping",true);
            _isJumping = true;
        }
        else 
        {
            _isJumping = false;
        }
    }

    private void LimitMaximumRunningSpeed()
    {
        Vector3 yVelocity = Vector3.Scale(_velocity, new Vector3(0, 1, 0));
        Vector3 xzVelocity = Vector3.Scale(_velocity, new Vector3(1, 0, 1));

        Vector3 clampedXzVelocity = Vector3.ClampMagnitude(xzVelocity, _maxRunningSpeed);

        _velocity = yVelocity + clampedXzVelocity;
    }

    private void ApplyGroundDrag()
    {
        if (_characterController.isGrounded)
        {
            _velocity = _velocity * (1 - Time.deltaTime * _dragOnGround);
        }
    }

    private void ApplyMovement()
    {
        if (_characterController.isGrounded)
        {
            Vector3 xzAbsoluteForward = Vector3.Scale(_absoluteForward.forward, new Vector3(1, 0, 1));

            Quaternion forwardRotation = Quaternion.LookRotation(xzAbsoluteForward);
            Vector3 relativeMovement = forwardRotation * _movement;

            _velocity += relativeMovement * Acceleration * Time.deltaTime;
        }
    }

    private void ApplyGround()
    {
        if (_characterController.isGrounded)
        {
            _velocity -= Vector3.Project(_velocity, Physics.gravity.normalized);
        }
    }

    private void ApplyGravity()
    {
        _velocity += Physics.gravity * Time.deltaTime;
    }

    private void MovementAnimations()
    {
        //left thumbstick input
        _animator.SetFloat(_horizontalVelocityParameter, _movement.x);
        _animator.SetFloat(_verticalVelocityParameter, _movement.z);
    }

    public void RotateCamera()
    {
        Vector3 tempRot = transform.localEulerAngles;
        tempRot.y += Input.GetAxis("VerticalRight") * _cameraMultiplier;
        transform.localEulerAngles = tempRot;

        Vector3 rotationCamPivot = _camPivot.transform.localEulerAngles;
        rotationCamPivot.z += Input.GetAxis("HorizontalRight") * -_cameraMultiplier;
        if (_playerCamera.gameObject.activeSelf)
        {
            rotationCamPivot.z = ClampAngle(rotationCamPivot.z, -20, 40);
        }
        else
        {
            rotationCamPivot.z = ClampAngle(rotationCamPivot.z, -40, 10);
        }
        _camPivot.transform.localEulerAngles = rotationCamPivot;
    }
    public static float ClampAngle(float angle, float min, float max)
    {
        angle = Mathf.Repeat(angle, 360);
        min = Mathf.Repeat(min, 360);
        max = Mathf.Repeat(max, 360);
        bool inverse = false;
        var tmin = min;
        var tangle = angle;
        if (min > 180)
        {
            inverse = !inverse;
            tmin -= 180;
        }
        if (angle > 180)
        {
            inverse = !inverse;
            tangle -= 180;
        }
        var result = !inverse ? tangle > tmin : tangle < tmin;
        if (!result)
            angle = min;

        inverse = false;
        tangle = angle;
        var tmax = max;
        if (angle > 180)
        {
            inverse = !inverse;
            tangle -= 180;
        }
        if (max > 180)
        {
            inverse = !inverse;
            tmax -= 180;
        }

        result = !inverse ? tangle < tmax : tangle > tmax;
        if (!result)
            angle = max;
        return angle;
    }

    private void PlayerFalling()
    {
        if(_characterController.transform.position.y < 11)
        {
            _animator.SetBool("isFalling", true);
        }
    }
}
