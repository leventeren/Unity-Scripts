private void CallFBInit()
    {
        FB.Init(OnInitComplete, OnHideUnity);

    }

    private void OnInitComplete()
    {
        if (FB.IsLoggedIn) 
        {
            Debug.Log ("Loggedin userid:  " + FB.UserId);   
            return;
        } 

    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)                                                                        
        {                                                                                        
            // pause the game - we will need to hide                                             
            Time.timeScale = 0;                                                                  
        }                                                                                        
        else                                                                                     
        {                                                                                        
            // start the game back up - we're getting focus again                                
            Time.timeScale = 1;                                                                  
        } 
    }

    private void LoginFB()
    {
        FB.Login("email,public_profile, user_friends", LoginCallback);
    }

    private void LogoutFB()
    {
        if (FB.IsLoggedIn) 
        {
            FB.Logout();

        }
    }


    private void LoginCallback(FBResult result)
    {
        // Call Cognito Login for FB as well
        AWSManager.FacebookLoginCallback (result);

        if (result.Error != null)
        {

        }
        else if (!FB.IsLoggedIn) 
        {

        } 
        else if(FB.IsLoggedIn)
        {


        }

    }

    public void OnClickedOnFBButton()
    {
        //Debug.Log ("clicked on facebook button");
        if(FB.IsLoggedIn)
        {
            return;
        }

        LoginFB ();
    }
