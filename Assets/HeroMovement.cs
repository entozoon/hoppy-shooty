using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HeroMovement : MonoBehaviour {
  public float accelerationHorizontal;
  public float dragHorizontalFactor;
  public float speedTurn;
  public float speedHorizontalMax;
  public float accelerationJump;
  private bool applyForce;
  private Rigidbody rigidBody;
  void Start() {
    rigidBody = GetComponent<Rigidbody>();
  }
  void Update() {
    // applyForce = false;
    if (Input.GetKey(KeyCode.Space)) {
      // applyForce = true;
      rigidBody.AddRelativeForce(0, accelerationJump, 0);
    }
    float inputHorizontal = Input.GetAxis("Horizontal");
    rigidBody.transform.Rotate(Vector3.up, inputHorizontal * speedTurn);
  }
  // Specifically for physics calculations
  void FixedUpdate() {
    float inputVertical = Input.GetAxis("Vertical");
    // Debug.Log("inputVertical = " + inputVertical);
    // rigidBody.AddRelativeForce(0, 0, speedHorizontal * inputVertical);
    // Thrust (horizontal)
    Vector3 forceHorizontal = Vector3.forward * accelerationHorizontal * inputVertical;
    forceHorizontal.y = 0f;
    rigidBody.AddRelativeForce(forceHorizontal);
    // if (rigidBody.velocity.magnitude > speedHorizontalMax) {
    // rigidBody.velocity = rigidBody.velocity.normalized * speedHorizontalMax;
    // }
    // Max speed (horizontal)
    float speedHorizontal = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z).magnitude;
    if (speedHorizontal > speedHorizontalMax) {
      // Tone down the horizontal velocities based on speedHorizontalMax
      float slowFactor = 1 - ((speedHorizontal - speedHorizontalMax) / speedHorizontal);
      // Debug.Log("slowFactor = " + slowFactor);
      rigidBody.velocity = new Vector3(rigidBody.velocity.x * slowFactor, rigidBody.velocity.y, rigidBody.velocity.z * slowFactor);
    } else {
      // Drag (horizontal)
      rigidBody.velocity = new Vector3(rigidBody.velocity.x * dragHorizontalFactor, rigidBody.velocity.y, rigidBody.velocity.z * dragHorizontalFactor);
      // Also use some PhysicsMaterial with surface drag settings
    }
    // Trying to drag horizontal without affecting vertical
    // Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized * speedHorizontal;
    // Vector3 clampedVelocity = Vector3.ClampMagnitude(targetVelocity - transform.InverseTransformDirection(rigidBody.velocity), speedHorizontalMax);
    // clampedVelocity.y = 0f;
    // rigidBody.AddRelativeForce(clampedVelocity);
    // Drag? Avoid the setting as it's both horizontal/vertical. Either do it here like:
    // rigidBody.velocity.x *= .9;
    // Or use PhysicMaterial on surfaces *
  }
}