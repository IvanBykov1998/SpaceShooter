using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class ShipInputController : MonoBehaviour
    {
        public enum ControlMode
        {
            Multiple,
            Keyboard,
            Mobile
        }

        [SerializeField] private ControlMode m_ControlMode;

        public void Construct(VirtualGamePad virtualGamePad)
        {
            m_VirtualGamePad = virtualGamePad;
        }

        private SpaceShip m_TargetShip;
        public void SetTargetShip(SpaceShip ship) => m_TargetShip = ship;

        private VirtualGamePad m_VirtualGamePad;

        private void Start()
        {
            if (m_ControlMode == ControlMode.Keyboard)
            {
                m_VirtualGamePad.VirtualJoystick.gameObject.SetActive(false);

                m_VirtualGamePad.MobileFirePrimary.gameObject.SetActive(false);
                m_VirtualGamePad.MobileFireSecondary.gameObject.SetActive(false);
            }
            else
            {
                m_VirtualGamePad.VirtualJoystick.gameObject.SetActive(true);

                m_VirtualGamePad.MobileFirePrimary.gameObject.SetActive(true);
                m_VirtualGamePad.MobileFireSecondary.gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            if (m_TargetShip == null) return;

            if (m_ControlMode == ControlMode.Keyboard)
                ControlKeyboard();

            if (m_ControlMode == ControlMode.Mobile)
                ControlMobile();

            if (m_ControlMode == ControlMode.Multiple)
                ControlMultiple();
        }

        private void ControlMultiple()
        {
            ControlMobile();

            if ((m_TargetShip.ThrustControl == 0) && (m_TargetShip.TorqueControl == 0))
                ControlKeyboard();
        }

        private void ControlMobile()
        {
            var dir = m_VirtualGamePad.VirtualJoystick.Value;
            m_TargetShip.ThrustControl = dir.y;
            m_TargetShip.TorqueControl = -dir.x;

            if (m_VirtualGamePad.MobileFirePrimary.IsHold == true)
            {
                m_TargetShip.Fire(TurretMode.Primary);
            }

            if (m_VirtualGamePad.MobileFireSecondary.IsHold == true)
            {
                m_TargetShip.Fire(TurretMode.Secondary);
            }
        }

        private void ControlKeyboard()
        {
            float thrust = 0;
            float torque = 0;

            if (Input.GetKey(KeyCode.UpArrow))
                thrust = 1.0f;

            if (Input.GetKey(KeyCode.DownArrow))
                thrust = -1.0f;

            if (Input.GetKey(KeyCode.LeftArrow))
                torque = 1.0f;

            if (Input.GetKey(KeyCode.RightArrow))
                torque = -1.0f;

            if (Input.GetKey(KeyCode.Space))
            {
                m_TargetShip.Fire(TurretMode.Primary);
            }

            if (Input.GetKey(KeyCode.X))
            {
                m_TargetShip.Fire(TurretMode.Secondary);
            }

            m_TargetShip.ThrustControl = thrust;
            m_TargetShip.TorqueControl = torque;
        }
    }
}