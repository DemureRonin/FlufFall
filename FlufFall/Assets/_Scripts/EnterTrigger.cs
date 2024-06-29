using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts
{
    public class EnterTrigger : MonoBehaviour
    {
        [SerializeField] private Actions[] _actions;

        private void OnTriggerEnter(Collider otherCollider)
        {
            foreach (var action in _actions)
            {
                if (!otherCollider.CompareTag(action.OtherTag)) continue;
                action.GameEvent.Invoke(otherCollider.gameObject);
                return;
            }
        }
    }
    [Serializable]
    public class Actions
    {
        [SerializeField] private string _otherTag = "Player";

        [SerializeField] private EnterEvent _gameEvent;
        public string OtherTag => _otherTag;

        public EnterEvent GameEvent => _gameEvent;
    }

    [Serializable]
    public class EnterEvent : UnityEvent<GameObject>
    {
    }
}