public interface ICanSpawn
{
	float TimeToSpawn { get; }
	bool InSpawnProcess(out float elapsedTime);
}
