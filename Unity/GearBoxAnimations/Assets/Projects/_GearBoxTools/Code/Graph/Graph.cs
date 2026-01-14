using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GearBoxTools
{
    public class Graph : MonoBehaviour
    {
        public bool active;
        public float repulsionPower;
        public float noisePower;
        public List<Node> nodes;
        public List<Link> links;

        private void Awake()
        {
            nodes = GetComponentsInChildren<Node>().ToList();
            links = GetComponentsInChildren<Link>().ToList();
        }

        private void FixedUpdate()
        {
            if (!active) return;

            var deltaTime = Time.fixedDeltaTime;
            CalculateRepulsion(deltaTime);

            foreach (var link in links)
            {
                link.Tick(deltaTime);
            }

            foreach (var node in nodes)
            {
                if (!node.IsActive) continue;

                if (noisePower > 0)
                    node.ApplyForce(deltaTime, GetDirection(node.Position) * noisePower);
                
                node.Tick(deltaTime);
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

        public static Vector3 GetDirection(Vector3 pos)
        {
            var dx = PerlinNoise3D(pos.x+0.1f, pos.y, pos.z) - PerlinNoise3D(pos.x-0.1f, pos.y, pos.z);
            var dy = PerlinNoise3D(pos.x, pos.y+0.1f, pos.z) - PerlinNoise3D(pos.x, pos.y-0.1f, pos.z);
            var dz = PerlinNoise3D(pos.x, pos.y, pos.z+0.1f) - PerlinNoise3D(pos.x, pos.y, pos.z-0.1f);
            return new Vector3(dx, dy, dz) * 5;
        }
        
        public static float PerlinNoise3D(float x, float y, float z)
        {
            y += 1;
            z += 2;
            float xy = _perlin3DFixed(x, y);
            float xz = _perlin3DFixed(x, z);
            float yz = _perlin3DFixed(y, z);
            float yx = _perlin3DFixed(y, x);
            float zx = _perlin3DFixed(z, x);
            float zy = _perlin3DFixed(z, y);

            return xy * xz * yz * yx * zx * zy;
        }

        static float _perlin3DFixed(float a, float b)
        {
            return Mathf.Sin(Mathf.PI * Mathf.PerlinNoise(a, b));
        }
    }
}