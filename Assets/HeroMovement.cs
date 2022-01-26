using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HeroMovement : MonoBehaviour {
  public float speedForward;
  public float speedTurn;
  private bool applyForce;
  private Rigidbody rigidBody;
  void Start() {
    rigidBody = GetComponent<Rigidbody>();
  }
  void Update() {
    // applyForce = false;
    // if (Input.GetKey(KeyCode.Space)) {
    //   applyForce = true;
    // }
    float inputHorizontal = Input.GetAxis("Horizontal");
    rigidBody.transform.Rotate(Vector3.up, inputHorizontal * speedTurn);
  }
  // Specifically for physics calculations
  void FixedUpdate() {
    float inputVertical = Input.GetAxis("Vertical");
    // Debug.Log("inputVertical = " + inputVertical);

    rigidBody.AddRelativeForce(0, 0, speedForward * inputVertical);
  }
}