using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasVRLaserDetect : MonoBehaviour
{
    public Text debugOut;
    Canvas siblingCanv;
    Camera usingCam;
    Button lastSeen;

    private void Start() {
        siblingCanv = GetComponent<Canvas>();
        usingCam = siblingCanv.worldCamera;
        debugOut.text = "none";
    }
    void Update()
    {
        bool IsFocused = false;
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
                    Debug.Log($"name: {t.name}");
                    lastSeen = button;
                    debugOut.text = t.name;
                } else {
                    contains = false;
                }
            }
            IsFocused |= contains;
        }
        if(lastSeen == null) {
            debugOut.text = "none";
        } else if(Input.GetButton("Fire1")) {
            lastSeen.Select();
            debugOut.text = "pressing";
        }
    }
}
