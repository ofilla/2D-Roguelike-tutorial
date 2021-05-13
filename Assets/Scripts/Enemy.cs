using UnityEngine;

public class Enemy : MovingObject
{
    public int playerDamage;

    private Animator animator;
    private Transform target;
    private bool skipMove;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    protected override void AttemptMove<T> (int xDir, int yDir) {
        if (skipMove) {
            skipMove = false;
            return;
        }

        base.AttemptMove<Player>(xDir, yDir);

        skipMove = true;
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;
        if (IsInSameColumn()) {
            yDir = GetMovementDirection(target.position.y, transform.position.y);
        } else {
            xDir = GetMovementDirection(target.position.x, transform.position.y);
        }

        AttemptMove<Player> (xDir, yDir);
    }

    protected override void OnCantMove<T> (T component)
    {
        Player hitPlayer = component as Player;
        hitPlayer.LoseFood(playerDamage);
    }

    private bool IsInSameColumn() {
        return Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon;
    }

    private int GetMovementDirection(float targetPosition, float ownPosition)
    {
        if (targetPosition > ownPosition) {
            return 1;
        } else {
            return -1;
        }
    }
}
