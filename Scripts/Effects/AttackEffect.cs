using Godot;

public partial class AttackEffect : Node2D
{
    private AnimatedSprite2D _Animated;
    public override void _Ready()
    {
        // 播放动画
        //_Animated = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        //_Animated.Play();

        var tween = CreateTween();
        tween.TweenProperty(this, "scale", Vector2.One * 2, 0.2f).SetTrans(Tween.TransitionType.Cubic);
        tween.TweenCallback(Callable.From(() => QueueFree()));
    }
}
