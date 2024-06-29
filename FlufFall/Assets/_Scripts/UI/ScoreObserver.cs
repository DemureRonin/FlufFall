using UnityEngine;

namespace _Scripts.UI
{
    public class ScoreObserver : MonoBehaviour
    {
        public delegate void PlayerEvent();

        public static event PlayerEvent OnScore;

        public void Score()
        {
            OnScore?.Invoke();
        }
    }
}