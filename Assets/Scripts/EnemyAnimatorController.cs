using UnityEngine;
using DG.Tweening;
public class EnemyAnimatorController : MonoBehaviour
{
    private EnemyStatus enemyStatus;
    private EnemyMove enemyMove;
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        enemyStatus = this.gameObject.GetComponent<EnemyStatus>();
        enemyMove = this.gameObject.GetComponent<EnemyMove>();
        enemyStatus.AttackedEvent.AddListener(() =>
        {
            if (enemyMove != null)
            {
                enemyMove.PermitMove = false;
            }
            anim.SetTrigger("Hurt");
            DOVirtual.DelayedCall(0.3f, () =>
            {
                //攻撃を受けたらしばらくの間動けないようにする
                if (enemyMove != null)
                {
                    enemyMove.PermitMove = true;
                }
            });
        });
        enemyStatus.DeathEvent.AddListener(() =>
        {
            anim.SetBool("Death", true);
        });

    }

    // Update is called once per frame
    void Update()
    {
        if (enemyMove != null)
        {
            anim.SetBool("Move", enemyMove.Move);
        }
    }
}
