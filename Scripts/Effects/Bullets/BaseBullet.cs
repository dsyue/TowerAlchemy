using Godot;
using System;

public partial class BaseBullet : Area2D
{
    [Export] public float Speed = 300f;
    [Export] public float Damage = 30f;
    [Export] public PackedScene HitEffectScene;

    private IEnemy _target;
    private Vector2 _velocity;

    public void Initialize(IEnemy target)
    {
        _target = target;
    }

    public override void _Ready()
    {
        base._Ready();
        BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_target == null || _target.IsDead() || !IsInstanceValid(_target as Node))
        {
            QueueFree();
            return;
        }

        // 计算朝向目标的方向
        var direction = GlobalPosition.DirectionTo(_target.GetPosition());
        _velocity = direction * Speed;
        Position += _velocity * (float)delta;

        // 旋转子弹朝向目标
        Rotation = direction.Angle();
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is IEnemy enemy && enemy == _target)
        {
            enemy.TakeDamage(Damage);

            if (HitEffectScene != null)
            {
                var effect = HitEffectScene.Instantiate<Node2D>();
                GetParent().AddChild(effect);
                effect.GlobalPosition = GlobalPosition;
            }

            QueueFree();
        }
    }
}
