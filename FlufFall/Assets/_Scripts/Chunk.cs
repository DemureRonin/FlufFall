using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class Chunk : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Color[] _colors;
        [SerializeField] private MeshRenderer[] _pipes;
        public bool IsInPool;


        private void Start()
        {
            if (!ProcLevelCreator.Started) return;
            var color = _colors[Random.Range(0, _colors.Length)];
            foreach (var pipe in _pipes)
            {
                pipe.material.color = color;
            }
        }
        private void OnDie()
        {
            _moveSpeed = 0;
        }


        private void Update()
        {
            var target = new Vector3(transform.position.x, 50, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, target, _moveSpeed* Time.deltaTime);
        }

        private void OnEnable()
        {
            PlayerDeathObserver.OnDie += OnDie;
        }

        
        private void OnDisable()
        {
            PlayerDeathObserver.OnDie -= OnDie;
        }
    }
}