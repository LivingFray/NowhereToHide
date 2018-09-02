using UnityEngine;

[CreateAssetMenu(fileName = "New Player Controller", menuName = "Controllers/Player")]
public class PlayerController : EntityController {

    public float lookSensitivity;

    public float jumpForce;

    public override void OnUpdate(Entity entity) {
        entity.equippedGun.triggerHeld = Input.GetButton("Fire1");
        if(Input.GetButtonDown("Reload")) {
            entity.equippedGun.Reload();
        }
    }

    public override void OnFixedUpdate(Entity entity) {
        MovePlayer(entity);
        MoveCamera(entity);
    }

    public override void OnDied(Entity entity) {
        Debug.Log("DEADED");
    }

    void MoveCamera(Entity entity) {
        //Look side to side
        float yRot = Input.GetAxisRaw("Mouse X");

        float xRot = -Input.GetAxisRaw("Mouse Y");
        entity.lookAngle += new Vector3(xRot, yRot, 0) * lookSensitivity;

        UpdateRotation(entity);
    }

    void MovePlayer(Entity entity) {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 pos = entity.transform.forward * y + entity.transform.right * x;
        pos = pos.normalized * speed;

        if(entity.canJump && Input.GetButtonDown("Jump")) {
            entity.rigidbody.AddForce(entity.transform.up * jumpForce, ForceMode.VelocityChange);
        }

        if (pos != Vector3.zero) {
            entity.rigidbody.MovePosition(entity.rigidbody.position + pos * Time.fixedDeltaTime);
        }
    }
}
