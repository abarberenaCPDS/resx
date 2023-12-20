# ECDSA Sign & Verify

![Veros ECDSA Sign But Verify](project.svg)
![build](build.svg)
![version](version.svg)

The **ECDSA** (Elliptic Curve Digital Signature Algorithm) is a cryptographically secure digital signature that retains the cryptographic security features associated with digital signature.

This deterministic usage of the *Digital Signature Algorithm* is commonly implemented to Sign and Verify content such as documents, files and streams to mention a few.

## ECDSA Private & Public Key 

Here's an example of ECDSA NIST-P256 keys:

#### Private Key

```sh
Private-Key: (256 bit)
priv:
    ff:c6:d1:75:99:2d:92:60:4e:d3:f9:63:dc:20:42:
    86:2d:3d:19:72:87:aa:aa:db:f7:fb:42:01:0f:8c:
    60:91
ASN1 OID: prime256v1
NIST CURVE: P-256

-----BEGIN EC PRIVATE KEY-----
MHcCAQEEIP/G0XWZLZJgTtP5Y9wgQoYtPRlyh6qq2/f7QgEPjGCRoAoGCCqGSM49
AwEHoUQDQgAE0oHiOSsT2BLTHzEolqkc565lVJVac5x/MdM7raVL4J9Pmf2XEQFn
5qRTqLpt32I8mpBMHXNC/Q4xlDJ32UqOkw==
-----END EC PRIVATE KEY-----
```

#### Public Key

```sh
Public-Key: (256 bit)
pub:
    04:d2:81:e2:39:2b:13:d8:12:d3:1f:31:28:96:a9:
    1c:e7:ae:65:54:95:5a:73:9c:7f:31:d3:3b:ad:a5:
    4b:e0:9f:4f:99:fd:97:11:01:67:e6:a4:53:a8:ba:
    6d:df:62:3c:9a:90:4c:1d:73:42:fd:0e:31:94:32:
    77:d9:4a:8e:93
ASN1 OID: prime256v1
NIST CURVE: P-256

-----BEGIN PUBLIC KEY-----
MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAE0oHiOSsT2BLTHzEolqkc565lVJVa
c5x/MdM7raVL4J9Pmf2XEQFn5qRTqLpt32I8mpBMHXNC/Q4xlDJ32UqOkw==
-----END PUBLIC KEY-----
```

## Steps Followed to generate Private and Public ECDSA standard keys

### Create a 256-bit ECDSA key using the P-256 elliptic curve and the __prime256v1__ as signing algorihtm:

```sh
# Step 1: Choose a password to protect the private key
pwd='Passw0rd!1'
```

```sh
# Step 2: Create private key
openssl genpkey -algorithm EC -pkeyopt ec_paramgen_curve:P-256 -aes-128-cbc -pass pass:$pwd -out vselect-devqa-key.pem
```

```sh
# Step 3: Generate the public key
openssl ec -in vselect-devqa-key.pem -pubout -passin pass:$pwd -out vselect-devqa-pub.pem
```

```sh
# Step 4: Use or create your own CSR
 openssl req -new -config csr-cnf.txt -key vselect-devqa-key.pem -out vselect-devqa-csr.pem -passin pass:$pwd

# An example for a csr-cnf.txt file will look like the follwing;
[req]
prompt = no
days = 90
distinguished_name = dn
req_extensions = ext

[dn]
OU = VeroSELECT
CN = VeroSELECT DEV QA Digital Signature 2033
O = Veros Real Estate Solutions LLC
L = Santa Ana
C = US
stateOrProvinceName = California

[ext]

```


```sh
# Step 5: Use the CSR to create a PEM certificate
openssl x509 -req -days 90 -in vselect-devqa-csr.pem -signkey vselect-devqa-key.pem -out vselect-devqa-crt.cer -passin pass:$pwd

# output:
Certificate request self-signature ok
subject=OU = VeroSELECT, CN = VeroSELECT DEV QA Digital Signature 2033, O = Veros Real Estate Solutions LLC, L = Santa Ana, C = US, ST = California
```

```sh
# Step 6: Export the certificate to a PFX file that includes both private and public keys
openssl pkcs12 -export -passout pass:$pwd \
-name "VeroSELECT DEV QA Digital Signature 2033" \
-out vselect-devqa-pfx.p12 \
-inkey vselect-devqa-key.pem -passin pass:$pwd \
-in vselect-devqa-crt.cer
```

