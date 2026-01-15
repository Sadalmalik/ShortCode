using System;
using UnityEngine;
using UnityEngine.Playables;

namespace GearBoxTools
{
    [Serializable]
    public class Vector2SetterPlayable : PlayableAsset
    {
        public ExposedReference<GameObject> bindingTarget;
        [SerializeReference]
        public PropertyBinding<Vector2> binding;
        public Vector2 value;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var target = bindingTarget.Resolve(graph.GetResolver());
            if (target != null) binding.SetupTarget(target);
            return ScriptPlayable<Vector2SetterBehaviour>.Create(graph, new Vector2SetterBehaviour
            {
                binding = binding,
                value = value,
            });
        }
    }
    
    public class Vector2SetterBehaviour : PlayableBehaviour
    {
        public PropertyBinding<Vector2> binding;
        public Vector2 value;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);

            binding.SetValue(value);
        }
    }
}