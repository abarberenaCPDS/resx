# Modern HTTP Application Security Assessment 

The scope of this security assessment is based on the guidelines and recommended practices from the `OWASP` Foundation and other resources listed in each section. 

The goal is to substantially improve the security of the Web application. With these small but important changes, we will not allow major attack vectors against the website.

## Keys and Certificates 

## Key algorithm 

The first decision is always to choose between 2 options: 

1. RSA: Universally supported
1. ECDSA: Newer, faster, and more secure. 

You can have both deployed at the same time for broader compatibility and performance. However, not all server software supports this kind of configuration.

## Key size

Key size selection is about balancing between security and performance: 

1. For RSA keys use a minimum of 2048 bits. A 3072-bit RSA key is much slower than an ECDSA 256 bit.
1. For ECDSA keys, use a minimum of 256 bits.

## Key Management

All network and application security depends on keeping your private keys private.

Always define regular schedule of key rotation. Below are recommended best practices:

- Restrict access to the keys.
- Always use a passphrase.
- Rotate your private keys: 
    - After the server is compromised
    - When an employee leaves the organization (RIF)
    - When getting a new certificate 

## Certificate sharing 

Certificate sharing simplifies maintenance, but it can often reduce security. 

Servers (sites) that share certificates must also share the keys. 

Sites that share certificates also share certain classes of vulnerability. 

When a certificate is revoked all servers that use it must be updated. 

Beware of overlapping certificate hostnames, even when keys are different. 

## Certificate lifetime 

Short lifetimes are best, issue them for `90` days, not 30. 

Problems identified:
- Without Perfect Forward Secrecy, your private key is a liability when compromised.
- Certificate revocation is not enough, shorter lifetimes are more secure. 

Recommended solutions:
- Automate the renewal process based on rotation frequency.
- Otherwise, renew keys and certificates once a year. 

## HSTS – HTTP Strict Transport Security 

`HSTS` is a standard (RFC 6797) that enforces encryption on the hostname for which is enabled. With this approach, a MITM attacker cannot inject malicious content and is designed to mitigate critical weaknesses in how TLS was historically implemented by browsers. Basically, the server instructs the browser to not establish an unencrypted HTTP connection. It also prevents users overriding certificate errors. 

### Recommendation 

To enable `HSTS`, we need to add a response header to the website: 
 
```sh
"Strict-Transport-Security":"max-age=31536000; includeSubDomains; preload" 
```

I recommend starting with a short policy duration `(max-age)` of 300 seconds and validate the content is good; then increase the duration over time. 

To complete the deployment, we will also need a permanent traffic redirection from `HTTP` (port `80`) to `HTTPS` (port `443`). This can be fixed by instructing the server to use the following HTTP code: 

```h
HTTP 301 Moved Permanently 
```

Currently, the server does its best serving an `HTTP 302 Moved Temporarily` header. 

## CSP – Content Security Policy 

`CSP` is a declarative security mechanism that allows to control the behavior of compliant browsers. The main goal of CSP is to avoid `XSS`. The control is expressed on what features are enabled and where the content is downloaded from. CSP blocks the used of mixed content (active and passive mixed content). 

The policy reduces the attack surface on completely disable inline JavaScript and control where the external code is loaded from. It also __disables dynamic code evaluation__ and XSS becomes more difficult to perform. CSP prevents loading of plaintext resources. 

### Recommendation

Crafting a policy that as a minimum: 

1. Explicitly identifies the source domain of the resources to be loaded. 

1. Disable CSS inline styling. 

1. Disable inline JavaScript. 

Below is a suggestion for the web application: 
 
```sh
"Content-Security-Policy":"default-src https: 'unsafe-inline' 'unsafe-eval' data:; connect-src https: wss:;" 
```

Optionally, enable reporting of the `CSP` policy would be ideal to have detailed diagnostic information of the policy and its behavior. 

```sh
"Content-Security-Policy-Report-Only":"default-src 'self'", 
```
 
## SRI – Sub resource Integrity 

When a site relies in on resources that are hosted on third party websites, encryption is not enough. A defense is needed in case those servers which we don’t control are compromised and malware is injected into their files, such as _JavaScript_ libraries. 

