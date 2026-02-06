using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class LivesIndicator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Text;
        [SerializeField] private Image m_Icon;

        private float lastNumLives;

        private void Start()
        {
            m_Icon.sprite = Player.Instance.ActiveShip.PrewiewImage;
        }

        private void Update()
        {
            int numLives = Player.Instance.NumLives;

            if (lastNumLives != numLives)
            {
                m_Text.text = numLives.ToString();
                lastNumLives = numLives;
            }
        }
    }
}