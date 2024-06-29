using _Scripts.UI;
using ShellTexturing;
using UnityEngine;

namespace _Scripts
{
    public class PlayerDeathParticles : MonoBehaviour
    {
        [SerializeField] private GameObject _particlesPrefab;
        [SerializeField] private float _forceMagnitude = 5f;
        [SerializeField] private Transform _spawnPos;

        public void OnDie()
        {
            for (int i = 0; i < 5; i++)
            {
                var obj = Instantiate(_particlesPrefab,_spawnPos.position, Quaternion.identity);
                var shell = obj.GetComponent<ShellTexture>();
                shell.shellColor = Shop.ParticleColor;
                var rb = obj.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    Vector3 randomDirection = (Random.insideUnitSphere + Vector3.up).normalized;
                    rb.AddForce(randomDirection * _forceMagnitude, ForceMode.Impulse);
                }
            }
        }
    }
}