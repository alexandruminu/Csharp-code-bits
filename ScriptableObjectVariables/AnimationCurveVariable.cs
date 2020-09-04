using UnityEngine;

[CreateAssetMenu(fileName = "AnimationCurveVariable", menuName = "ScriptableObjects/Variables/Animation Curve", order = 1)]
public class AnimationCurveVariable : ScriptableObject
{
    public AnimationCurve Value;
}
