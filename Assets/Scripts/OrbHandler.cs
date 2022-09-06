using UnityEngine; //Connect to Unity Engine

public class OrbHandler : MonoBehaviour
{
    #region Variables
    [Header("Movement Variables")]
    [Tooltip("Choose how fast you want the orb to travel")]
    [SerializeField] private float _speed = 3f;
    [Header("Component References")]
    [Tooltip("Add the main camera of the scene, or it will be grabbed during start")]
    [SerializeField] private Camera _camera;
    //Private speed float to store proper calculated movement speed
    private float _moveSpeed;
    #endregion
    #region Setup
    private void Start()
    {
        //Assign the main camera to the camera variable
        _camera = Camera.main;
        //Make the rotation of this object match the cameras rotation so it will move in direction camera was facing at spawning
        transform.rotation = _camera.transform.rotation;
    }
    #endregion
    #region Movement
    private void Update()
    {
        //Move speed equals the speed time deltaTime
        _moveSpeed = _speed * Time.deltaTime;
        //Transform the position of the orb in a forward direction multiplied by the speed
        transform.position += transform.forward * _moveSpeed;
        //Retrieve the child game object sphere so we can rotate it
        GameObject _sphere = transform.GetChild(0).gameObject;
        //Rotate the sphere by 5f on the y axis
        _sphere.transform.Rotate(0f,5f,0f);
        //If the orb gets too far away from the player without hitting anything, destroy it
        if (Vector3.Distance(transform.position, _camera.transform.position) > 50f)
        {
            Destroy(this.gameObject);
        }
    }
    //Destroy the object if it runs into another trigger collider
    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }
    #endregion

}
