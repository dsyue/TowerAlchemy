using Godot;

public partial class Tower : Node2D
{
    [Export] public float Range = 800f;
    [Export] public float Cooldown = 1f;
    [Export] public PackedScene AttackEffectScene;

    [Export] public PackedScene BulletScene; // 新增子弹场景引用
    [Export] public float BulletSpeed = 800f; // 新增子弹速度

    private AnimatedSprite2D _Animated;

    private float cooldownTimer = 0f;

    public override void _Ready()
    {
        base._Ready();

        // 播放动画
        _Animated = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _Animated.Play();
    }

    public override void _Process(double delta)
    {
        cooldownTimer -= (float)delta;
        if (cooldownTimer <= 0f)
        {
            var target = FindClosestEnemy();
            if (target != null)
            {
                Attack(target);
                cooldownTimer = Cooldown;
            }
        }
    }

    private IEnemy FindClosestEnemy()
    {
        var enemies = GetTree().GetNodesInGroup("enemies");
        IEnemy closest = null;
        float minDist = Range;

        foreach (var node in enemies)
        {
            if (node is IEnemy enemy && !enemy.IsDead())
            {
                float dist = GlobalPosition.DistanceTo(enemy.GetPosition());
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = enemy;
                }
            }
        }

        return closest;
    }

    private void Attack(IEnemy enemy)
    {
        if (BulletScene == null)
        {
            GD.PushWarning("Bullet scene is not assigned!");
            return;
        }

        var bullet = BulletScene.Instantiate<BaseBullet>();
        GetParent().AddChild(bullet);
        bullet.GlobalPosition = GlobalPosition;
        bullet.Initialize(enemy);
        bullet.Speed = BulletSpeed;
    }
}
