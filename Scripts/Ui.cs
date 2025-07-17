using Godot;

public partial class UIManager : CanvasLayer
{
    private Label enemyCountLabel;

    public override void _Ready()
    {
        enemyCountLabel = GetNode<Label>("VBoxContainer/EnemyCountLabel");
    }

    public override void _Process(double delta)
    {
        int count = GetTree().GetNodesInGroup("enemies").Count;
        enemyCountLabel.Text = $"Enemies: {count}";
    }
}
