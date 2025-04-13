using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PhoenixFlame
{
    public class Particle_Manager : MonoBehaviour
    {
        [Header("References")]

        public Animator animator;
        public TextMeshProUGUI buttonText;

        private bool isFadedOut = false;

        void Start()
        {
            UpdateButtonText();
        }

        public void ToggleFade()
        {
            if (isFadedOut)
            {
                animator.SetTrigger("Fade In");
            }
            else
            {
                animator.SetTrigger("Fade Out");
            }

            isFadedOut = !isFadedOut;
            UpdateButtonText();
        }

        private void UpdateButtonText()
        {
            if (buttonText != null)
            {
                buttonText.text = isFadedOut ? "Fade In" : "Fade Out";
            }
        }
    }
}
