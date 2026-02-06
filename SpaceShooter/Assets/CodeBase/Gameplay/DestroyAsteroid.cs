using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class DestroyAsteroid : MonoBehaviour
    {
        [SerializeField] private GameObject m_Asteroid;
        [SerializeField] private GameObject m_Destroy;

        private void Start()
        {
            m_Asteroid.GetComponent<Destructible>().EventOnDeath.AddListener(OnDestroyAsteroid);
        }

        private void OnDestroyAsteroid()
        {
            Vector3 posDestroy = m_Asteroid.transform.position;
            var destroy = Instantiate(m_Destroy, posDestroy, Quaternion.identity);
            Destroy(destroy, 2f);
        }
    }
}