using System;
using System.Threading;
using Priority_Queue;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("Logged objects")]
    public PhysicsObject[] controlledObjects;
    public ObjLogger[] loggers;
    
    [Header("Not logged objects")]
    public PhysicsObject[] notLogged;

    public PhysicsObject[] allObjects;

    [Header("Network Settings")]
    public bool isServer;
    
    private Thread thread;

    private bool running;
    
    private SimplePriorityQueue<Event> pq = new SimplePriorityQueue<Event>();

    private long startTime;

    private void Start()
    {
        allObjects = new PhysicsObject[controlledObjects.Length + notLogged.Length];

        for (int i = 0; i < controlledObjects.Length; i++)
        {
            allObjects[i] = controlledObjects[i];
            controlledObjects[i].isServer = isServer;
        }

        for (int i = 0; i < notLogged.Length; i++)
        {
            allObjects[i + controlledObjects.Length] = notLogged[i];
            notLogged[i].isServer = isServer;
        }
        
        thread = new Thread(loop);
        
        running = true;
        
        thread.Start();
        Debug.Log("START");
    }

    public void loop()
    {
        startTime = DateTime.Now.Ticks;

        foreach (ObjLogger logger in loggers)
        {
            pq.Enqueue(new Event(logger, DateTime.Now, DateTime.Now), 0);
        }

        foreach (PhysicsObject physicsObject in notLogged)
        {
            pq.Enqueue(new Event(physicsObject, DateTime.Now, DateTime.Now), 0);
        }

        while (running)
        {
            //No check for empty queue, since queue should never be empty.
            if (DateTime.Now >= pq.First.Time)
            {
                Event currentEvent = pq.Dequeue();
                float dt = (DateTime.Now.Ticks - currentEvent.PreviousTime.Ticks) / 10000000f; //dt in seconds
                
                // Debug.Log("Time From Start: " + ((DateTime.Now.Ticks - startTime) / 10000f));
                Event? next = currentEvent.Runner.runTask(dt, ref currentEvent);
                if (next != null)
                {
                    pq.Enqueue(next.Value, next.Value.Time.Ticks - startTime);
                }
            }
            // else
            // {
            //     float sleepAmount = ((DateTime.Now.Ticks - pq.First.Time.Ticks) / 10000f);
            //     
            //     if (sleepAmount >= 1)
            //     {
            //         DateTime start = DateTime.Now;
            //         Thread.Sleep((int) Math.Floor(sleepAmount));
            //     }
            // }
        }
    }

    public void UpdatePhysicsObject(int id, Vector3 position, long sentTime)
    {
        foreach (PhysicsObject physicsObject in allObjects)
        {
            if (physicsObject.id == id)
            {
                physicsObject.State.Pos = position;
                physicsObject.State.Vel = Vector3.zero;
                physicsObject.latency = DateTime.Now.Ticks - sentTime;
                return;
            }
        }
    }

    public void DeselectPhysicsObject(int id)
    {
        foreach (PhysicsObject physicsObject in allObjects)
        {
            if (physicsObject.id == id)
            {
                physicsObject.DeselectFromNetwork();
                return;
            }
        }
    }

    public void UpdatePhysicsObject(int id, Vector3 position, Vector3 velocity)
    {
        foreach (PhysicsObject physicsObject in allObjects)
        {
            if (physicsObject.id == id)
            {
                physicsObject.State.Pos = position;
                physicsObject.State.Vel = velocity;
                return;
            }
        }
    }

    public void OnDestroy()
    {
        running = false;

        thread.Join();
    }
}
