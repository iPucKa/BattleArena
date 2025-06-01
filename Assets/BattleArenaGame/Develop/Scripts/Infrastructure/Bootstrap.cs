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

		//�������� �����-�� �������� ���������������
		//������� ������������� ��������� ��������, ���������
		//��������� ��������
		//�������� ��� ��������� ������/���������
		//������ ���������������� ��������

		//��������� �����-�� ������������� � ��.
		yield return new WaitForSeconds(1.5f);

		//���������� ����
		yield return _gameplayCircle.Prepare();

		//_loadingScreen.Hide();

		//����� ����

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
