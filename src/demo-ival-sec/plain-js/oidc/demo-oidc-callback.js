new Oidc.UserManager({ response_mode: "query", loadUserInfo:true, filterProtocol:true })
    .signinRedirectCallback()
    .then(function (user) {
        console.log(user);
        window.history.replaceState({},
            window.document.title,
            window.location.origin + window.location.pathname);
        window.location = "/index-oidc.html";
    }).catch(function (e) {
        console.error(e);
    });