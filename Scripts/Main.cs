using Godot;
using System;
using System.IO;
using System.Reflection.Emit;

public partial class Main : Node2D
{
    [Export] public PackedScene EnemyScene;
    [Export] public PackedScene TowerScene;
    [Export] public Node2D EnemyContainer;
    [Export] public float SpawnInterval = 0.5f;

    private float spawnTimer = 0f;
    private RandomNumberGenerator rng = new();

    public override void _Ready()
    {
        var Tower = GetNode<Node2D>("Tower");
        Tower.Position = new Vector2(960, 540);

        // 创建10个塔
        Random random = new Random();
        for (int i = 0; i < 10; i++)
        {
            var tower = TowerScene.Instantiate<Node2D>();
            tower.Position = new Vector2(960 + random.Next(-100,100) * 960 / 100, 540 + random.Next(-100, 100) * 540 / 100);
            AddChild(tower);
        }
    }

    public override void _Process(double delta)
    {
        spawnTimer -= (float)delta;
        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = SpawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        if (EnemyScene == null) return;

        var enemy = EnemyScene.Instantiate<CharacterBody2D>();
        Vector2 spawnPos = GetRandomEdgePosition();
        enemy.GlobalPosition = spawnPos;
        EnemyContainer.AddChild(enemy);
        enemy.AddToGroup("enemies");
    }

    private Vector2 GetRandomEdgePosition()
    {
        var screenSize = GetViewport().GetVisibleRect().Size;
        int side = rng.RandiRange(0, 3);

        return side switch
        {
            0 => new Vector2(rng.RandfRange(0, screenSize.X), 50), // Top
            1 => new Vector2(rng.RandfRange(0, screenSize.X), screenSize.Y + 50), // Bottom
            2 => new Vector2(50, rng.RandfRange(0, screenSize.Y)), // Left
            3 => new Vector2(screenSize.X + 50, rng.RandfRange(0, screenSize.Y)), // Right
            _ => Vector2.Zero
        };
    }
}
