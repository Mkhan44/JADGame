using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Behaviour : MonoBehaviour
{
    Vector2 startPos;
    Vector2 endPos;
    float accum = 0f;

    public enum ElementType
    {
        neutral,
        fire,
        ice
    }

    [SerializeField]
    ElementType objectElement;

    private void Awake()
    {
        startPos = this.transform.position;
        endPos = new Vector2((startPos.x - 50), (startPos.y));
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        accum += 0.05f * Time.deltaTime;

        this.transform.position = Vector2.Lerp(startPos, endPos, accum);

        if(this.transform.position.x == endPos.x)
        {
            Destroy(gameObject);
            return;
        }
    }

    //Get the element from another script if needed.
    public ElementType getElement()
    {
        return objectElement;
    }
}
