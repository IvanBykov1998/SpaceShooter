using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    public class LevelController : SingletonBase<LevelController>
    {
        private const string MainMenuSceneName = "main_menu";

        public event UnityAction LevelPassed; // Событие - уровень пройден
        public event UnityAction LevelLost; // Событие - поражение

        [SerializeField] private LevelCondition[] m_Conditions;

        private bool m_IsLevelComplited;
        private float m_LevelTime;
        private LevelSequencesController m_LevelSequencesController;
        private LevelProperties m_CurrentLevelProperties;

        public float LevelTime => m_LevelTime;

        private void Start()
        {
            Time.timeScale = 1;
            m_LevelTime = 0;

            m_LevelSequencesController = LevelSequencesController.Instance;
            m_CurrentLevelProperties = m_LevelSequencesController.GetCurrentLoadedLevel();
        }

        private void Update()
        {
            if (m_IsLevelComplited == false)
            {
                m_LevelTime += Time.deltaTime;
                CheckLevelConditions();
            }

            if (Player.Instance.NumLives == 0)
            {
                Lose();
            }
        }

        private void CheckLevelConditions() // Проверка на завершение уровня - нужно выполнить все условия m_Conditions[]
        {
            int numComplited = 0;

            for (int i = 0; i < m_Conditions.Length; i++)
            {
                if (m_Conditions[i].IsCompleted == true)
                {
                    numComplited++;
                }
            }

            if (numComplited == m_Conditions.Length)
            {
                m_IsLevelComplited = true;
                Pass();
            }
        }

        private void Lose()
        {
            LevelLost?.Invoke();
            Time.timeScale = 0;
        }

        private void Pass()
        {
            LevelPassed?.Invoke();
            Time.timeScale = 0;
        }

        public void LoadNextLevel() // Загрузка следующего уровня
        {
            if (m_LevelSequencesController.CurrentLevelIsLast() == false)
            {
                string nextLevelSceneName = m_LevelSequencesController.GetNextLevelProperties(m_CurrentLevelProperties).SceneName;

                SceneManager.LoadScene(nextLevelSceneName);
            }
            else
            {
                SceneManager.LoadScene(MainMenuSceneName);
            }
        }

        public void RestartLevel() // Рестарт текущего уровня
        {
            SceneManager.LoadScene(m_CurrentLevelProperties.SceneName);
        }
    }
}