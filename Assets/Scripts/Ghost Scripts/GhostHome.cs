using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHome : GhostBehavior
{
    public Transform insideTransform;
    public Transform outsideTransform;
    private void OnEnable()
    {
        StopAllCoroutines();
    }

    //Calls start of coroutine, useful for pausing execution
    private void OnDisable()
    {
        if (this.gameObject.activeSelf) 
        {
            StartCoroutine(ExitTransition());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(true);
        if (this.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")) 
        {
            this.ghost.movement.SetDirection(-this.ghost.movement.direction);
        }
    }

    private IEnumerator ExitTransition() 
    {
        //Ghost is forced up when transition is active
        this.ghost.movement.SetDirection(Vector2.up, true);

        //Turns off collision with object while transition is active
        this.ghost.movement.rigidbody.isKinematic = true; 
        this.ghost.movement.enabled = false;

        Vector3 position = this.transform.position;

        float duration = 0.5f;
        float timeElapsed = 0.0f;

        //Interpolating from 1st position: inside position (Continues until timeElapsed/duration = 1, then position of ghost will equal insideTransform.position)
        while (timeElapsed < duration)
        {
            Vector3 newPosition = Vector3.Lerp(position, this.insideTransform.position, timeElapsed / duration);
            newPosition.z = position.z;
            this.ghost.transform.position = newPosition;
            timeElapsed += Time.deltaTime;
            yield return null; 
        }

        timeElapsed = 0.0f;

        //Interpolating to 2nd position: outside position 
        while (timeElapsed < duration)
        {
            Vector3 newPosition = Vector3.Lerp(this.insideTransform.position, this.outsideTransform.position, timeElapsed / duration);
            newPosition.z = position.z;
            this.ghost.transform.position = newPosition;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        //When transition is inactive, this new Vector2 is a value between 0 and 1: if < .5, goes left, and if > .5, goes right 
        this.ghost.movement.SetDirection(new Vector2(Random.value < 0.5f ? -1.0f : 1.0f, 0.0f), true);

        //Turns on collision with object while transition is inactive
        this.ghost.movement.rigidbody.isKinematic = false; 
        this.ghost.movement.enabled = true;

    }
}
