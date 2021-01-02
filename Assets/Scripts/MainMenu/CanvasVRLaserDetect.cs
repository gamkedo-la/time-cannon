using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasVRLaserDetect : MonoBehaviour
{
    public Text debugOut;
    Canvas siblingCanv;
    Camera usingCam;
    Button lastSeen;
    bool pressLock = false;

    private void Start() {
        siblingCanv = GetComponent<Canvas>();
        usingCam = siblingCanv.worldCamera;
        // debugOut.text = "none";

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    void Update()
    {
        // adapted from an approach found at https://answers.unity.com/questions/1202359/raycast-against-ui-in-world-space.html
        bool IsFocused = false;
        Button wasLast = lastSeen;
        lastSeen = null;
        Vector2 center = usingCam.ViewportToScreenPoint(Vector2.one / 2);
        var graphics = GraphicRegistry.GetGraphicsForCanvas(siblingCanv);
        for (var i = 0; i < graphics.Count; i++) {
            var t = graphics[i].rectTransform;
            bool contains = RectTransformUtility.RectangleContainsScreenPoint(t, center, usingCam);
            if (contains) {
                Button button = graphics[i].GetComponent<Button>();
                Image img = graphics[i].GetComponent<Image>();
                if (button != null && (img == null || img.raycastTarget)) {
                    // Debug.Log($"name: {t.name}");
                    lastSeen = button;
                    // debugOut.text = t.name;
                } else {
                    contains = false;
                }
            }
            IsFocused |= contains;
        }
        if(lastSeen == null) {
            if(wasLast) {
                EventSystem.current.SetSelectedGameObject(null);
            }
            // debugOut.text = "none";
        } else {
            lastSeen.Select();
            // debugOut.text = "go?";
        }

        if (Input.GetButton("Fire1") || Input.GetAxis("TriggerAxis") > 0.2f) {
            if (pressLock == false) {
                if(lastSeen) {
                    lastSeen.onClick.Invoke();
                }
                pressLock = true;
            }
        } else {
            pressLock = false;
        }
    }
}
