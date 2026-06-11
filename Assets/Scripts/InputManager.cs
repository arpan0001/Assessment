using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace InspectionSystem.Interaction
{
    /// <summary>
    /// Handles mouse and touch input.
    /// Detects object selection using raycasts.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField]
        private Camera mainCamera;

        // Check for input every frame
        private void Update()
        {
            HandleMouseInput();
            HandleTouchInput();
        }

        /// <summary>
        /// Detects mouse clicks.
        /// </summary>
        private void HandleMouseInput()
        {
            // Exit if no mouse is available
            if (Mouse.current == null)
                return;

            // Only react when left mouse button is pressed
            if (!Mouse.current.leftButton .wasPressedThisFrame) 
                return;

            ProcessSelection(Mouse.current.position.ReadValue());
        }

        /// <summary>
        /// Detects screen touches.
        /// </summary>
        private void HandleTouchInput()
        {
            // Exit if no touchscreen is available
            if (Touchscreen.current == null)
                return;

            var touch = Touchscreen.current.primaryTouch;

            // Only react when touch begins
            if (!touch.press.wasPressedThisFrame)
                return;

            ProcessSelection( touch.position.ReadValue());
        }

        /// <summary>
        /// Selects an object at the given screen position.
        /// </summary>
        private void ProcessSelection( Vector2 screenPosition)
        {
            // Ignore input when clicking UI
            if (IsPointerOverUI())
                return;

            // Create a ray from the camera
            Ray ray = mainCamera.ScreenPointToRay(screenPosition);

            // Check if ray hits an object
            if (!Physics.Raycast(ray,out RaycastHit hit))
            {
                return;
            }

            // Look for an InteractableObject component
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();

            if (interactable == null)
                return;

            // Select the object
            interactable.Select();
        }

        /// <summary>
        /// Returns true if pointer is over UI.
        /// </summary>
        private bool IsPointerOverUI()
        {
            if (EventSystem.current == null)
                return false;

            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}