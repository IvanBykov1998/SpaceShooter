using UnityEngine;
using TMPro;

namespace SpaceShooter
{
    public class ResultPanel : MonoBehaviour
    {
        private const string PassedText = "Passed";
        private const string LoseText = "Lose";
        private const string RestartText = "Restart";
        private const string NextLevelText = "NextLevel";
        private const string MainMenuText = "Main menu";
        private const string KillsTextPrefix = "Kills: ";
        private const string ScoreTextPrefix = "Scores: ";
        private const string TimeTextPrefix = "Time: ";


        [SerializeField] private TextMeshProUGUI m_Kills;
        [SerializeField] private TextMeshProUGUI m_Score;
        [SerializeField] private TextMeshProUGUI m_Time;
        [SerializeField] private TextMeshProUGUI m_Result;
        [SerializeField] private TextMeshProUGUI m_ButtonNextText;

        private bool m_LevelPassed = false;

        private void Start()
        {
            gameObject.SetActive(false);

            LevelController.Instance.LevelLost += OnLevelLost;
            LevelController.Instance.LevelPassed += OnLevelPassed;
        }

        private void OnDestroy()
        {
            LevelController.Instance.LevelLost -= OnLevelLost;
            LevelController.Instance.LevelPassed -= OnLevelPassed;
        }

        private void OnLevelPassed()
        {
            gameObject.SetActive(true);

            m_LevelPassed = true;

            FillLevelStatistics();
            m_Result.text = PassedText;

            if (LevelSequencesController.Instance.CurrentLevelIsLast() == true)
            {
                m_ButtonNextText.text = MainMenuText;
            }
            else
            {
                m_ButtonNextText.text = NextLevelText;
            }
        }

        private void OnLevelLost()
        {
            gameObject.SetActive(true);

            FillLevelStatistics();
            m_Result.text = LoseText;
            m_ButtonNextText.text = RestartText;
        }

        private void FillLevelStatistics() // Статистика уровня
        {
            m_Kills.text = KillsTextPrefix + Player.Instance.NumKills.ToString();
            m_Score.text = ScoreTextPrefix + Player.Instance.Score.ToString();
            m_Time.text = TimeTextPrefix + LevelController.Instance.LevelTime.ToString("F0");
        }

        public void OnButtonNextAction()
        {
            gameObject.SetActive(false);

            if (m_LevelPassed == true)
            {
                LevelController.Instance.LoadNextLevel();
            }
            else
            {
                LevelController.Instance.RestartLevel();
            }
        }
    }
} 