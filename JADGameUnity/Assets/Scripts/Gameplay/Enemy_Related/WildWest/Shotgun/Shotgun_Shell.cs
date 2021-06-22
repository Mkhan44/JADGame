using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun_Shell : MonoBehaviour
{
    [SerializeField] BoxCollider2D thisCollider;
    [SerializeField] Transform thisTransform;
    [SerializeField] Rigidbody2D thisRigid;
    [SerializeField] Shotgun shottyParent;
    [SerializeField] Animator thisAnimator;
    [SerializeField] GameObject theOtherShell;
    [SerializeField] BoxCollider2D playerCollider;

    private void Awake()
    {
        playerCollider = GameObject.Find("Player").GetComponent<BoxCollider2D>();
        thisRigid = this.GetComponent<Rigidbody2D>();
        thisAnimator = this.GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initializeShell(GameObject otherShell, Shotgun theShotty)
    {
        shottyParent = theShotty;
        theOtherShell = otherShell;

    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Despawner")
        {
            Destroy(theOtherShell);

            Wave_Spawner.Instance.updateEnemiesLeft(1);
            Level_Manager.Instance.increaseEnemiesDodged();
            if (shottyParent.getScoreVal() <= 0)
            {
                Level_Manager.Instance.updateScore(100);
            }
            else
            {
                Level_Manager.Instance.updateScore(shottyParent.getScoreVal());
            }

            //TURN ON WHEN WE ARE READY TO POOL
            thisRigid.velocity = Vector2.zero;
            Object_Pooler.Instance.AddToPool(shottyParent.gameObject);
            Destroy(gameObject);
            return;
        }

        if (collision.gameObject.tag == "Player")
        {
            Destroy(theOtherShell);

           // Wave_Spawner.Instance.updateEnemiesLeft(1);
            Level_Manager.Instance.increaseEnemiesDodged();
            Level_Manager.Instance.Damage();
            Object_Pooler.Instance.AddToPool(shottyParent.gameObject);
            Destroy(gameObject);
        }

    }

}
