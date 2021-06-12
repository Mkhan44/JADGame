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
    [SerializeField] Material backSaiMat;
    bool gotCollider;

    protected override void Awake()
    {
        backSaiMat = sai2Collider.gameObject.GetComponent<SpriteRenderer>().material;

        backSaiMat.SetFloat("_OutlineThickness", 3f);

        if (objectElement == ElementType.fire)
        {
            // Debug.Log("SPAWNED FIRE ELEMENTAL ITEM, CHANGING SHADER.");
            Color fireColor = new Color(212, 139, 57, 255);
            backSaiMat.SetColor("_OutlineColor", Color.red);
        }
        else if (objectElement == ElementType.ice)
        {
            // Debug.Log("SPAWNED ICE ELEMENTAL ITEM, CHANGING SHADER.");
            Color iceColor = new Color(70, 219, 213, 255);
            backSaiMat.SetColor("_OutlineColor", Color.cyan);
        }

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
