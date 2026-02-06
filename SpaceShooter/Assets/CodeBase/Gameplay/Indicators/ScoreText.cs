using UnityEngine;
using TMPro;

namespace SpaceShooter
{
    public class ScoreText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Text;

        private float lastScore;

        private void Update()
        {
            int score = Player.Instance.Score;

            if (lastScore != score)
            {
                m_Text.text = "Score: " + score.ToString();
                lastScore = score;
            }
        }
    }
}