using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
        public Vector2 direction {get; private set;}
        public void Update(){
            Debug.Log("Blinky is moving: " + direction);
        }
        
}