`SRI` is a standard that enables you to associate a cryptographic hash with such remote resources and ask browsers to verify them on every retrieval. 

### Recommendation 

Protect all internal and external assets such as JS and CSS files using a SHA 256 hash. An example of a protected script would be: 

 ```html
<link 
    rel="stylesheet" href=https://fonts.googleapis.com/css?family=Roboto%3A300%2C400%2C500%2C700&display=swap 
    integrity="sha256-I+BxHzPOV6XIP6mSFShwQOOYElrin+xIO5hXGgCL/cE=" 
    crossorigin="anonymous"
> 
```

## Secure Cookies

`Cookies` are the __weakest link__ in the HTTP world. Even a website that is 100% encrypted may remain insecure due to misconfigured cookies. 

You may refer to IETF 6265 - HTTP State Management Mechanism for more information about cookies’ historical infelicities. 

### Mark cookies as secure 

Cookies may be used in both HTTP and HTTPS, therefore, explicitly mark them as secure so browsers know they need to avoid plain text. 

### Mark cookies as HttpOnly. 

This flag tells the browser to prevent client side scripting from accesing the cookie. 

### Use cookie name prefixes 

Name prefixes are a new security measure that is widely supported by browsers. The standard specification can be found as RFC 6265bis. Essentially, cookies with names that start with the prefixes: 

- `__Host-`  
- `__Secure-` 

Are treated specially by browsers to address the common problems. All cookies must be transitioned using these naming prefixes. 

The goal is to substantially improve the security of the web application. With these small but important changes, we __will not allow major attack vectors__ against the website. 

### `__Secure `

When using this prefix follow these 2 requirements: 

Set with a Secure attribute. 

Set from a URI whose scheme is considered secure by the uzer agent. 

This means that if you set the cookie without the secure flag, the browser will reject it. See the specification for a valid example.  

### `__Host`

This prefix is much more restrictive than the `__Secure` prefix and offers additional protections. 

When using this prefix follow these 4 requirements: 

- Set with a `Secure` attribute 
- Set from a URI whose `scheme` is considered **secure** by the user agent. 
- Sent only to the host which set the cookie.  That is, a cookie named `__Host-cookie1` set from "https://example.com" **MUST NOT** contain a `Domain` attribute (and will therefore be sent only to "example.com", and not to "subdomain.example.com"). 
- Sent to every request for a host.  That is, a cookie named "__Host-cookie1" **MUST** contain a `Path` attribute with a value of `"/"`. 

See the [specification](https://datatracker.ietf.org/doc/html/draft-ietf-httpbis-cookie-prefixes-00#section-3.2) for a valid example.

### No ‘perfect cookie’ 

The best possile cookie you can make (or bake if you prefer) is:

```sh
__Host-session=id123; path=/; Secure; HttpOnly; SameSite=Lax 
```

This means that the `__Host` prefix indicates the secure flag must be set and server from a trusted host, that there is no Domain attribute and the path is /. HttpOnly is for protection against XSS and SameSite is enabled in lax mode. 
 
## References 

- [WSTG - v4.1 | OWASP Test HTTP Strict Transport Security](https://owasp.org/www-project-web-security-testing-guide/v41/4-Web_Application_Security_Testing/02-Configuration_and_Deployment_Management_Testing/07-Test_HTTP_Strict_Transport_Security)
- [HTTP Headers - OWASP Cheat Sheet Series](https://cheatsheetseries.owasp.org/cheatsheets/HTTP_Headers_Cheat_Sheet.html)
- [HTTP Strict Transport Security - OWASP Cheat Sheet Series](https://cheatsheetseries.owasp.org/cheatsheets/HTTP_Strict_Transport_Security_Cheat_Sheet.html)
- [Content Security Policy - OWASP Cheat Sheet Series](https://cheatsheetseries.owasp.org/cheatsheets/Content_Security_Policy_Cheat_Sheet.html)
- [Subresource Integrity - Security on the web | MDN (mozilla.org)](https://developer.mozilla.org/en-US/docs/Web/Security/Subresource_Integrity)
- [Subresource Integrity - The GitHub Blog](https://github.blog/2015-09-19-subresource-integrity/)
- [Subresource Integrity Sample (googlechrome.github.io)](https://googlechrome.github.io/samples/subresource-integrity/index.html)