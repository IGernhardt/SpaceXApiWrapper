##   To Interact With Incidents API:
1) Register the user by using the /api/auth/register endpoint. Use either the "Admin" or "Reader" role in the "role" field of the JSON Request.
2) Login using the /api/auth/login endpoint using the same username and password.
3) Copy the returned JWT token
4) Click the "Authorize" button and input "bearer [JWT Token]" then click authorize.