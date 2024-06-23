using UnityEngine;

public interface Force
{
    public Vector3 CalculateForce(PhysicsData state);
}
