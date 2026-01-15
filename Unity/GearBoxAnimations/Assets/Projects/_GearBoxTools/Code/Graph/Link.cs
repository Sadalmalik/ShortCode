using System;
using UnityEngine;

namespace GearBoxTools.ForceGraph
{
    public class Link : MonoBehaviour
    {
        public Node NodeA;
        public Node NodeB;

        public float Length = 1;

        public float SpringForce;
        public float SpringDamping;

        public bool IsActive
            => NodeA != null &&
               NodeB != null &&
               NodeA.IsActive &&
               NodeB.IsActive;

        public void Tick(float deltaTime)
        {
            var delta = NodeB.Position - NodeA.Position;
            var dist = delta.magnitude;
            var dir = delta / dist;

            var velA = Vector3.Dot(dir, NodeA.Velocity);
            var velB = Vector3.Dot(dir, NodeB.Velocity);

            var totalVel = velB - velA;
            var difference = Length - dist;
            var force = (difference * SpringForce - totalVel * SpringDamping) * 0.5f;
            NodeA.ApplyForce(deltaTime, -dir * force);
            NodeB.ApplyForce(deltaTime, dir * force);
        }

        private void OnDrawGizmos()
        {
            if (!IsActive) return;

            Debug.DrawLine(NodeA.Position, NodeB.Position);
        }
    }
}