using UnityEngine;

public class Gravity : Force
{
    private const float G = 9.81f;
    
    public Vector3 CalculateForce(PhysicsData state)
    {
        return Vector3.down * G * state.Mass;
    }
}
