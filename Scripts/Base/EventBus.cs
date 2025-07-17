// EventBus.cs
using System;

public static class EventBus
{
    public static event Action<IEnemy> OnEnemyDied;

    // 敌人死亡时调用（在Enemy脚本中触发）
    public static void PublishEnemyDeath(IEnemy enemy)
    {
        OnEnemyDied?.Invoke(enemy);
    }
}