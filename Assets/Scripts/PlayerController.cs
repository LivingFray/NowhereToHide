using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Controller", menuName = "Controllers/Player")]
public class PlayerController : EntityController {

    public float lookSensitivity;

    public override void OnUpdate(Entity entity) {

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

        //Clamp angles
        entity.lookAngle.x = Mathf.Clamp(entity.lookAngle.x, -90.0f, 90.0f);

        if(entity.lookAngle.y > 360.0f) {
            entity.lookAngle.y -= 360.0f;
        }

        if(entity.lookAngle.y < 0.0f) {
            entity.lookAngle.y += 360.0f;
        }

        if (xRot != 0.0f) {
            entity.head.transform.localRotation = Quaternion.Euler(new Vector3(entity.lookAngle.x, 0.0f, 0.0f));
        }
        if (yRot != 0.0f) {
            entity.rigidbody.MoveRotation(Quaternion.Euler(new Vector3(0.0f, entity.lookAngle.y, 0.0f)));
        }
    }

    void MovePlayer(Entity entity) {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 pos = entity.transform.forward * y + entity.transform.right * x;
        pos = pos.normalized * speed;

        if (pos != Vector3.zero) {
            entity.rigidbody.MovePosition(entity.rigidbody.position + pos * Time.fixedDeltaTime);
        }
    }
}
