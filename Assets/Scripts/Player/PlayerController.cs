using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    #region serializefields
    [SerializeField] private float _strength = 5f;
    [SerializeField] private Rigidbody2D _rigidbody;
    #endregion

    #region private fields
    private PlayerInputActions _inputActions;
    private Vector2 _direction;
    #endregion

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
    }

    private void Start()
    {
        _direction = Vector2.zero;
        _rigidbody.velocity = Vector2.zero;
    }

    private void OnEnable()
    {
        _inputActions.GamePlay.Enable();
        _inputActions.GamePlay.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        _inputActions.GamePlay.Jump.performed -= OnJump;
        _inputActions.GamePlay.Disable();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _direction = Vector2.up * _strength;
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        _rigidbody.velocity = _direction;
    }
}
