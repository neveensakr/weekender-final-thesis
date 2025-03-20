using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Transform _camera;
    
    public static Movement Instance;
    
    private Rigidbody _rigidbody;
    private Animator _animator;
    private CapsuleCollider _collider;
    private bool _movementEnabled;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        if (InputManager.Instance.InputActivated && IntroManager.Instance.CurrentGameMode == GameMode.InteractiveMode)
        {
            if (!_movementEnabled)
            {
                _movementEnabled = true;
                GetComponent<FollowTarget>().enabled = false;
                _collider.enabled = true;
            }
            
            _animator.SetBool("isWalking", _rigidbody.velocity != Vector3.zero);
        }
    }

    private void FixedUpdate()
    {
        if (InputManager.Instance.InputActivated && IntroManager.Instance.CurrentGameMode == GameMode.InteractiveMode)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

            if (inputDirection.magnitude > 0.1f)
            {
                float angleToRotateTo = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0f, angleToRotateTo , 0f);
                Vector3 moveDirection = Quaternion.Euler(0f, angleToRotateTo , 0f) * Vector3.forward;
                moveDirection.y = 0;
                _rigidbody.velocity = moveDirection.normalized * _speed;
            }
            else
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
            }
        }
    }

    public bool IsMoving()
    {
        return _rigidbody.velocity != Vector3.zero;
    }
}
