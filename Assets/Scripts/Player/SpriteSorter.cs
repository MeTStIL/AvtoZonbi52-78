using UnityEngine;

namespace Player
{
    public class SpriteSorter : MonoBehaviour
    {
        public bool isStatic;
        private const int SorterOrderBase = 0;
        private new Renderer renderer;

        private void Awake()
        {
            renderer = GetComponent<Renderer>();
        }

        private void LateUpdate()
        {
            renderer.sortingOrder = (int)(SorterOrderBase - transform.position.y);
            if (isStatic)
                Destroy(this);
        }
    }
}
