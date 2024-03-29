using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EllieMovement : NPCFollow //Inherits all methods and classes in the NPCFollow script. Used by multiple NPCs
{
    public CircularQueue path;

    // Start is called before the first frame update
    void Start()
    {
        path = new CircularQueue(15, this);
    }

    // Update is called once per frame
    void Update()
    {
        IsLeaderInFront();
    }
    void FixedUpdate()
    {
        IsLeaderMoving();
        if (isLeaderMoving == true)
        {
            EnqueueLeaderPosition(path); 
        }
    }
}
