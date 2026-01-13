using UnityEngine.Playables;

namespace GearBoxTools
{
    public static class PlayableExtensions
    {
        public static float GetProgress(this Playable playable)
        {
            var duration = playable.GetDuration();
            if (duration == 0) return 1;
            return (float) (playable.GetTime() / duration);
        }
    }
}