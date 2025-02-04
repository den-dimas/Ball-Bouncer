using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class Platform : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 moveDirection;

    private PlayerInput input;
    private InputAction moveAction, startAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        input = GetComponent<PlayerInput>();
        moveAction = input.actions.FindAction("Move");
        startAction = input.actions.FindAction("Start");

        Assert.IsNotNull(moveAction);
        Assert.IsNotNull(startAction);

        startAction.performed += OnSpacebarInput;
    }

    private void Update()
    {
        if (GameManager.Instance.State == GameState.CHOOSE_DIRECTION || GameManager.Instance.State == GameState.STARTED)
        {
            moveDirection.x = moveAction.ReadValue<Vector2>().x;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = Time.deltaTime * moveSpeed * moveDirection;
    }

    private void OnSpacebarInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            switch (GameManager.Instance.State)
            {
                case GameState.END:
                    EventBus.Publish<GameStateEvent>(new(GameState.SPAWNING_ENEMIES));
                    break;
                case GameState.CHOOSE_DIRECTION:
                    EventBus.Publish<GameStateEvent>(new(GameState.STARTED));
                    break;
                default:
                    break;
            }
        }
    }

    private void OnDisable()
    {
        moveAction.performed -= OnSpacebarInput;
    }

    public void Reset()
    {
        moveDirection = Vector2.zero;
        rb.velocity = Vector2.zero;
        transform.position = new(0, transform.position.y);
    }
}
