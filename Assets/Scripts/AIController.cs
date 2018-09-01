using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AI Controller", menuName = "Controllers/AI")]
public class AIController : EntityController {
    public override void OnDied(Entity entity) {
        entity.gameObject.SetActive(false);
    }
}
