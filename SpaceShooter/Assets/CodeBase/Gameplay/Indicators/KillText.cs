using UnityEngine;
using TMPro;

namespace SpaceShooter
{
    public class KillText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Text;

        private float lastNumKills;

        private void Update()
        {
            int numKills = Player.Instance.NumKills;

            if (lastNumKills != numKills)
            {
                m_Text.text = "Kills: " + numKills.ToString();
                lastNumKills = numKills;
            }
        }
    }
}