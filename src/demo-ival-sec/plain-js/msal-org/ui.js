// Select DOM elements to work with
const signInButton = document.getElementById('signIn');
const signOutButton = document.getElementById('signOut')
const titleDiv = document.getElementById('title-div');
const welcomeDiv = document.getElementById('welcome-div');
const tableDiv = document.getElementById('table-div');
const tableBody = document.getElementById('table-body-div');
const editProfileButton = document.getElementById('editProfileButton');
const callApiButton = document.getElementById('callApiButton');
const response = document.getElementById("response");
const label = document.getElementById('label');

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

function welcomeUser(username) {
    if (username) {
        log("\nUser logged in:\n\nProfile:", username);
    } else {
        console.log("User not logged in");
    }
    // welcomeDiv.innerHTML = `Welcome ${username}!`

    // label.classList.add('d-none');
    // signInButton.classList.add('d-none');
    // titleDiv.classList.add('d-none');

    // signOutButton.classList.remove('d-none');
    // editProfileButton.classList.remove('d-none');
    // welcomeDiv.classList.remove('d-none');
    // callApiButton.classList.remove('d-none');
}

function displayTokens(response) {
    document.getElementById('txtIdToken').value = response.idToken
    document.getElementById('txtAccessToken').value = response.accessToken
}

function logMessage(s) {
    response.appendChild(document.createTextNode('\n' + s + '\n'));
}