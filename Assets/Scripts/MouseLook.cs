using UnityEngine; //Connect to Unity Engine

public class MouseLook : MonoBehaviour
{
    #region Variables
    [Header("Movement Variables")]
    [Tooltip("Set the sensitivity of movement for rotating the camera")]
    [SerializeField] private float _sensitivity = 100f;
    [Tooltip("Set the maximum angle for vertical rotation of the camera")]
    [SerializeField] private float _clampAngle = 60f;
    [Header("Component References")]
    [Tooltip("Drag the player object in here, or it will be retrieve on start")]
    [SerializeField] private GameObject _player;
    [SerializeField] private Player _playerClass;
    //Private variables to store the values for vertical and horizontal rotation of the camera
    private float _verticalRotation;
    private float _horizontalRotation;
    #endregion
    #region Setup
    private void Start()
    {
        //If we don't have the player assigned, find it in the scene and assign it
        if (_player == null)
        {
            _player = GameObject.Find("Player");
        }
        //If we don't have the player class assigned, grab it from the player object and assign it
        if (_playerClass == null)
        {
            _playerClass = _player.GetComponent<Player>();
        }
        //Set the vertical and horizontal rotation values to the current x and y rotation of the object
        _verticalRotation = transform.localEulerAngles.x;
        _horizontalRotation = _player.transform.eulerAngles.y;
        //Toggle cursor for playing
        ToggleCursorMode();
    }
    #endregion
    #region Movement
    private void Update()
    {
        //If we click the mouse button and have enough energy we will expend some energy and spawn an attack orb
        if (Input.GetMouseButtonDown(0) && _playerClass.playerEnergy >= 25f)
        {
            _playerClass.playerEnergy -= 25f;
            Instantiate(Resources.Load("Sparks"), transform.position + transform.forward, transform.rotation);
        }
        //If we press the escape key toggle the cursor visibility and lock mode on or off
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorMode();
        }
        //If cursor is locked run the Look method to control rotation
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Look();
        }
        //Draw a ray so we can see which way the player is facing in scene view while game is running
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.green);
    }
    private void Look()
    {
        //Store the mouse movement input into new vertical and horizontal variables
        float mouseVertical = -Input.GetAxis("Mouse Y");
        float mouseHorizontal = Input.GetAxis("Mouse X");
        //Make the rotational values equal the mouse input times our set sensitivity and times it by Time.deltaTime
        _verticalRotation += mouseVertical * _sensitivity * Time.deltaTime;
        _horizontalRotation += mouseHorizontal * _sensitivity * Time.deltaTime;
        //Make sure our vertical rotation does not exceed our max rotation value
        _verticalRotation = Mathf.Clamp(_verticalRotation, -_clampAngle, _clampAngle);
        //Use vertical rotation to rotate the camera up and down
        transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
        //Use horizontal rotation to rotate the player around horizontally
        _player.transform.rotation = Quaternion.Euler(0f, _horizontalRotation, 0f);
    }
    #endregion
    #region Cursor
    private void ToggleCursorMode()
    {
        //Make the visibility of the cursor swap from it's current value
        Cursor.visible = !Cursor.visible;
        //If cursor is not locked then lock it, and unlock it if it is locked
        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    #endregion
}
