//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dragon : Obstacle_Behaviour
{
    [SerializeField] BoxCollider2D ogCollider;
    [SerializeField] BoxCollider2D endCollision;
    [SerializeField] BoxCollider2D despawnerCollider;
    [SerializeField] BoxCollider2D indicatorCollider;
    BoxCollider2D playerCollider;
    bool gotCollider;
    protected override void Awake()
    {
        gotCollider = false;
       // Physics2D.IgnoreLayerCollision(6, 7);
        base.Awake();
    }

    public override void OnObjectSpawn()
    {
        this.transform.position = new Vector2(this.transform.position.x + 2, this.transform.position.y);
        if (!gotCollider)
        {
            //Bad, gotta refactor!
            despawnerCollider = GameObject.Find("Despawner").GetComponent<BoxCollider2D>();
            indicatorCollider = GameObject.Find("Indicator_Vicinity_Collider").GetComponent<BoxCollider2D>();
            playerCollider = GameObject.Find("Player").GetComponent<BoxCollider2D>();
            Physics2D.IgnoreCollision(endCollision, playerCollider);
            Physics2D.IgnoreCollision(endCollision, indicatorCollider);
            Physics2D.IgnoreCollision(ogCollider, despawnerCollider);
            gotCollider = true;
        }
        base.OnObjectSpawn();
    }

}
