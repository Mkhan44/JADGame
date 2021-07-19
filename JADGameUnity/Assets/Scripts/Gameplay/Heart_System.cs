//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart_System : MonoBehaviour
{
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite brokenHeart;
    [SerializeField] GameObject healthIconPrefab;
    [SerializeField] List<Image> amountOfHearts = new List<Image>();
    [SerializeField] GridLayoutGroup theGrid;
    int currentHealth;
    int maxHealth;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Should be called at the start from LevelManager. This populates the health counter based on health of the player.
    public void initializeHealth(int healthVal)
    {
        maxHealth = healthVal;
        currentHealth = maxHealth;

        if(maxHealth > 8)
        {
            theGrid.cellSize = new Vector2(100, 100);
        }

        if(Canvas_Resolution.instance.getReferenceReso() == new Vector2(828, 1792))
        {
            theGrid.cellSize = new Vector2(100, 100);
        }

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject tempObj = Instantiate(healthIconPrefab, this.transform);
            Image currentSprite = tempObj.GetComponent<Image>();
            amountOfHearts.Add(currentSprite);
        }
    }

    public void updateHealth(int healthVal)
    {
        currentHealth = healthVal;

        //Play particle effect that breaks the heart.
        //If we want a heal function, then we'll need to have conditions for if we want a 'hurt' particle vs a 'heal' particle to play.
        //Insantiate(particle, amountOfHearts[currentHealth+1].gameobject.transform)

        for (int i = 0; i < maxHealth; i++)
        {
            if(i + 1 <= currentHealth)
            {
                amountOfHearts[i].sprite = fullHeart;
            }
            else
            {
                amountOfHearts[i].sprite = brokenHeart;
            }
        }
    }
}
