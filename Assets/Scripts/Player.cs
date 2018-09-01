using UnityEngine;
using UnityEngine.UI;

public class Player : Entity {
    bool mouseLocked;

    public GameObject crosshair;
    RectTransform crossTrans;
    public Text ammo;

    // Use this for initialization
    protected override void OnStart () {
        mouseLocked = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crossTrans = crosshair.GetComponent<RectTransform>();
    }

    protected override void OnUpdate() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            mouseLocked = !mouseLocked;
            if (mouseLocked) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            } else {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        //Handle resizing crosshair
        if(crossTrans.localScale.x > 1.0f) {
            crossTrans.localScale -= new Vector3(1.0f, 1.0f, 1.0f) * Time.deltaTime;
            if(crossTrans.localScale.x < 1.0f) {
                crossTrans.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
        ammo.text = equippedGun.Ammo + "/" + equippedGun.Clip;
    }

    public override void OnHit() {
        crossTrans.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }
}
