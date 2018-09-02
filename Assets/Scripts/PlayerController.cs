using UnityEngine;

[CreateAssetMenu(fileName = "New Player Controller", menuName = "Controllers/Player")]
public class PlayerController : EntityController {

    public float lookSensitivity;

    public float jumpHeight;

    public float gravity;

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
        Vector3 velocity = entity.rigidbody.velocity;
        Vector3 targetVelocity = entity.transform.forward * y + entity.transform.right * x;
        targetVelocity = targetVelocity.normalized * speed;

        var velocityChange = (targetVelocity - velocity);
        velocityChange.y = 0;
        entity.rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

        if (entity.canJump && Input.GetButtonDown("Jump")) {
            float jump = Mathf.Sqrt(2 * jumpHeight * gravity);
            entity.rigidbody.velocity = new Vector3(velocity.x, jump, velocity.z);
        }
        entity.velocity = entity.rigidbody.velocity;
    }
}
