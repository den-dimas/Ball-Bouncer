using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class LevelManager : Singleton<LevelManager>
{
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private Image image;

	override protected void Start()
	{
		Initialize();

		Assert.IsNotNull(image, "Image Component is null.");
		Assert.IsNotNull(animator, "Animator Component is null.");

		image.enabled = false;
		animator.enabled = false;
	}

	IEnumerator LoadSceneAsync(string sceneName)
	{
		image.enabled = true;
		animator.enabled = true;

		yield return new WaitForSeconds(1);

		animator.SetTrigger("endTransition");

		SceneManager.LoadScene(sceneName);

		yield return new WaitForSeconds(1);

		animator.enabled = false;
		image.enabled = false;
	}

	public void LoadScene(string sceneName)
	{
		StartCoroutine(LoadSceneAsync(sceneName));
	}

	private void OnEnable()
	{
		EventBus.Subscribe<SceneEvent>(OnSceneChanged);
		EventBus.Subscribe<GameStateEvent>(OnGameStateChanged);
	}

	private void OnDisable()
	{
		EventBus.Unsubscribe<SceneEvent>(OnSceneChanged);
		EventBus.Unsubscribe<GameStateEvent>(OnGameStateChanged);
	}

	private void OnSceneChanged(SceneEvent sceneEvent)
	{
		LoadScene(sceneEvent.sceneName);
	}

	private void OnGameStateChanged(GameStateEvent stateEvent)
	{
		if (stateEvent.state == GameState.FINISHED)
		{
			EventBus.Publish<SceneEvent>(new("MainMenu"));
			EventBus.Publish<GameStateEvent>(new(GameState.END));
		}
	}
}
