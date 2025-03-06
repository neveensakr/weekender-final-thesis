using UnityEngine;

public class FootstepController : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _footstepInterval = 0.5f;
    private float _nextStepTime = 0f;

    void Update()
    {
        if (Movement.Instance.IsMoving() || WomanAnimationController.Instance.isWalking)
        {
            if (Time.time >= _nextStepTime)
            {
                _audioSource.Play();
                _nextStepTime = Time.time + _footstepInterval;
            }
        }
    }
}