```sh
# Finally, confirm the PFX file content
openssl pkcs12 -in  vselect-devqa-pfx.p12 -info -passin pass:$pwd -passout pass:$pwd | less

# output:
MAC: sha256, Iteration 2048
MAC length: 32, salt length: 8
PKCS7 Encrypted data: PBES2, PBKDF2, AES-256-CBC, Iteration 2048, PRF hmacWithSHA256
Certificate bag
Bag Attributes
    localKeyID: 53 1F D7 F6 D6 55 2A 20 92 FB 8D 86 22 8F AD 2C 8C 0E 7F 12
    friendlyName: VeroSELECT DEV QA Digital Signature 2033
subject=OU = VeroSELECT, CN = VeroSELECT DEV QA Digital Signature 2033, O = Veros Real Estate Solutions LLC, L = Santa Ana, C = US, ST = California
issuer=OU = VeroSELECT, CN = VeroSELECT DEV QA Digital Signature 2033, O = Veros Real Estate Solutions LLC, L = Santa Ana, C = US, ST = California
-----BEGIN CERTIFICATE-----
MIICTDCCAfMCFFJIN/2YBcc8gVo6ij3OR9sc+UrhMAoGCCqGSM49BAMCMIGoMRMw
EQYDVQQLDApWZXJvU0VMRUNUMTEwLwYDVQQDDChWZXJvU0VMRUNUIERFViBRQSBE
aWdpdGFsIFNpZ25hdHVyZSAyMDMzMSgwJgYDVQQKDB9WZXJvcyBSZWFsIEVzdGF0
ZSBTb2x1dGlvbnMgTExDMRIwEAYDVQQHDAlTYW50YSBBbmExCzAJBgNVBAYTAlVT
MRMwEQYDVQQIDApDYWxpZm9ybmlhMB4XDTIzMTIxMzA0NDUwMloXDTI0MDMxMjA0
NDUwMlowgagxEzARBgNVBAsMClZlcm9TRUxFQ1QxMTAvBgNVBAMMKFZlcm9TRUxF
Q1QgREVWIFFBIERpZ2l0YWwgU2lnbmF0dXJlIDIwMzMxKDAmBgNVBAoMH1Zlcm9z
IFJlYWwgRXN0YXRlIFNvbHV0aW9ucyBMTEMxEjAQBgNVBAcMCVNhbnRhIEFuYTEL
MAkGA1UEBhMCVVMxEzARBgNVBAgMCkNhbGlmb3JuaWEwWTATBgcqhkjOPQIBBggq
hkjOPQMBBwNCAATUyRHZFxJgKAOxeqPAi1UZnej9hLkN+5++YCZXko7FC4AKqDAd
SUlYpLSXXA+vPNALcZ5P/7PNTAcUVgeRDO3cMAoGCCqGSM49BAMCA0cAMEQCIAEZ
n83CQHt+aHopKykf3VU90RBEK/pIk2YnuDzqQT5PAiATHKtCeF4rOSyrgLWh2U7X
kAvYcqIL0M0jXpstx7nZEg==
-----END CERTIFICATE-----
PKCS7 Data
Shrouded Keybag: PBES2, PBKDF2, AES-256-CBC, Iteration 2048, PRF hmacWithSHA256
Bag Attributes
    localKeyID: 53 1F D7 F6 D6 55 2A 20 92 FB 8D 86 22 8F AD 2C 8C 0E 7F 12
    friendlyName: VeroSELECT DEV QA Digital Signature 2033
Key Attributes: <No Attributes>
-----BEGIN ENCRYPTED PRIVATE KEY-----
MIHsMFcGCSqGSIb3DQEFDTBKMCkGCSqGSIb3DQEFDDAcBAgNcNpyceMw9QICCAAw
DAYIKoZIhvcNAgkFADAdBglghkgBZQMEASoEEHT5Aa5x8pcbMxtkbtp6lJIEgZCh
jxsm3vdx/nfH0OKHlbOHw3Z8tVupmec+SPlse1WbSrtCBnF3GB4kCS2zPSlfms7j
j161dqyW4D8OscrLnPRsQ7VVGWAclC+OvNGLnFok6iucRUKYxpH/I5+DFDBiqZ+F
wSmKE74H18FIlYM2DOVe8TeaV66CeD4jQ1Fu97+Xz9MHo/8iy2OlwagRkwaydZs=
-----END ENCRYPTED PRIVATE KEY-----
```

## C# Test Client

The .NET client does **Hashing**, ECDSA **Signing** and ECDSA signature **Verification** (valid and tampered) in `PEM` format.

#### Instructions

1. Create the private and public keys
1. Export both keys from `PEM` to `DER` format.
1. Use the DER keys in the code to Sign and Verify both sample json files.

#### Syntax

```sh
# run
dotnet run

# Program output

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

## Java Signature Validation Client

![Veros Digital Signature](./Figure/verify-java-sample/project.svg)

See the [README](./Figure/verify-java-sample/README.md) document for further reference.

## References

#### Main References:

- [RFC-7468 extual Encodings of PKIX, PKCS, and CMS Structure (see section-13)](https://datatracker.ietf.org/doc/html/rfc7468)
- [RFC-6979 - Deterministic Usage of the DSA and ECDSA](https://datatracker.ietf.org/doc/html/rfc6979)
- [RFC-5915 - Elliptic Curve Private Key Structure](https://datatracker.ietf.org/doc/html/rfc5915)
- [RFC-5480 - Elliptic Curve Cryptography Subject Public Key Information](https://datatracker.ietf.org/doc/html/rfc5480)


#### Additional References

- [LE - A Warm Welcome to ASN.1 and DER](https://letsencrypt.org/docs/a-warm-welcome-to-asn1-and-der/)
- [OpenSSL ecparam](https://www.openssl.org/docs/man1.0.2/man1/ecparam.html)
- [GitHub ecdsa-dotnet](https://github.com/starkbank/ecdsa-dotnet)
- [StackOverflow](https://stackoverflow.com/questions/56144550/loading-an-ecc-private-key-in-net)


#### Microsoft Windows specific references:

- [Openssl PKCS#8 - Windows .NET 4.x](https://www.openssl.org/docs/man3.0/man1/openssl-pkcs8.html)
- [ECDSA Microsoft Docs - ImportEncryptedPkcs8PrivateKey](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.asymmetricalgorithm.importencryptedpkcs8privatekey?view=net-8.0&viewFallbackFrom=netframework-4.7.2#system-security-cryptography-asymmetricalgorithm-importencryptedpkcs8privatekey(system-readonlyspan((system-byte))-system-readonlyspan((system-byte))-system-int32@))
- [ECDsa Class](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.ecdsa?view=netframework-4.7.2)