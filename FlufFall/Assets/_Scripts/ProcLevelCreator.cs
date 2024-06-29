using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class ProcLevelCreator : MonoBehaviour
    {
        [SerializeField] private List<Chunk> _chunkPrefabs;
        [SerializeField] private List<Chunk> _chunks;
        private const float DespawnHeight = 26;
        private const float SpawnHeight = -38;
        private readonly Vector3 _spawnPosition = new(0, SpawnHeight, 0);
        public static bool Started;
        private bool _dead;
        public static event PlayerDeathObserver.PlayerEvent OnStart;
        private int _count;
        private Vector3 _poolPos = new(100, 100, 100);

        private void Start()
        {
            Started = false;
        }

        public void StartGame()
        {
            Started = true;
            OnStart?.Invoke();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Time.timeScale = 1;
            }

            /*if (Input.GetKeyDown(KeyCode.Space))
            {
               
            }*/

            if (_dead) return;
            if (_chunks.First().transform.position.y > DespawnHeight)
            {
                Despawn(_chunks.First());

                if (Started)
                {
                    if (_count < 8)
                    {
                        Spawn(_chunkPrefabs[Random.Range(0, _count)]);
                        _count++;
                    }
                    else
                    {
                        Spawn(_chunkPrefabs[Random.Range(0, _chunkPrefabs.Count)]);
                    }
                }
                else
                {
                    Spawn(_chunkPrefabs[0]);
                    
                }
            }
        }

        private void Spawn(Chunk chunk)
        {
            if (chunk.IsInPool)
            {
                chunk = Instantiate(chunk, _spawnPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));
                _chunkPrefabs.Add(chunk);
            }

            chunk.IsInPool = true;
            chunk.transform.position = _chunks.First().transform.position - new Vector3(0, 48, 0);
            _chunks.Add(chunk);
            chunk.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            chunk.gameObject.SetActive(true);
        }

        private void Despawn(Chunk chunk)
        {
            chunk.gameObject.SetActive(false);
            _chunks.Remove(chunk);
            chunk.transform.position = _poolPos;
            chunk.IsInPool = false;
        }

        private void OnEnable()
        {
            PlayerDeathObserver.OnDie += OnDie;
        }

        private void OnDie()
        {
            _dead = true;
        }


        private void OnDisable()
        {
            PlayerDeathObserver.OnDie -= OnDie;
        }
    }
}