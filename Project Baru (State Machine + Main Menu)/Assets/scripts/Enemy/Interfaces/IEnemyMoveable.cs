using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemymoveable
{
    Rigidbody2D RB { get; set; }
    bool IsFacingRight { get; set; }
    void MoveEnemy(Vector2 velocity);
    void CheckForLeftorRightFacing(Vector2 velocity);
}
