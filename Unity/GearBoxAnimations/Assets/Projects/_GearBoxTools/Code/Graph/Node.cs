using System;
using UnityEngine;

namespace GearBoxTools
{
    public class Node : MonoBehaviour
    {
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Vector3 Velocity;
        public float Damping = 0.25f;
        public bool kinematic;

        public bool IsActive => gameObject.activeSelf;

        public void ApplyForce(float deltaTime, Vector3 force)
        {
            Velocity += deltaTime * force;
        }

        public void Tick(float deltaTime)
        {
            ApplyForce(deltaTime, -Velocity * Damping);

            if (!kinematic)
                transform.position += Velocity * deltaTime;
        }
    }
}