using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] 
    private Camera camera;

    private Rigidbody rigidbody;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 rotationCamera = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void RotateCam(Vector3 _rotationCamera)
    {
        rotationCamera = _rotationCamera;
    }

    private void FixedUpdate()
    {
        PerformMove();
    }

    void PerformMove()
    {
        if (velocity != Vector3.zero)
        {
            rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
        }
    }

    void PerformRotation()
    {
        rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(rotation));
        if (camera != null)
        {
            camera.transform.Rotate(rotationCamera);
        }
    }
}
