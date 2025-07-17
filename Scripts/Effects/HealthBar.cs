using Godot;
using System;

public partial class HealthBar : Node2D
{
    [Export] public TextureProgressBar ProgressBar { get; set; }
    [Export] public float DisplayDuration = 2.0f;
    
    private Timer _hideTimer;
    private Tween _valueTween;
    
    public override void _Ready()
    {
        _hideTimer = new Timer();
        _hideTimer.OneShot = true;
        _hideTimer.Timeout += HideBar;
        AddChild(_hideTimer);
        
        // 初始隐藏
        ProgressBar.Visible = false;
    }
    
    public void UpdateHealth(float current, float max, bool animate = true)
    {
        // 确保值有效
        current = Mathf.Clamp(current, 0, max);
        ProgressBar.MaxValue = max;
        
        // 显示血条
        ProgressBar.Visible = true;
        ProgressBar.Modulate = new Color(1, 1, 1, 1);
        
        // 重置计时器
        _hideTimer.Stop();
        _hideTimer.Start(DisplayDuration);
        
        // 更新值（带动画）
        if (_valueTween != null && _valueTween.IsRunning())
            _valueTween.Kill();
        
        if (animate)
        {
            _valueTween = CreateTween();
            _valueTween.TweenProperty(ProgressBar, "value", current, 0.3f)
                       .SetEase(Tween.EaseType.Out);
        }
        else
        {
            ProgressBar.Value = current;
        }

        // 根据血量改变颜色
        //var ratio = current / max;
        //ProgressBar.TintProgress = ratio > 0.6f ? Colors.Green :
        //                          ratio > 0.3f ? Colors.Yellow : Colors.Red;
    }
    
    private void HideBar()
    {
        var tween = CreateTween();
        tween.TweenProperty(ProgressBar, "modulate:a", 0.0f, 0.5f)
             .SetEase(Tween.EaseType.Out);
        tween.TweenCallback(Callable.From(() => ProgressBar.Visible = false));
    }
}