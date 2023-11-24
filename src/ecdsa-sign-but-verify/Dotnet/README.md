# ECDSA Sign & Verify

The **ECDSA** (Elliptic Curve Digital Signature Algorithm) is a cryptographically secure digital signature that retains the cryptographic security features associated with digital signature.

This deterministic usage of the *Digital Signature Algorithm* is commonly implemented to Sign and Verify content such as documents, files and streams to mention a few.

## Steps Followed

#### Create a 256-bit ECDSA key using the P-256 (aka __secp256r1__) named curve.

```sh
# Generate Private Key
openssl genpkey -algorithm EC -pkeyopt ec_paramgen_curve:P-256 -out prv.pem
```

#### Export the PEM Private key to PEM PKCS#8 format

```sh
# Export PEM PKCS#8 to PEM PKCS#8
openssl pkcs8 -in prv.pem -topk8 -nocrypt -out prv8.pem
```

#### Export the PEM Private key to DER PKCS#8 format

```sh
# Export PEM PKCS#8 to DER PKCS#8
openssl pkcs8 -in prv.pem -topk8 -nocrypt -outform DER -out prv8.der
```

#### Generate PEM Public Key from PEM Private Key PKCS#8

```sh
# Generate PEM PCKS#8 Public Key from PEM PKCS#8 Private Key
openssl ec -in prv8.pem -pubout -out pub8.pem
```

#### Export PEM Public Key to DER Public Key
```sh
# Export PEM PKCS#8 Publick Key to DER PKCS#8 Public Key
openssl ec -pubin -in pub8.pem -outform DER -out pub8.der
```


## Key Generation and Export

## Test Client

The .NET client does **Hashing**, ECDSA **signing** and ECDSA signature **verification** (valid and tampered).

#### Instructions

1. Create the private and public keys
1. Export both keys from `PEM` to `DER` format.
1. Use the DER keys in the code to Sign and Verify both sample json files.

#### Syntax

```sh
# run
dotnet run


# Output

```sh
=== ECDSA - Sign but Verify ===

Content:                sample.json
Private key file:       prv8.der
Private Key loaded:     ECDH_P256 ready.
Signature:              W7SfGJZ/h7apim6UXR4ZZtWi73rK0lv5ILZsMSXhrsCkcUsqx6QqAEtDw2d5TLoThU2m88mKYu0igAgRHOM00w==
Public Key:             MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAE0oHiOSsT2BLTHzEolqkc565lVJVac5x/MdM7raVL4J9Pmf2XEQFn5qRTqLpt32I8mpBMHXNC/Q4xlDJ32UqOkw==

Is Signature valid?             TRUE            sample.json
Is Signature valid?             FALSE           sample-tampered.json

=== ECDSA - Just Verify Signature ===

Signature:              miyXZ6sf+IF2mdvMhOTE4A/WzMyTW4AQbk9FOOEobpryh9UY1VuqapA2sGn0Jov5cLMv66DksO+gH/RJQP2f6A==
Public Key:             MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAE0oHiOSsT2BLTHzEolqkc565lVJVac5x/MdM7raVL4J9Pmf2XEQFn5qRTqLpt32I8mpBMHXNC/Q4xlDJ32UqOkw==

Is Signature valid?             TRUE            sample.json

+++ Program completed +++
```

#### Directory tree

This is where the keys and json files should be placed:

```sh
.
├── README.md
├── Program.cs
# Public and Private Keys located in the EcdsaKeysContainer project
# They are copied to output directory if newer
# Keys in PEM format
    └── prv.pem
    └── prv8.pem
    └── pub8.pem
# Keys in DER format
    └── prv8.der
    └── pub8.der
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