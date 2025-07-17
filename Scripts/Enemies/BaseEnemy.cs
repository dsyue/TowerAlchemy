using Godot;
using System;

public partial class BaseEnemy : CharacterBody2D, IEnemy
{
    [Export] public float MaxHP = 100;
    [Export] public float Speed = 50f;
    [Export] public int GoldReward { get; private set; } = 10;

    protected float currentHP;
    protected Label hpLabel;

    private HealthBar _healthBar;
    private PackedScene _damageNumberScene;
    private Vector2 _healthBarOffset = new Vector2(0, -20);
    private AnimatedSprite2D _Animated;

    public override void _Ready()
    {
        currentHP = MaxHP;
        hpLabel = GetNode<Label>("HpLabel");
        _healthBar = GetNode<HealthBar>("HealthBar");
        _damageNumberScene = GD.Load<PackedScene>("res://Scenes/Effects/DamageNumber.tscn");
        _Animated = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _Animated.Play();
    }


    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        UpdateHPLabel();

        // 更新血条
        if (_healthBar != null)
        {
            _healthBar.UpdateHealth(currentHP, MaxHP);
        }

        // 显示伤害数字
        ShowDamageNumber(damage);

        if (currentHP <= 0)
        {
            EventBus.PublishEnemyDeath(this); // 发布死亡事件
            QueueFree();
        }
    }

    private void ShowDamageNumber(float damage)
    {
        var damageNumber = _damageNumberScene.Instantiate<DamageNumber>();
        AddChild(damageNumber);

        // 随机位置偏移
        var rng = new RandomNumberGenerator();
        rng.Randomize();

        damageNumber.Position = _healthBarOffset + new Vector2(
            rng.RandfRange(-20, 20),
            rng.RandfRange(-30, -40)
        );

        // 判断是否为暴击
        bool isCritical = rng.Randf() < 0.0f; // 0% 暴击率
        if (isCritical) damage *= 1.5f;

        // 显示伤害
        damageNumber.ShowDamage(damage, isCritical);
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = DirectionToCenter() * Speed;
        MoveAndSlide();
    }

    protected Vector2 DirectionToCenter() => (new Vector2(960, 540) - GlobalPosition).Normalized();


    public float GetHP() => currentHP;
    public bool IsDead() => currentHP <= 0;
    public Vector2 GetPosition() => GlobalPosition;

    protected void UpdateHPLabel()
    {
        hpLabel.Text = $"HP: {Mathf.Ceil(currentHP)}";
    }
}
