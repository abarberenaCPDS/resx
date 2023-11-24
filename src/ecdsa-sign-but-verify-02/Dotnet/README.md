# ECDSA Sign & Verify

The **ECDSA** (Elliptic Curve Digital Signature Algorithm) is a cryptographically secure digital signature that retains the cryptographic security features associated with digital signature.

This deterministic usage of the *Digital Signature Algorithm* is commonly implemented to Sign and Verify content such as documents, files and streams to mention a few.

## Key Generation and Export

#### Create a `Private` key

Create a 256-bit ECDSA key using the P-256 (aka __secp256r1__) named curve.

If a _password_ is needed, then use `-aes-128-cbc` at the end.

```sh
# Generate Private Key
openssl genpkey -algorithm EC -pkeyopt ec_paramgen_curve:P-256 -out prv.pem
```

#### Create the `Public` key

To generate the Public key in `PEM` format:

```sh
# Generate the Public key
openssl ec -in prv.pem -pubout -out pub.pem
```

Exporting the Public key to `DER` format:

```sh
# Convert PEM formatted public key to DER
openssl ec -pubin -in pub.pem -inform PEM -outform DER -out pub.der
```

If you wanto to **verify** the conversion:

```sh
openssl asn1parse -inform DER -in pub.der
```

Output:

```sh
    0:d=0  hl=2 l=  89 cons: SEQUENCE          
    2:d=1  hl=2 l=  19 cons: SEQUENCE          
    4:d=2  hl=2 l=   7 prim: OBJECT            :id-ecPublicKey
   13:d=2  hl=2 l=   8 prim: OBJECT            :prime256v1
   23:d=1  hl=2 l=  66 prim: BIT STRING 
```

#### Export key from PEM format to PKCS#8 DER 

To export PEM to PKCS#8 DER so .NET can read it:

```sh
# Export private key to PKCS#8
openssl pkcs8 -in prv.pem -topk8 -inform PEM -outform DER -out prv.der
```

### Optional commands

#### Convert from PEM to DER

```sh
# Export from PEM to DER ASN1 format, this is NOT in PKCS#8
openssl ec -in prv.pem -inform PEM -outform DER -out prv.der
```

## Test Client

The .NET client does **Hashing**, ECDSA **signing** and ECDSA signature **verification** (valid and tampered).

#### Instructions

1. Create the private and public keys
1. Export both keys from `PEM` to `DER` format.
1. Use the DER keys in the code to Sign and Verify both sample json files.

#### Syntax

```sh
# run
dotnet run hasher

# Output
OK: Private Key loaded...: ECDsa ready.
Is Valid? --> TRUE
Is Valid? --> TRUE
```

#### Directory tree

This is where the keys and json files should be placed:

```sh
.
├── README.md
├── Program.cs
# Public and Private keys in PEM format
├── prv.pem
├── pub.pem
├── prv-pwd.pem
├── pub-pwd.pem
# Public and Private keys in DER format
├── prv.der
├── pub.der
├── prv-pwd.der
├── pub-pwd.der
# Sample json files
├── sample-tampered.json
└── sample.json
```

## References

Main References:

1. [Openssl PKCS#8 - Windows .NET 4.x](https://www.openssl.org/docs/man3.0/man1/openssl-pkcs8.html)
1. [ECDSA Microsoft Docs - ImportEncryptedPkcs8PrivateKey](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.asymmetricalgorithm.importencryptedpkcs8privatekey?view=net-8.0&viewFallbackFrom=netframework-4.7.2#system-security-cryptography-asymmetricalgorithm-importencryptedpkcs8privatekey(system-readonlyspan((system-byte))-system-readonlyspan((system-byte))-system-int32@))
1. [ECDsa Class](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.ecdsa?view=netframework-4.7.2)

Additional References:
- [RFC-6979 - Deterministic Usage of the DSA and ECDSA](https://datatracker.ietf.org/doc/html/rfc6979)
- [OpenSSL ecparam](https://www.openssl.org/docs/man1.0.2/man1/ecparam.html)
- [LE - A Warm Welcome to ASN.1 and DER](https://letsencrypt.org/docs/a-warm-welcome-to-asn1-and-der/)
- [GitHub ecdsa-dotnet](https://github.com/starkbank/ecdsa-dotnet)
- [StackOverflow](https://stackoverflow.com/questions/56144550/loading-an-ecc-private-key-in-net)
- [How to load a PEM file](https://www.scottbrady91.com/c-sharp/pem-loading-in-dotnet-core-and-dotnet)