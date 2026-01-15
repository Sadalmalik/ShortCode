using System;
using UnityEngine;
using UnityEngine.Playables;

namespace GearBoxTools
{
    [Serializable]
    public class BouncyPositionPlayable : PlayableAsset
    {
        public ExposedReference<GameObject> bouncyObject;
        public ExposedReference<GameObject> slotObject;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var bouncy = bouncyObject.Resolve(graph.GetResolver());
            var slot = slotObject.Resolve(graph.GetResolver());
            
            return ScriptPlayable<BouncyPositionBehaviour>.Create(graph, new BouncyPositionBehaviour
            {
                bouncyObject = bouncy.GetComponent<BouncyPosition2D>(),
                slotObject = slot.GetComponent<RectTransform>(),
            });
        }
    }
    
    public class BouncyPositionBehaviour : PlayableBehaviour
    {
        public BouncyPosition2D bouncyObject;
        public RectTransform slotObject;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);

            bouncyObject.FollowRect = slotObject;
        }
    }
}