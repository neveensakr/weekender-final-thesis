using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Transform _camera;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private bool _movementEnabled = false;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    public void EnablePlayerMovement()
    {
        GetComponent<FollowTarget>().enabled = false;
        _movementEnabled = true;
        GetComponent<RigBuilder>().layers[0].active = false;
    }

    void Update()
    {
        if (_movementEnabled)
            _animator.SetBool("isWalking", _rigidbody.velocity != Vector3.zero);
    }

    private void FixedUpdate()
    {
        if (_movementEnabled)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

            if (inputDirection.magnitude > 0.1f)
            {
                float angleToRotateTo = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0f, angleToRotateTo , 0f);
                Vector3 moveDirection = Quaternion.Euler(0f, angleToRotateTo , 0f) * Vector3.forward;
                _rigidbody.velocity = moveDirection.normalized * _speed;
            }
            else
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
            }
        }
    }
}
