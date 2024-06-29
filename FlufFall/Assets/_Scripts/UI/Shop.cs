using System;
using ShellTexturing;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace _Scripts.UI
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private Image _redImage;
        [SerializeField] private Image _orangeImage;
        [SerializeField] private Image _purpleImage;

        [SerializeField] private Color _redColor;
        [SerializeField] private Color _orangeColor;
        [SerializeField] private Color _purpleColor;
        [SerializeField] private Color _blueColor;
        [SerializeField] private Color _greenColor;
        [SerializeField] private Color _yellowColor;

        [SerializeField] private GameObject _redAd;
        [SerializeField] private GameObject _orangeAd;
        [SerializeField] private GameObject _purpleAd;
        [SerializeField] private GameObject _greenAd;
        [SerializeField] private GameObject _yellowAd;
        
        [SerializeField] private Button _redButton;
        [SerializeField] private Button _orangeButton;
        [SerializeField] private Button _purpleButton;
        [SerializeField] private Button _greenButton;
        [SerializeField] private Button _yellowButton;
        

        [SerializeField] private ShellTexture _shell;
        [SerializeField] private AudioSource _click;
        public static Color ParticleColor;
        
        public int _blue;
        public int _red;
        public int _orange;
        public int _purple;
        public int _yellow;
        public int _green;
        public int _lastColor = 0;

        private void Start()
        {
            ParticleColor = _blueColor;
            _lastColor = PlayerPrefs.GetInt("Last");
            _red = PlayerPrefs.GetInt("Red");
            _orange = PlayerPrefs.GetInt("Orange");
            _purple = PlayerPrefs.GetInt("Purple");
            _yellow = PlayerPrefs.GetInt("_yellow");
            _green = PlayerPrefs.GetInt("_green");
           

            if (_red == 1)
                UnlockColor(2);
            if (_orange == 1)
                UnlockColor(3);
            if (_purple == 1)
                UnlockColor(4);
            if (_green == 1)
                UnlockColor(5);
            if (_yellow == 1)
                UnlockColor(6);
           
            OnColorChosen(_lastColor);
        }

        public void UnlockColor(int id)
        {
            switch (id)
            {
                case 2:
                    PlayerPrefs.SetInt("Red", 1);
                    _redAd.SetActive(false);
                    _redButton.enabled = true;
                    break;
                case 3:
                    PlayerPrefs.SetInt("Orange", 1);
                    _orangeAd.SetActive(false);
                    _orangeButton.enabled = true;
                    break;
                case 4:
                    PlayerPrefs.SetInt("Purple", 1);
                    _purpleAd.SetActive(false);
                    _purpleButton.enabled = true;
                    break;
                case 5:
                    PlayerPrefs.SetInt("_green", 1);
                    _greenAd.SetActive(false);
                    _greenButton.enabled = true;
                    break;
                case 6:
                    PlayerPrefs.SetInt("_yellow", 1);
                    _yellowAd.SetActive(false);
                    _yellowButton.enabled = true;
                    break;
            }
        }

        public void OnAdClick(int id)
        {
            YandexGame.RewVideoShow(id);
        }

        private void OnAdWatched(int id)
        {
            switch (id)
            {
                case 2:
                    UnlockColor(2);
                    break;
                case 3:
                    UnlockColor(3);
                    break;
                case 4:
                    UnlockColor(4);
                    break;
            }
        }

        public void OnColorChosen(int id)
        {
            _click.Play();
            PlayerPrefs.SetInt("Last", id);
            switch (id)
            {
                case 1:
                    _shell.shellColor = _blueColor;
                    ParticleColor = _blueColor;
                    
                    break;
                case 2:

                    _shell.shellColor  = _redColor;
                    ParticleColor  = _redColor;
                    break;
                case 3:

                    _shell.shellColor = _orangeColor;
                    ParticleColor = _orangeColor;
                    break;
                case 4:

                    _shell.shellColor  = _purpleColor;
                    ParticleColor  = _purpleColor;
                    break;
                case 5:

                    _shell.shellColor  = _greenColor;
                    ParticleColor  = _greenColor;
                    break;
                case 6:

                    _shell.shellColor  = _yellowColor;
                    ParticleColor  = _yellowColor;
                    break;
            }
        }

        private void OnEnable()
        {
            YandexGame.RewardVideoEvent += OnAdWatched;
        }

        private void OnDisable()
        {
            YandexGame.RewardVideoEvent -= OnAdWatched;
        }
    }
}