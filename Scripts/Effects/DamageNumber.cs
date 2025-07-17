using Godot;
using System;

public partial class DamageNumber : Node2D
{
    [Export] public Label TextLabel { get; set; }

    private RandomNumberGenerator _rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        // 随机旋转
        Rotation = Mathf.DegToRad(_rng.RandfRange(-15, 15));
    }

    public void ShowDamage(float damage, bool isCritical = false)
    {
        TextLabel.Text = Mathf.Round(damage).ToString();

        // 设置样式
        if (isCritical)
        {
            TextLabel.AddThemeColorOverride("font_color", Colors.Gold);
            TextLabel.AddThemeFontSizeOverride("font_size", 24);
        }
        else
        {
            TextLabel.AddThemeColorOverride("font_color", new Color(1, 0.8f, 0.8f));
        }

        // 播放动画
        var tween = CreateTween();
        tween.SetParallel();

        // 上浮
        tween.TweenProperty(this, "position:y", Position.Y - 50, 0.7f)
             .SetEase(Tween.EaseType.Out);

        // 淡出
        tween.TweenProperty(TextLabel, "modulate:a", 0.0f, 0.8f)
             .SetEase(Tween.EaseType.Out);

        // 结束时删除
        tween.Chain().TweenCallback(Callable.From(QueueFree));
    }
}