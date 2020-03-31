import { createUserManager } from 'redux-oidc';


const config  = {
  authority: "http://localhost:5000",
    client_id: "account.service.client",
    redirect_uri: "http://localhost:3000/callback",
    response_type: "token id_token",
    scope:"openid profile api1",
    post_logout_redirect_uri : "http://localhost:3000/index.html",
};

const userManager = createUserManager(config);

export default userManager;