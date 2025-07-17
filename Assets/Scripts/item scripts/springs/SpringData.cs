using UnityEngine;

[CreateAssetMenu(menuName = "Spring Data")]

public class SpringData : ScriptableObject
{
    public Vector2 upSpringForce;
    public Vector2 rightSpringForce;
    public Vector2 downSpringForce;
    public Vector2 leftSpringForce;

    [Space(5)]
    public float SpringBoostCheckDuration;

}
