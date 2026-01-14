using System;
using UnityEngine;
using UnityEngine.Playables;

namespace GearBoxTools
{
    [Serializable]
    public class FloatTransitionPlayable : PlayableAsset
    {
        public ExposedReference<GameObject> bindingTarget;
        [SerializeReference]
        public PropertyBinding<float> binding;
        public float valueStart;
        public float valueEnd;
        public AnimationCurve curve;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var target = bindingTarget.Resolve(graph.GetResolver());
            if (target != null) binding.SetupTarget(target);
            return ScriptPlayable<FloatTransitionBehaviour>.Create(graph, new FloatTransitionBehaviour
            {
                binding = binding,
                valueStart = valueStart,
                valueEnd = valueEnd,
                curve = curve,
            });
        }
    }

    public class FloatTransitionBehaviour : PlayableBehaviour
    {
        public PropertyBinding<float> binding;
        public float valueStart;
        public float valueEnd;
        public AnimationCurve curve;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);

            var progress = playable.GetProgress();

            var t = curve.Evaluate(progress);

            binding.SetValue(Mathf.Lerp(valueStart, valueEnd, t));
        }
    }
}