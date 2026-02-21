using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace GearBoxTools.ForceGraph
{
    public class Graph : MonoBehaviour
    {
        public bool active;
        public float repulsionPower;
        public float noisePower = 0.00f;
        public float noiseSpeed = 0.01f;
        public float noiseScale = 10f;
        public List<Node> nodes;
        public List<Link> links;
        public List<Well> wells;
        public List<Transform> traces;
        
        private void Awake()
        {
            nodes = GetComponentsInChildren<Node>(true).ToList();
            links = GetComponentsInChildren<Link>(true).ToList();
            wells = GetComponentsInChildren<Well>(true).ToList();
        }

        private void FixedUpdate()
        {
            if (!active) return;

            var deltaTime = Time.fixedDeltaTime;
            CalculateRepulsion(deltaTime);

            foreach (var link in links)
            {
                if (link.IsActive)
                    link.Tick(deltaTime);
            }

            foreach (var node in nodes)
            {
                if (!node.IsActive) continue;
                
                foreach (var well in wells)
                    well.Apply(deltaTime, node);

                if (noisePower > 0)
                    node.ApplyForce(deltaTime, GetDirection(node.Position) * noisePower);
                
                node.Tick(deltaTime);
            }

            foreach (var trace in traces)
            {
                trace.position += GetDirection(trace.position) * noisePower;
            }
        }

        private void CalculateRepulsion(float deltaTime)
        {
            var count = nodes.Count;
            for (int i = 0; i < count; i++)
            for (int k = i + 1; k < count; k++)
            {
                var nodeA = nodes[i];
                var nodeB = nodes[k];

                var delta = nodeB.Position - nodeA.Position;
                var dist = delta.magnitude;
                var dir = delta / dist;

                var repulsion = repulsionPower * Mathf.Pow(dist, -2);
                
                nodeA.ApplyForce(deltaTime, -dir * repulsion);
                nodeB.ApplyForce(deltaTime, dir * repulsion);
            }
        }

        public Vector3 GetDirection(Vector3 pos)
        {
            pos /= noiseScale;
            var t = Time.time * noiseSpeed;
            var dx = PerlinNoise3D(t, pos.x+0.1f, pos.y, pos.z) - PerlinNoise3D(t, pos.x-0.1f, pos.y, pos.z);
            var dy = PerlinNoise3D(t, pos.x, pos.y+0.1f, pos.z) - PerlinNoise3D(t, pos.x, pos.y-0.1f, pos.z);
            var dz = PerlinNoise3D(t, pos.x, pos.y, pos.z+0.1f) - PerlinNoise3D(t, pos.x, pos.y, pos.z-0.1f);
            return new Vector3(dx, dy, dz) * 5;
        }
        
        public static float PerlinNoise3D(float t, float x, float y, float z)
        {
            y += 1;
            z += 2;
            float xy = Mathf.PerlinNoise(x, y * t); // _perlin3DFixed(x, y);
            float xz = Mathf.PerlinNoise(x, z * t);
            float yz = Mathf.PerlinNoise(y, z * t);
            float yx = Mathf.PerlinNoise(y, x * t);
            float zx = Mathf.PerlinNoise(z, x * t);
            float zy = Mathf.PerlinNoise(z, y * t);

            return xy * xz * yz * yx * zx * zy;
        }

        static float _perlin3DFixed(float a, float b)
        {
            return Mathf.Sin(Mathf.PI * Mathf.PerlinNoise(a, b));
        }
    }
}