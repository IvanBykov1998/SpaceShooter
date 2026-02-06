using UnityEngine;

namespace Common
{
    public class RotationObject : MonoBehaviour
    {
        void Update()
        {
            transform.Rotate(0, 0, 0.1f);
        }
    }
}