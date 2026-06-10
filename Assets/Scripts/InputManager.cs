using UnityEngine;
using UnityEngine.EventSystems;

namespace InspectionSystem.Interaction
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera;

        private void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            HandleMouseInput();
#else
            HandleTouchInput();
#endif
        }

        private void HandleMouseInput()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            ProcessSelection(
                Input.mousePosition);
        }

        private void HandleTouchInput()
        {
            if (Input.touchCount == 0)
                return;

            Touch touch =
                Input.GetTouch(0);

            if (touch.phase != TouchPhase.Began)
                return;

            ProcessSelection(
                touch.position);
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
                hit.collider.GetComponent<InteractableObject>();

            if (interactable == null)
                return;

            interactable.Select();
        }

        private bool IsPointerOverUI()
        {
#if UNITY_EDITOR || UNITY_STANDALONE

            return EventSystem.current
                .IsPointerOverGameObject();

#else

            if (Input.touchCount > 0)
            {
                return EventSystem.current
                    .IsPointerOverGameObject(
                        Input.GetTouch(0).fingerId);
            }

            return false;

#endif
        }
    }
}   