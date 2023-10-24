var config = {
    authority: "https://demo.duendesoftware.com",
    client_id: "interactive.public.short",
    redirect_uri: "http://localhost:5002/callback-oidc.html",
    response_type: "code",
    scope: "openid profile email api offline_access",
    post_logout_redirect_uri: "http://localhost:5002/index-oidc.html",
};

// var config = {
//     authority: "https://dev-6b0mu6pvkqz2xf0v.us.auth0.com/",
//     client_id: "3M90HmrkSs7hcbi1A7C8K4LUMHyEAAPP",
//     redirect_uri: "http://localhost:5002/callback-oidc.html",
//     response_type: "code",
//     scope: "openid profile email api offline_access",
//     post_logout_redirect_uri: "http://localhost:5002/index-oidc.html",
// };


// Oidc.Log.logger = console;

var mgr = new Oidc.UserManager(config);
// console.log('OIDC -->',mgr.user)

mgr.events.addUserSignedOut(function () {
    log("User signed out of IdentityServer");
});

mgr.getUser().then(function (user) {
    if (user) {
        log("\nUser logged in:\n\nProfile:", user.profile);
        document.getElementById('txtIdToken').value = user.id_token
        document.getElementById('txtAccessToken').value = user.access_token
    } else {
        log("User not logged in");
    }
});

function log() {
    document.getElementById("results").innerText = "";

    Array.prototype.forEach.call(arguments, function (msg) {
        if (typeof msg !== "undefined") {
            if (msg instanceof Error) {
                msg = "Error: " + msg.message;
            } else if (typeof msg !== "string") {
                msg = JSON.stringify(msg, null, 2);
            }
            document.getElementById("results").innerText += msg + "\r\n";
        }
    });
}

function signIn() {
    console.log('login');
    mgr.signinRedirect();
}

function api() {
    console.log('api');
    return;
    mgr.getUser().then(function (user) {
        var url = "https://localhost:6001/identity";

        var xhr = new XMLHttpRequest();
        xhr.open("GET", url);
        xhr.onload = function () {
            log(xhr.status, JSON.parse(xhr.responseText));
        };
        xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
        xhr.send();
    });
}

function signOut() {
    console.log('logout');
    // return;
    mgr.signoutRedirect();
}
