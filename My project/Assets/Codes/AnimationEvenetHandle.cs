using UnityEngine;

public class AnimationEvenetHandle : MonoBehaviour
{

    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    private void DisableMovementandJump() 
    {
        player.EnableJumpMovement(false);
    }

    private void EnableMovementandJump()
    {
        player.EnableJumpMovement(true);
    }

    public void DamageEnemy() 
    {
        player.DamageEnemies();
    }
}
