using System;
using UnityEngine;

public class PhysicsObject : MonoBehaviour, Eventable
{
    [Header("Initial data")]
    public Vector3 initialSpeed;
    public Vector3 initialAcceleration;
    public float initialMass;
    public float radius;
    
    public Force[] forces;
    public PhysicsData State;
    private PhysicsData tempState;
    
    [Header("Time settings")]
    public float Frequency;
    public float Period;

    [Header("References")]
    public GameObject interactor;

    [Header("Network data")]
    public int id;
    public bool isServer;
    
    [Header("Read only values")]
    public bool selected;

    public float residualTime;

    public long latency;
    
    
    private void Awake()
    {
        State = new PhysicsData(initialAcceleration, initialSpeed, transform.position, initialMass);
        tempState = new PhysicsData(initialAcceleration, initialSpeed, transform.position, initialMass);

        forces = new Force[] { new Gravity() };
        
        Period = 1.0f / Frequency;
        residualTime = 0.0f;
    }

    public void SolveRK(float dt)
    {
        float halfDt = dt / 2;
        
        
        Vector3 k1 = AccelerationAt(State);
        
        State.UpdateOther(ref tempState, k1 * halfDt, halfDt); //check what the second value of CustomUpdate is, could be dt, halfdt, or 1. (Changes force calculation)
        Vector3 k2 = AccelerationAt(tempState); 
        
        State.UpdateOther(ref tempState, k2 * halfDt, halfDt); //check what the second value of CustomUpdate is, could be dt, halfdt, or 1. (Changes force calculation)
        Vector3 k3 = AccelerationAt(tempState);
        
        State.UpdateOther(ref tempState, k3 * dt, dt); //check what the second value of CustomUpdate is, could be dt, halfdt, or 1. (Changes force calculation)
        Vector3 k4 = AccelerationAt(tempState);
        
        
        Vector3 totalAcceleration = dt / 6.0f * (k1 + 2 * k2 + 2 * k3 + k4);

        tempState = State;
        State.Update(totalAcceleration, dt);
    }

    public Vector3 AccelerationAt(PhysicsData state)
    {
        Vector3 totalForce = Vector3.zero;

        foreach (Force force in forces)
        {
            totalForce += force.CalculateForce(state);
        }

        return totalForce * state.invMass;
    }

    public void RunPhysics(float dt)
    {
        SolveRK(dt);

        if (State.Pos.y <= radius)
        {
            if (tempState.Pos.y > radius)
            {
                float timeToCollision = (tempState.Pos.y - radius) / State.Vel.y;

                State = tempState;

                SolveRK(timeToCollision);

                State.Vel.y *= -0.8f;
            }
            else
            {
                State.Pos.y = radius;

                if (State.Vel.y >= 0)
                {
                    State.Vel.y *= -1;
                }
            }
        }
    }

    public void DeselectFromNetwork()
    {
        Debug.Log("Server: " + isServer + ", latency: " + latency);
        float dt = latency / 10000000.0f; //TimeSpan.TicksPerSecond

        int times = (int)(dt / Period);
        residualTime += dt % Period;

        if (residualTime > Period)
        {
            times += (int)(residualTime / Period);
            residualTime %= Period;
        }

        for (int i = 0; i < times; i++)
        {
            RunPhysics(Period);
        }
    }

    public Event? runTask(float dt, ref Event self)
    {
        int times = (int) (dt / Period);
        residualTime += dt % Period;
        
        if (residualTime > Period)
        {
            times += (int) (residualTime / Period);
            residualTime %= Period;
        }

        if (!selected)
        {
            for (int i = 0; i < times; i++)
            {
                RunPhysics(Period);
            }
        }

        self.PreviousTime = DateTime.Now;
        self.Time = DateTime.Now.Add(TimeSpan.FromSeconds(Period));
        return self;
    }

    private void FixedUpdate()
    {
        if (selected)
        {
            State.Pos = interactor.transform.position;
            State.Vel = Vector3.zero;
            if (isServer)
            {
                ServerSend.PhysicsObjectUpdateFromServer(id, State.Pos);
            }
            else
            {
                ClientSend.PhysicsObjectUpdate(id, State.Pos);
            }
        }

        transform.position = State.Pos;
    }

    public void StartSelection()
    {
        selected = true;
    }

    public void EndSelection()
    {
        selected = false;

        if (isServer)
        {
            ServerSend.PhysicsObjectDeselectFromServer(id);
        }
        else
        {
            ClientSend.PhysicsObjectDeselected(id);
        }
    }
}

[Serializable]
public struct PhysicsData
{
    public Vector3 Acc;
    public Vector3 Vel;
    public Vector3 Pos;

    public float Mass;
    public float invMass; // 1/Mass

    public PhysicsData(Vector3 acc, Vector3 vel, Vector3 pos, float mass)
    {
        Acc = acc;
        Vel = vel;
        Pos = pos;

        Mass = mass;
        invMass = 1 / mass;
    }

    public void Update(Vector3 acceleration, float dt)
    {
        Acc = acceleration;
        Vel += acceleration; //The acceleration in runga-kutta already has the dt baked in
        Pos += Vel * dt;
    }

    public void UpdateOther(ref PhysicsData state, Vector3 acceleration, float dt)
    {
        state.Acc = acceleration;
        state.Vel = Vel + acceleration;
        state.Pos = Pos + Vel * dt;
    }
}
