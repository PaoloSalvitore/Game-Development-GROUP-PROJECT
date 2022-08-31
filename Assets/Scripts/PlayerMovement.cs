using UnityEngine; //Connect to Unity Engine
//Require a character controller for character movement
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Movement Variables")]
    [Tooltip("Set the speed you want the player to move at when walking")]
    [SerializeField] private float _walkSpeed = 5f;
    [Tooltip("Set the speed the player can run at")]
    [SerializeField] private float _runSpeed = 10f;
    [Tooltip("Set the speed the player will move when sneaking around")]
    [SerializeField] private float _crouchSpeed = 3.5f;
    [Tooltip("How high do you want the player to jump?")]
    [SerializeField] private float _jumpHeight = 3f;
    [Tooltip("Set the strength of gravity to adjust how quickly the player will fall back to ground")]
    [SerializeField] private float _gravity = -9.81f;
    [Header("Component References")]
    [Tooltip("Attach the character controller of the player object. It may also be retrieved during start if you forget.")]
    [SerializeField] private CharacterController _charCon;

    //Variables for calculated gravity, movement and jump values
    private float _gravityAcceleration;
    private float _jumpSpeed;
    private float _jumpVelocity;
    private float _moveSpeed;
    //Vector2 to store player input
    private Vector2 _input;
    #endregion
    #region Setup
    private void Start()
    {
        //If no character controller is stored, retrieve it from the gameobject this script is attached to.
        if (_charCon == null)
        {
            _charCon = GetComponent<CharacterController>();
        }
    }
    #endregion
    #region Movement
    private void FixedUpdate()
    {
        //Calculate the gravity by multiplying gravity twice by delta time
        _gravityAcceleration = _gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
        //Adjust the jump speed using the height multiplied by -2f and by our calculated gravity
        _jumpSpeed = Mathf.Sqrt(_jumpHeight * -2f * _gravityAcceleration);
    }
    private void Update()
    {
        //If the player is on the ground we can control its movement
        if (_charCon.isGrounded)
        {
            //Reset jump velocity to 0 so we are not jumping when we don't want to be
            _jumpVelocity = 0f;
            //Store the player inputs in the approopriate axis of the Vector2 variable
            _input.y = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
            _input.x = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
            //Adjust the speed of movement based on whether we are running, crouching or walking
            _moveSpeed = Input.GetKey(KeyCode.LeftShift) ? _runSpeed : Input.GetKey(KeyCode.LeftControl) ? _crouchSpeed : _walkSpeed;
        }
        //Store our input values in a Vector3 transform direction so we move in the correct direction
        Vector3 move = transform.TransformDirection(new Vector3(_input.x, 0, _input.y));
        //Multiply the movement data by the speed and delta time
        move *= _moveSpeed * Time.deltaTime;
        //If we have pushed the jump button make the jump velocity equal the jump speed
        if (Input.GetKeyDown(KeyCode.Space) && _charCon.isGrounded)
        {
            _jumpVelocity = _jumpSpeed;
        }
        //Add our calculated gravity to our jump velocity to allow gravity to bring us back down (this will happen even when not jumping)
        _jumpVelocity += _gravityAcceleration;
        //Our y axis movement should equal the jump velocity
        move.y = _jumpVelocity;
        //Use the character controller to move the character according to the movement value
        _charCon.Move(move);
    }
    #endregion
}
