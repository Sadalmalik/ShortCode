using System;
using UnityEngine;
using UnityEngine.Playables;

namespace GearBoxTools
{
    [Serializable]
    public class FloatSetterPlayable : PlayableAsset
    {
        public ExposedReference<GameObject> bindingTarget;
        [SerializeReference]
        public PropertyBinding<float> binding;
        public float value;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var target = bindingTarget.Resolve(graph.GetResolver());
            if (target != null) binding.SetupTarget(target);
            return ScriptPlayable<FloatSetterBehaviour>.Create(graph, new FloatSetterBehaviour
            {
                binding = binding,
                value = value,
            });
        }
    }
    
    public class FloatSetterBehaviour : PlayableBehaviour
    {
        public PropertyBinding<float> binding;
        public float value;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);

            binding.SetValue(value);
        }
    }
}