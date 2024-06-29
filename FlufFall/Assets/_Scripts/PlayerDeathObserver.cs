using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using YG;

namespace _Scripts
{
    public class PlayerDeathObserver : MonoBehaviour
    {
        public delegate void PlayerEvent();

        public static event PlayerEvent OnDie;

        public void OnPlayerDeath()
        {
            OnDie?.Invoke();
         
           
        }

        private IEnumerator ShowAd()
        {
            yield return new WaitForSeconds(0.3f);
          
        }
    }
}