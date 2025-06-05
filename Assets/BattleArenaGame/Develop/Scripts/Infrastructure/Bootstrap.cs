using System.Collections;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
	[SerializeField] private ConfirmPopup _confirmPopup;

	private ControllersUpdateService _controllersUpdateService;

	private GameplayCycle _gameplayCycle;

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

		_gameplayCycle = new GameplayCycle(
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

		//���������� �����
		yield return _gameplayCycle.PrepareScene();		

		//_loadingScreen.Hide();

		//����� ����

		yield return _gameplayCycle.Launch();
	}

	private void OnDestroy()
	{
		_gameplayCycle?.Dispose();
	}

	private void Update()
	{
		_controllersUpdateService?.Update(Time.deltaTime);

		_gameplayCycle?.Update(Time.deltaTime);
	}
}
