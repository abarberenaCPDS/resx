// The current application coordinates were pre-registered in a B2C tenant.
const apiConfig = {
    b2cScopes: ["openid"],
    read: ["openid", "profile", "https://iamvresdnadev001.onmicrosoft.com/app-desktop-eval-bff/read.access"],
    // b2cScopes: ["https://fabrikamb2c.onmicrosoft.com/helloapi/demo.read"],
    // webApi: "http://localhost:60020/api/v1/order/history"
    // webApi: "http://localhost:4000/desktopeval/api/v1/order/history"
    webApi: "http://localhost:4000/bff/api/order/history"
  };