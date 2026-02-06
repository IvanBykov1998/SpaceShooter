using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class EnergyBar : MonoBehaviour
    {
        [SerializeField] private Image m_Image;

        private float lastNumEnergy;

        private void Update()
        {
            float numEnergy = ((float) Player.Instance.ActiveShip.PrimaryEnergy / (float) Player.Instance.ActiveShip.MaxEnergy);

            if (numEnergy != lastNumEnergy)
            {
                m_Image.fillAmount = numEnergy;
                lastNumEnergy = numEnergy;
            }
        }
    }
}