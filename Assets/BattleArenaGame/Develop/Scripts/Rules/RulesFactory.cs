public class RulesFactory
{
	public MaxLifeTimeRule CreateMaxLifeTimeRule(Character character)
	{
		return new MaxLifeTimeRule(character);
	}

	public MainHeroDeadRule CreateMainHeroDeadRule(Character character)
	{
		return new MainHeroDeadRule(character);
	}

	public MaxSpawnedEnemiesRule CreateMaxSpawnedEnemiesRule(ReactiveVariable<int> currentCount)
	{
		return new MaxSpawnedEnemiesRule(currentCount);
	}

	public KilledEnemiesCountRule CreateKilledEnemiesCountRule(ReactiveVariable<int> currentCount, ReactiveVariable<int> maxCount)
	{
		return new KilledEnemiesCountRule(currentCount, maxCount);
	}
}
