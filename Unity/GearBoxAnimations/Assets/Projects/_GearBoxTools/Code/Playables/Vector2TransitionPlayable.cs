using System;
using UnityEngine;
using UnityEngine.Playables;

namespace GearBoxTools
{
    [Serializable]
    public class Vector2TransitionPlayable : PlayableAsset
    {
        public ExposedReference<GameObject> bindingTarget;
        [SerializeReference]
        public PropertyBinding<Vector2> binding;
        public Vector2 valueStart;
        public Vector2 valueEnd;
        public AnimationCurve curve;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var target = bindingTarget.Resolve(graph.GetResolver());
            if (target != null) binding.SetupTarget(target);
            return ScriptPlayable<Vector2TransitionBehaviour>.Create(graph, new Vector2TransitionBehaviour
            {
                binding = binding,
                valueStart = valueStart,
                valueEnd = valueEnd,
                curve = curve,
            });
        }
    }

    public class Vector2TransitionBehaviour : PlayableBehaviour
    {
        public PropertyBinding<Vector2> binding;
        public Vector2 valueStart;
        public Vector2 valueEnd;
        public AnimationCurve curve;

        public override void OnPlayableDestroy(Playable playable)
        {
            Debug.Log("HEYAA");
            base.OnPlayableDestroy(playable);
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);

            var progress = playable.GetProgress();

            var t = curve.Evaluate(progress);

            binding.SetValue(Vector2.Lerp(valueStart, valueEnd, t));
        }
    }
}