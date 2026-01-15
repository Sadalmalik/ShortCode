using UnityEngine;

namespace GearBoxTools.ForceGraph
{
    public class Well : MonoBehaviour
    {
        public Vector3 Aspect;
        public float Power;
        public AnimationCurve Curve;
        
        public void Apply(float deltaTime, Node node)
        {
            var delta = transform.position - node.Position;
            delta.x /= Aspect.x;
            delta.y /= Aspect.y;
            delta.z /= Aspect.z;
            var distance = delta.magnitude;
            if (distance == 0) return;
            node.ApplyForce(deltaTime, delta * (Power * Curve.Evaluate(Mathf.Clamp01(distance)) / distance));
        }
    }
}