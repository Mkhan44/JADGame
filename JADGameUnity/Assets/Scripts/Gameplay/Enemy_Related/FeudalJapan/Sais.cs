//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sais : Obstacle_Behaviour
{
    //Front sai.
    [SerializeField] BoxCollider2D sai1Collider;
    //Back sai.
    [SerializeField] BoxCollider2D sai2Collider;
    [SerializeField] BoxCollider2D despawnerCollider;
    bool gotCollider;

    protected override void Awake()
    {
        gotCollider = false;
        base.Awake();
    }

    public override void OnObjectSpawn()
    {
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y+0.25f);
        if (!gotCollider)
        {
            //Bad, gotta refactor!
            despawnerCollider = GameObject.Find("Despawner").GetComponent<BoxCollider2D>();
            Physics2D.IgnoreCollision(sai1Collider, despawnerCollider);
            gotCollider = true;
        }
        base.OnObjectSpawn();
    }

    protected override void Movement()
    {
        base.Movement();
    }

}
