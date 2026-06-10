using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace InspectionSystem.Interaction
{
    public class InputManager : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField]
        private Camera mainCamera;

        private void Update()
        {
            HandleMouseInput();
            HandleTouchInput();
        }

        private void HandleMouseInput()
        {
            if (Mouse.current == null)
                return;

            if (!Mouse.current.leftButton
                .wasPressedThisFrame)
                return;

            ProcessSelection(
                Mouse.current.position.ReadValue());
        }

        private void HandleTouchInput()
        {
            if (Touchscreen.current == null)
                return;

            var touch =
                Touchscreen.current.primaryTouch;

            if (!touch.press
                .wasPressedThisFrame)
                return;

            ProcessSelection(
                touch.position.ReadValue());
        }

        private void ProcessSelection(
            Vector2 screenPosition)
        {
            if (IsPointerOverUI())
                return;

            Ray ray =
                mainCamera.ScreenPointToRay(
                    screenPosition);

            if (!Physics.Raycast(
                ray,
                out RaycastHit hit))
            {
                return;
            }

            InteractableObject interactable =
                hit.collider.GetComponent<
                    InteractableObject>();

            if (interactable == null)
                return;

            interactable.Select();
        }

        private bool IsPointerOverUI()
        {
            if (EventSystem.current == null)
                return false;

            return EventSystem.current
                .IsPointerOverGameObject();
        }
    }
}