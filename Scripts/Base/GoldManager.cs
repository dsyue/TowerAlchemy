// GoldManager.cs
using Godot;
using System;

public partial class GoldManager : Node
{
    public static GoldManager Instance { get; private set; }

    private int _currentGold = 100; // 初始金币
    public int CurrentGold => _currentGold;

    public event Action<int> OnGoldChanged; // 金币变化事件

    private Label _goldLabel;

    public override void _Ready()
    {
        if (Instance == null) Instance = this;
        else QueueFree(); // 确保单例唯一性

        Instance.OnGoldChanged += UpdateUI;
        _goldLabel = GetNode<Label>("Label");
        UpdateUI(Instance.CurrentGold);

        // 事件总线
        EventBus.OnEnemyDied += enemy => AddGold(enemy.GoldReward);
    }

    // 增加金币（敌人死亡调用）
    public void AddGold(int amount)
    {
        _currentGold += amount;
        OnGoldChanged?.Invoke(_currentGold);
    }

    // 消费金币（建塔/升级调用）
    public bool SpendGold(int cost)
    {
        if (_currentGold < cost) return false;

        _currentGold -= cost;
        OnGoldChanged?.Invoke(_currentGold);
        return true;
    }

    private void UpdateUI(int newGold)
    {
        _goldLabel.Text = $"Gold: {newGold}";
    }
}