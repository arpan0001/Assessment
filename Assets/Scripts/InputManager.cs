using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace InspectionSystem.Interaction
{
    
    // Handles mouse and touch input.
   
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

        
        // Detects mouse clicks.
        
        private void HandleMouseInput()
        {
            
            if (Mouse.current == null)
                return;

            
            if (!Mouse.current.leftButton .wasPressedThisFrame) 
                return;

            ProcessSelection(Mouse.current.position.ReadValue());
        }

        
        //Detects screen touches.
      
        private void HandleTouchInput()
        {
            
            if (Touchscreen.current == null)
                return;

            var touch = Touchscreen.current.primaryTouch;

            if (!touch.press.wasPressedThisFrame)
                return;

            ProcessSelection( touch.position.ReadValue());
        }

        
        // Selects an object at the given screen position.
        
        private void ProcessSelection( Vector2 screenPosition)
        {
           
            if (IsPointerOverUI())
                return;

            
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

            
            interactable.Select();
        }

       
        // Returns true if pointer is over UI.
        
        private bool IsPointerOverUI()
        {
            if (EventSystem.current == null)
                return false;

            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}