async function callApi(endpoint, token) {
    
    const headers = new Headers();
    const bearer = `Bearer ${token}`;
  
    headers.append("Authorization", bearer);
    headers.append("Content-Type", "application/json");
  
    const options = {
        method: "GET",
        headers: [
          {"Authorization" : bearer},
          // {"Access-Control-Allow-Origin" : '*'},
          {"Content-Type": "application/json"}
        ],
        url: endpoint,
        // responseType: 'stream'
      };

      console.log('Calling web API...');
      await axios(options)
      .then(response => {

        if (response) {
          console.log('Web API responded: ' + response.name);
        }

        var r = response.data.json();
        console.log(r)
        
        return response;
      }).catch(error => {
        console.error(error);
      });
  }