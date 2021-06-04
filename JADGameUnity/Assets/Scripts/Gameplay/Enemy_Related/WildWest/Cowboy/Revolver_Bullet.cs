using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver_Bullet : MonoBehaviour
{
    [SerializeField] BoxCollider2D thisCollider;
    [SerializeField] Transform thisTransform;
    [SerializeField] GameObject cowboyParent;
    [SerializeField] Cowboy cowboyScript;
    [SerializeField] Rigidbody2D thisRigid;
    [SerializeField] Animator thisAnimator;

    //1 = up , 2 = down.
    [Tooltip("If shot up, then this is 1, if shot down then it should be 2.")]
    [SerializeField] int shootDirection;
    [SerializeField] float shotSpeed;

    const string Bounce = "Bounce";

    private void Awake()
    {
        thisRigid = this.GetComponent<Rigidbody2D>();
        thisAnimator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        
       // thisRigid.velocity = Vector2.left * shotSpeed;
       // thisRigid.velocity = Vector2.up * shotSpeed;
        if(shootDirection == 1)
        {
            thisRigid.velocity = new Vector2(-shotSpeed, shotSpeed);
        }
        else
        {
            thisRigid.velocity = new Vector2(-shotSpeed, -shotSpeed);
        }
    }

    //Initialization.
    public void initializeBullet(int setDir, float theSpeed, GameObject theCowboy)
    {
        shootDirection = setDir;
        shotSpeed = theSpeed;
        cowboyParent = theCowboy;
        cowboyScript = theCowboy.GetComponent<Cowboy>();
    }

    public void setShootDirection(int setDir)
    {
        shootDirection = setDir;
    }

    //This will be dependent on the difficulty.
    public void setShotSpeed(float theSpeed)
    {
        shotSpeed = theSpeed;
    }


    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Despawner")
        {
            Wave_Spawner.Instance.updateEnemiesLeft(1);
            Level_Manager.Instance.increaseEnemiesDodged();
            if (cowboyScript.getScoreVal() <= 0)
            {
                Level_Manager.Instance.updateScore(100);
            }
            else
            {
                Level_Manager.Instance.updateScore(cowboyScript.getScoreVal());
            }
            
            //TURN ON WHEN WE ARE READY TO POOL
            thisRigid.velocity = Vector2.zero;
            Object_Pooler.Instance.AddToPool(cowboyParent.gameObject);
            Destroy(gameObject);
            return;
        }

        if(collision.gameObject.tag == "Ceiling")
        {
            shootDirection = 2;
            thisAnimator.SetBool(Bounce, true);
            StartCoroutine(waitBounce());
        }

        if (collision.gameObject.tag == "Ground")
        {
            shootDirection = 1;
            thisAnimator.SetBool(Bounce, true);
            StartCoroutine(waitBounce());
        }

        if (collision.gameObject.tag == "Player")
        {
            Wave_Spawner.Instance.updateEnemiesLeft(1);
            Level_Manager.Instance.increaseEnemiesDodged();
            Level_Manager.Instance.Damage();
            Object_Pooler.Instance.AddToPool(cowboyParent.gameObject);
            Destroy(gameObject);
        }

    }

    IEnumerator waitBounce()
    {
        bool animDone = false;

        while(!animDone)
        {
            if (thisAnimator.GetCurrentAnimatorStateInfo(0).IsName("ricochet"))
            {
                thisAnimator.SetBool(Bounce, false);
                animDone = true;
            }
            yield return null;
        }


        yield return null;
    }

}
