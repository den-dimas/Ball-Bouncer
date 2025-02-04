using UnityEngine;

public class Player : MonoBehaviour
{
	[Header("Stats")]
	public int health = 3;

	[Header("Initial Positions")]
	public float initialHeight = 0.2f;

	[Header("Player Controls")]
	public float initialMoveSpeed = 400.0f;
	public float moveSpeed;

	[Header("Shoot Indicator")]
	[SerializeField, Range(0, 1)]
	private float lineRendererLength;

	private Rigidbody2D rb;
	private Vector2 velocity;
	private Platform platform;
	private LineRenderer lr;

	private void Awake()
	{
		moveSpeed = initialMoveSpeed;
		rb = GetComponent<Rigidbody2D>();
		lr = GetComponent<LineRenderer>();

		lr.positionCount = 2;
		lr.SetPosition(0, transform.position);
	}

	private void Start()
	{
		platform = FindAnyObjectByType<Platform>();

		transform.position = (Vector2)platform.transform.position + new Vector2(0, initialHeight);
	}

	private void Update()
	{
		if (GameManager.Instance.State == GameState.STARTED &&
			Utils.WorldToViewport(transform.position).y < 0)
		{
			health--;

			if (health > 0)
			{
				platform.Reset();
				Reset();
				EventBus.Publish<GameStateEvent>(new(GameState.CHOOSE_DIRECTION));
			}
			else
			{
				EventBus.Publish<GameStateEvent>(new(GameState.FINISHED));
			}
		}

		if (GameManager.Instance.State == GameState.CHOOSE_DIRECTION)
		{
			velocity = new(
				Mathf.Lerp(-1f, 1f, Mathf.PingPong(Time.time / 2f, 1f)),
				Mathf.Lerp(0.018f, 1f, Mathf.PingPong(Time.time, 1f))
			);

			transform.position = new(platform.transform.position.x, transform.position.y);

			lr.SetPosition(0, (Vector2)transform.position);
			lr.SetPosition(1, (Vector2)transform.position + velocity.normalized * lineRendererLength);
		}
	}

	private void FixedUpdate()
	{
		if (GameManager.Instance.State == GameState.STARTED)
		{
			rb.velocity = moveSpeed * Time.deltaTime * velocity.normalized;
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		velocity = Vector2.Reflect(velocity, other.contacts[0].normal);

		EventBus.Publish<PlayerBounceEvent>(new());
	}

	private void OnGameStateChanged(GameStateEvent stateEvent)
	{
		switch (stateEvent.state)
		{
			case GameState.CHOOSE_DIRECTION:
				velocity = new(-1f, 0.018f);
				lr.SetPosition(0, (Vector2)transform.position);
				lr.SetPosition(1, (Vector2)transform.position + velocity.normalized * lineRendererLength);

				lr.enabled = true;
				break;
			case GameState.STARTED:
				lr.enabled = false;
				break;
			case GameState.END:
				platform.Reset();
				Reset();
				break;
			case GameState.FINISHED:
				platform.Reset();
				Reset();
				break;
			default:
				break;
		}
	}

	private void OnEnable()
	{
		EventBus.Subscribe<GameStateEvent>(OnGameStateChanged);
	}

	private void OnDisable()
	{
		EventBus.Unsubscribe<GameStateEvent>(OnGameStateChanged);
	}

	private void Reset()
	{
		velocity = Vector2.zero;
		rb.velocity = Vector2.zero;
		moveSpeed = initialMoveSpeed;

		transform.position = new(platform.transform.position.x, platform.transform.position.y + initialHeight);
	}
}
