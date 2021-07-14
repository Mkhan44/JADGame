using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizBeam : MonoBehaviour
{
    [SerializeField] BoxCollider2D thisCollider;
    [SerializeField] Transform thisTransform;
    [SerializeField] GameObject wizParent;
    [SerializeField] Wizard wizScript;
    [SerializeField] Animator thisAnimator;


    private void Awake()
    {
        thisAnimator = this.GetComponent<Animator>();
    }


    //Initialization.
    public void initializeBeam(GameObject theWizard)
    {
        wizParent = theWizard;

    }




    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if (collision.gameObject.tag == "Despawner")
        {
            Wave_Spawner.Instance.updateEnemiesLeft(1);
            Level_Manager.Instance.increaseEnemiesDodged();
            if (wizScript.getScoreVal() <= 0)
            {
                Level_Manager.Instance.updateScore(100);
            }
            else
            {
                Level_Manager.Instance.updateScore(wizScript.getScoreVal());
            }
            Object_Pooler.Instance.AddToPool(wizScript.gameObject);
            Destroy(gameObject);
            return;
        }
        */
      

        if (collision.gameObject.tag == "Player")
        {
            //Wave_Spawner.Instance.updateEnemiesLeft(1);
            Level_Manager.Instance.increaseEnemiesDodged();
            Level_Manager.Instance.Damage();
            Object_Pooler.Instance.AddToPool(wizParent.gameObject);
            Destroy(gameObject);
        }

    }


}
