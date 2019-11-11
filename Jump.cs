IEnumerator DoJump()
 {
      //the initial jump
      playerRigidBody.AddForce(Vector2.Up * jumpForce);
      yield return null;
 
      //can be any value, maybe this is a start ascending force, up to you
      float currentForce = jumpForce;
      
      while(Input.GetKey(KeyCode.Space) && currentForce > 0)
      {
            playerRigidBody.AddForce(Vector2.Up * currentForce);
             
            currentForce -= decayRate * Time.deltaTime;
            yield return null;
      }
 }
 
 void Update()
 {
      if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
      {
            StartCoRoutine(DoJump());
      }
 }
