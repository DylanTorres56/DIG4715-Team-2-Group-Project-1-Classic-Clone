using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusFruit : MonoBehaviour
{
    // BONUS FRUIT RULES:
    // 1) Spawns below Ghost House
    // 2) Appears after 70 pellets eaten for the first time, OR 170 pellets the next times unless 1st fruit is still active
    // 3) Disappears after 9-10 seconds or when eaten
    // 4) Cherries give 100, Strawberries give 300

    public int fruitPoints { get; private set; } = 100;
    public float duration = 9.0f;


    protected virtual void Eat()
    {
        FindObjectOfType<GameManager>().BonusFruitEaten(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pac-Man"))
        {
            //Checks to see if a bonus fruit was eaten before, and returns a new score value of 300;
            if (FindObjectOfType<GameManager>().numOfBonusFruitEaten == 1) 
            {
                fruitPoints = 300;
            }

            Eat();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
