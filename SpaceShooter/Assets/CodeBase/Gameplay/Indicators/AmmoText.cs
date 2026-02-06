using UnityEngine;
using TMPro;

namespace SpaceShooter
{
    public class AmmoText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Text;

        private float lastNumAmmo;

        private void Update()
        {
            int numAmmo = Player.Instance.ActiveShip.SecondaryAmmo;

            if (lastNumAmmo != numAmmo)
            {
                m_Text.text = numAmmo.ToString();
                lastNumAmmo = numAmmo;
            }
        }
    }
}