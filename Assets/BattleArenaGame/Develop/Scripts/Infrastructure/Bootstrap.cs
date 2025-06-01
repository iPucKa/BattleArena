using System.Collections;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
	[SerializeField] private ConfirmPopup _confirmPopup;

	private ControllersUpdateService _controllersUpdateService;

	private GameplayCircle _gameplayCircle;

	private void Awake()
	{
		StartCoroutine(StartProcess());
	}

	private IEnumerator StartProcess()
	{

		MainHeroConfig heroConfig = Resources.Load<MainHeroConfig>("Configs/MainHeroConfig");
		LevelConfig levelConfig = Resources.Load<LevelConfig>("Configs/LevelConfig");

		_controllersUpdateService = new ControllersUpdateService();

		ControllersFactory controllersFactory = new ControllersFactory();
		CharacterFactory charactersFactory = new CharacterFactory();

		MainHeroFactory mainHeroFactory = new MainHeroFactory(_controllersUpdateService, controllersFactory, charactersFactory);
		EnemiesFactory enemiesFactory = new EnemiesFactory(_controllersUpdateService, controllersFactory, charactersFactory);

		EnemySpawner enemiesSpawner = new EnemySpawner(enemiesFactory);		

		_gameplayCircle = new GameplayCircle(
			mainHeroFactory,
			heroConfig,
			levelConfig,
			_confirmPopup,
			enemiesSpawner,
			this);

		//создание каких-то сервисов вспомогательных
		//процесс инициализации рекламных сервисов, аналитики
		//подгрузка настроек
		//загрузка или генерация уровня/окружения
		//другие подготовительные операции

		//симуляция какой-то инициализации и тп.
		yield return new WaitForSeconds(1.5f);

		//подготовка игры
		yield return _gameplayCircle.Prepare();

		//_loadingScreen.Hide();

		//старт игры

		yield return _gameplayCircle.Launch();
	}

	private void OnDestroy()
	{
		_gameplayCircle?.Dispose();
	}

	private void Update()
	{
		_controllersUpdateService?.Update(Time.deltaTime);

		_gameplayCircle?.Update(Time.deltaTime);
	}
}
