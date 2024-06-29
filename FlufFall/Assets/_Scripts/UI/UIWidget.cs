using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

namespace _Scripts.UI
{
    public class UIWidget : MonoBehaviour
    {
        [SerializeField] private int _score;
        [SerializeField] private int _best;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _scoreTextShadow;
        [SerializeField] private TextMeshProUGUI _bestText;
        [SerializeField] private TextMeshProUGUI _bestTextShadow;
        [SerializeField] private GameObject _bestContainer;
        [SerializeField] private GameObject _restartContainer;
        [SerializeField] private GameObject _shopContainer;
        [SerializeField] private GameObject _joyStick;
        [SerializeField] private Animator _animator;
        [SerializeField] private Shop _shop;
        [SerializeField] private AudioSource _transition;
        private bool _dead;

        private void Start()
        {
            _animator.Play("hide");
            _transition.Play();
            _score = 0;
            UpdateScore();
            _best = PlayerPrefs.GetInt("Best");
            _bestText.text = _best.ToString();
            _bestTextShadow.text = _best.ToString();
        }
        private void OnStart()
        {
            _bestContainer.SetActive(false);
            _shopContainer.SetActive(false);
            _joyStick.SetActive(true);
        }

        public void Restart()
        {
            YandexGame.FullscreenShow();
            _animator.Play("show");
            _transition.Play();
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnDie()
        {
            _dead = true;
            if (_best < _score)
            {
                _best = _score;
                PlayerPrefs.SetInt("Best", _best);
            }
            _bestContainer.SetActive(true);
            _bestText.text = _best.ToString();
            _bestTextShadow.text = _best.ToString();
            _joyStick.SetActive(false);
            _restartContainer.SetActive(true);
        }
        private void AddScore()
        {
            if (_dead) return;
            if (!ProcLevelCreator.Started) return;
            _score++;
            UpdateScore();
        }

        private void UpdateScore()
        {
            _scoreText.text = _score.ToString();
            _scoreTextShadow.text = _score.ToString();
            if (_score == 50)
            {
                _shop.UnlockColor(5);
            }
            if (_score == 100)
            {
                _shop.UnlockColor(6);
            }
        }
        private void OnEnable()
        {
            ScoreObserver.OnScore += AddScore;
            PlayerDeathObserver.OnDie += OnDie;
            ProcLevelCreator.OnStart += OnStart;
        }

     

        private void OnDisable()
        {
            ScoreObserver.OnScore -= AddScore;
            PlayerDeathObserver.OnDie -= OnDie;
            ProcLevelCreator.OnStart -= OnStart;
        }

       
    }
}