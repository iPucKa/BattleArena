using UnityEngine;

public class SpawnView : MonoBehaviour, IInitializable
{
	private const string DissolveKey = "_Dissolve";

	private SkinnedMeshRenderer[] _renderers;

	private ICanSpawn _spawnEntity;

	private bool _isInit;

	public void Initialize()
	{
		_spawnEntity = GetComponentInParent<ICanSpawn>();

		_renderers = GetComponentsInChildren<SkinnedMeshRenderer>();

		_isInit = true;

		UpdateRenderers();
	}

	private void Update()
	{
		if (_isInit == false)
			return;

		UpdateRenderers();
	}

	private void UpdateRenderers()
	{
		if (_spawnEntity.InSpawnProcess(out float elapsedTime))
			SetFloatFor(_renderers, DissolveKey, elapsedTime / _spawnEntity.TimeToSpawn);
		else
			SetFloatFor(_renderers, DissolveKey, 1f);
	}

	private void SetFloatFor(SkinnedMeshRenderer[] renderers, string key, float param)
	{
		foreach (SkinnedMeshRenderer renderer in renderers)
			renderer.material.SetFloat(key, param);
	}
}
