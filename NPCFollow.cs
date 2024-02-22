using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class NPCFollow : MonoBehaviour
{
    public float speed;
    public float enqRate;
    public string leader;
    public bool isLeaderMoving;
    public Rigidbody2D leaderRB;
    public GameObject follower;
    public Rigidbody2D followerRB;
    public Animator anim;

    public class CircularQueue
    {
        private Vector2[] positions;
        private int front;
        private int rear;
        private int maxSize;
        private NPCFollow npcFollowInstance;

        public CircularQueue(int size, NPCFollow npcFollow)
        {
            positions = new Vector2[size];
            front = -1;
            rear = -1;
            maxSize = size;
            npcFollowInstance = npcFollow;
        }
        public void Enqueue(Vector2 item)
        {
            if ((rear + 1) % maxSize == front)
            {
                if (front != -1)
                {
                    Dequeue();
                }
            }
            if (rear == -1)
                front = 0;
            rear = (rear + 1) % maxSize;
            positions[rear] = item;
        }
        public Vector2 Dequeue()
        {
            if (front == -1)
            {
                return Vector2.zero;
            }
            Vector2 dequeuedItem = positions[front];
            if (front == rear)
            {
                front = rear = -1;
            }
            else
            {
                front = (front + 1) % maxSize;
            }
            npcFollowInstance.MoveToDequeuedPosition(dequeuedItem);
            return dequeuedItem;
        }
    }
    
    public void EnqueueLeaderPosition(CircularQueue queue)
    {
        GameObject Leader = GameObject.Find(leader);
        if (Leader != null)
        {
            Vector2 leaderPosition = Leader.transform.position;
            queue.Enqueue(leaderPosition);     
        }
        else
        {
            Debug.LogError("GameObject 'leader' not found. Make sure it exists in the scene.");
        }
    }
    public void MoveToDequeuedPosition(Vector2 dequeuedItem)
    {
        Vector2 position = transform.position;
        Vector2 direction = (dequeuedItem - position).normalized;

        float distance = Vector2.Distance(position, dequeuedItem);

        if (distance > Mathf.Epsilon) //very small value 
        {
            followerRB.MovePosition(position + direction * speed * Time.fixedDeltaTime);
        }
        else
        {
            followerRB.MovePosition(dequeuedItem);
        }
    }
    public void IsLeaderMoving()
    {
        GameObject leaderMoving = GameObject.Find(leader);
        Rigidbody2D rbe = leaderMoving.GetComponent<Rigidbody2D>();
        if (rbe.velocity == Vector2.zero)
        {
            isLeaderMoving = false;
            followerRB.velocity = Vector2.zero;

        }
        else
        {
            isLeaderMoving = true;
            followerRB.velocity = new Vector2(1, 1);
        }

    }
    public void IsLeaderInFront()
    {
        GameObject leaderFront = GameObject.Find(leader);
        float followerY = transform.position.y;
        float leaderY = leaderFront.transform.position.y;

        if (followerY < leaderY)
        {
            Renderer frenderer = GetComponent<Renderer>();
            Renderer lrenderer = leaderFront.GetComponent<Renderer>();
            frenderer.sortingOrder = lrenderer.sortingOrder + 1;
        }
        else
        {
            Renderer renderer = GetComponent<Renderer>();
            Renderer lrenderer = leaderFront.GetComponent<Renderer>();
            renderer.sortingOrder = lrenderer.sortingOrder - 1;
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
