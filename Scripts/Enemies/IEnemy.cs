using Godot;
using System;

public interface IEnemy
{
    int GoldReward { get; }
    float GetHP();
    void TakeDamage(float damage);
    Vector2 GetPosition();
    bool IsDead();
}