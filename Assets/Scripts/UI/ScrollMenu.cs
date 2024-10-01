using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
using UnityEngine;
using UnityEngine.InputSystem;

public class ScrollMenu : MonoBehaviour
{
    public RectTransform scrollPanel;   // The RectTransform of the settings menu to be scrolled
    public float scrollSpeed = 1.0f;    // Speed of scrolling
    public float inertia = 0.95f;       // Inertia for smooth stopping
    public float maxScroll = 1000f;      // Maximum scroll distance in positive direction
    public float minScroll = -500f;     // Minimum scroll distance in negative direction

    private Vector2 dragStartPos;       // Start position for dragging (touch or mouse)
    private float scrollVelocity = 0f;  // Scroll velocity for smooth scrolling
    private bool isDragging = false;    // Whether the user is dragging

    private bool isTouchInput = false;  // To differentiate between mouse and touch input

    private void Update()
    {
        HandleTouchInput();
        HandleMouseInput();

        // Handle inertial scrolling when not dragging
        if (!isDragging)
        {
            if (Mathf.Abs(scrollVelocity) > 0.01f)
            {
                Scroll(scrollVelocity * Time.deltaTime);
                scrollVelocity *= inertia; // Gradually reduce the velocity
            }
            else
            {
                scrollVelocity = 0f;
            }
        }
    }

    private void HandleTouchInput()
    {
        if (Touchscreen.current == null) return; // Ensure Touchscreen is available

        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();

            if (!isDragging)
            {
                // On touch start, record the initial touch position
                dragStartPos = touchPos;
                isDragging = true;
                scrollVelocity = 0f;  // Stop any inertia while dragging
                isTouchInput = true;
            }
            else
            {
                // While dragging, calculate the delta and apply scrolling
                Vector2 delta = touchPos - dragStartPos;
                dragStartPos = touchPos; // Update the touch position

                Scroll(delta.y * scrollSpeed * Time.deltaTime); // Apply scrolling
            }
        }
        else if (isDragging && isTouchInput)
        {
            // On touch end, apply inertia to keep scrolling smoothly
            isDragging = false;
            Vector2 touchEndPos = Touchscreen.current.primaryTouch.position.ReadValue();
            scrollVelocity = (touchEndPos.y - dragStartPos.y) * scrollSpeed;
            isTouchInput = false;
        }
    }

    private void HandleMouseInput()
    {
        if (Mouse.current == null) return; // Ensure Mouse is available

        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            if (!isDragging)
            {
                // On mouse drag start, record the initial mouse position
                dragStartPos = mousePos;
                isDragging = true;
                scrollVelocity = 0f;  // Stop any inertia while dragging
                isTouchInput = false;
            }
            else
            {
                // While dragging, calculate the delta and apply scrolling
                Vector2 delta = mousePos - dragStartPos;
                dragStartPos = mousePos; // Update the mouse position

                Scroll(delta.y * scrollSpeed * Time.deltaTime); // Apply scrolling
            }
        }
        else if (isDragging && !isTouchInput)
        {
            // On mouse drag end, apply inertia to keep scrolling smoothly
            isDragging = false;
            Vector2 mouseEndPos = Mouse.current.position.ReadValue();
            scrollVelocity = (mouseEndPos.y - dragStartPos.y) * scrollSpeed;
        }
    }



        private void Scroll(float amount)
        {
            // Get the current position of the scroll panel
            Vector3 currentPosition = scrollPanel.anchoredPosition;

            // Clamp the position within the scroll limits
            float newY = Mathf.Clamp(currentPosition.y + amount, minScroll, maxScroll);

            // Apply the new position to the scroll panel
            scrollPanel.anchoredPosition = new Vector3(currentPosition.x, newY, currentPosition.z);
        }
    }
}
