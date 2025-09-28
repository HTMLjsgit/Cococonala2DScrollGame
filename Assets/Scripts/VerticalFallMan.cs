using UnityEngine;
using UnityEngine.UIElements;

public class VerticalFallMan : MonoBehaviour
{
    private EnemyAnimatorController enemyAnimatorController;
    private EnemyMove enemyMove;
    private Animator anim;
    private enum Direction
    {
        Up,
        Down
    }
    [SerializeField]
    private Direction direction;
    [SerializeField]
    private AudioSource DownGroundSound;
    private float DefaultSpeed;
    [SerializeField] private GroundCheckScript groundCheckScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        enemyMove = this.gameObject.GetComponent<EnemyMove>();
        DefaultSpeed = enemyMove.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (direction == Direction.Up)
        {
            anim.SetBool("Down", false);
            anim.SetBool("Up", true);
        }
        else if (direction == Direction.Down)
        {
            anim.SetBool("Down", true);
            anim.SetBool("Up", false);
            if (groundCheckScript.IsGround)
            {
                DownGroundSound.Play();
            }
        }
        if (enemyMove.DirectionX > 0)
        {
            direction = Direction.Up;
            enemyMove.speed = DefaultSpeed * 0.3f;
        }
        else
        {
            direction = Direction.Down;
            enemyMove.speed = DefaultSpeed;
        }
    }
}
