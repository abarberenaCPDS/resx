function callApi(endpoint, token) {


  const bearer = `Bearer ${token}`;

  console.log('Calling web API...');
  axios.get(endpoint, { headers: { Authorization: bearer } })
    .then(response => {

      if (response) {
        console.log('Web API responded: ' + response.name);
      }

      var r = response.data;
      console.log(r)

      return r;

    }).catch(error => {
      console.error(error);
    });
}