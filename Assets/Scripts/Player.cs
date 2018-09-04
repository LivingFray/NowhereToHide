using UnityEngine;
using UnityEngine.UI;

public class Player : Entity {
    bool mouseLocked;

    public GameObject crosshair;
    RectTransform crossTrans;
    public Text ammo;
    public Text health;
    public Image hurt;

    public GameObject specCamera;
    public GameObject playerCamera;
    public GameObject xrayCamera;

    // Use this for initialization
    protected override void OnStart () {
        mouseLocked = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crossTrans = crosshair.GetComponent<RectTransform>();
    }

    protected override void OnUpdate() {
        //Lock mouse to window
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
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }
        //Handle resizing crosshair
        if (crossTrans.localScale.x > 1.0f) {
            crossTrans.localScale -= new Vector3(1.0f, 1.0f, 1.0f) * Time.deltaTime;
            if(crossTrans.localScale.x < 1.0f) {
                crossTrans.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
        //Handle hurt display
        if (hurt.color.a > 0.0f) {
            hurt.color = new Color(hurt.color.r, hurt.color.g, hurt.color.b, hurt.color.a - Time.deltaTime);
        }
        //UI
        ammo.text = equippedGun.Ammo + "/" + equippedGun.Clip;
        health.text = Health.ToString();
        //Xray camera
        isXRaying = Health > 0 && Input.GetButton("Fire2");
        xrayCamera.SetActive(isXRaying);
    }

    public override void OnHit() {
        crossTrans.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    public override void OnDied() {
        playerCamera.SetActive(false);
        specCamera.SetActive(true);
    }

    public override bool Damage(int damage) {
        hurt.color = new Color(hurt.color.r, hurt.color.g, hurt.color.b, 0.25f);
        return base.Damage(damage);
    }

    public override void OnRespawn() {
        base.OnRespawn();
        playerCamera.SetActive(true);
        specCamera.SetActive(false);
    }
}
