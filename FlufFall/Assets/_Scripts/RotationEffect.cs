using UnityEngine;

namespace _Scripts
{
    public class RotationEffect : MonoBehaviour
    {
       [SerializeField] private float _rotationSpeedZ;
       [SerializeField] private float _rotationSpeedY = 0;

        private void Update()
        {
            transform.Rotate(0,_rotationSpeedY * Time.deltaTime,_rotationSpeedZ * Time.deltaTime);
        }
        
    }
}
