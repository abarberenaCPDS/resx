# Public Key Verification using Java

![project.svg](project.svg)

## Docker Setup

- [Alpine Java image](https://blog.developer.atlassian.com/minimal-java-docker-containers/)


```sh
# Image build:
docker build -t abes-alpine-java .
```

```sh
# Container execution
docker container run --rm -it --name abes-alpine-java -v $(pwd):/root abes-alpine-java sh
```

## Runnnig the Java client

```sh
# change to source directory 
cd ~

# set signature
sig='MEYCIQD5vBVFMUwXKdpeXaEGcCBrYYooT4P/Rmoaz7nKl+2ovgIhAK4/kFWfc1U0dkTzD4
IQxbSqs72s3Rufr+6fmctxfkRR'

# run the program
javac verify.java && java verify sample.json $sig
```

#### Output Example:

```sh
# output
=== Running ===
--> Input       sample.json
--> Signature   MEYCIQD5vBVFMUwXKdpeXaEGcCBrYYooT4P/Rmoaz7nKl+2ovgIhAK4/kFWfc1U0dkTzD4IQxbSqs72s3Rufr+6fmctxfkRR
--> Public Key  MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAE0oHiOSsT2BLTHzEolqkc565lVJVac5x/MdM7raVL4J9Pmf2XEQFn5qRTqLpt32I8mpBMHXNC/Q4xlDJ32UqOkw==
--> PubKey File pub8.pem
--> Ecdsa ready Signature object: SHA256withECDSA<initialized for verifying>
--> Ecdsa updated...
--> Ecdsa verified...true
```


## Instructions to Verify from Figure

This program can be used to verify digital signatures from production.  The program takes two arguments:

- An input file with the JSON response to validate
- Ahe `x-clearcapital-signature` response header value

The program will then read the file and the production public key (the `public.pem` file in this directory) and validate the signature value. A response of `true` means the signature is valid.

Two same production input files are also included here; you can use them to validate the signatures below:

```bash
$ java verify input_clearprop.json

MEUCIFCHc7ipel34M2bVxt/OhHTrPPvl1sYKrBc0ylwpV2NAAiEA0caNs9DjhgO7sI4/6NmxD4n39t8zgfElQ8LM1ciF+pY=

$ java verify input_avm.json

MEQCIE1zYJVW6PgCEyE6tlUtoH9k7BvsSIch/dwyS4HUhkVLAiBhK/Be0O2Rs1hdwylOWqv0I50sZ0l1O66pWqfHtR8aKw==
```

## ECDSA References

- [ECDSA using java.security.Signature](http://fog.misty.com/perry/ccs/EC/all-EC.html)
- [How to Read PEM File to Get Public and Private Keys](https://www.baeldung.com/java-read-pem-file-keys)
- [Encode Base64 cannot find symbol error](https://stackoverflow.com/questions/39711122/encode-base64-cannot-find-symbol-error)

## Java References

- [Input and Convert the Encoded Public Key Bytes](https://docs.oracle.com/javase/tutorial/security/apisign/vstep2.html)
- [Class X509EncodedKeySpec](https://docs.oracle.com/javase/8/docs/api/java/security/spec/X509EncodedKeySpec.html)
- [Verifying a Digital Signature](https://docs.oracle.com/javase/tutorial/security/apisign/versig.html)
- [Class Signature](https://docs.oracle.com/javase/8/docs/api/java/security/Signature.html#verify-byte:A-)
- [Interface ECPublicKey](https://docs.oracle.com/javase/7/docs/api/java/security/interfaces/ECPublicKey.html)

## Bouncy Castle References

- [How to Read PEM File to Get Public and Private Keys - Good Read](https://www.baeldung.com/java-read-pem-file-keys)

## 



### ECDSA public and private

Here's an example of self-issued keys:

#### Private Key

```sh
Private-Key: (256 bit)
priv:
    f0:69:5b:54:ed:b6:8f:67:f5:fe:88:72:ec:23:0e:
    7b:46:f5:59:85:8b:dd:08:61:ef:14:3d:86:07:27:
    3b:1f
pub:
    04:bd:c1:8f:b6:ed:84:9c:f3:16:83:0a:d0:43:21:
    56:96:38:a4:a6:c3:8c:70:6d:fa:b2:80:f6:90:48:
    d2:bb:73:2a:68:ce:dd:e0:9f:77:f5:6a:0b:74:41:
    dd:05:91:f1:30:9b:ba:61:e6:97:d2:30:f5:0d:a2:
    ac:fe:d1:38:18
ASN1 OID: prime256v1
NIST CURVE: P-256
-----BEGIN EC PRIVATE KEY-----
MHcCAQEEIPBpW1Ttto9n9f6IcuwjDntG9VmFi90IYe8UPYYHJzsfoAoGCCqGSM49
AwEHoUQDQgAEvcGPtu2EnPMWgwrQQyFWljikpsOMcG36soD2kEjSu3MqaM7d4J93
9WoLdEHdBZHxMJu6YeaX0jD1DaKs/tE4GA==
-----END EC PRIVATE KEY-----

```

#### Public Key

```sh
-----BEGIN PUBLIC KEY-----
MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEvcGPtu2EnPMWgwrQQyFWljikpsOM
cG36soD2kEjSu3MqaM7d4J939WoLdEHdBZHxMJu6YeaX0jD1DaKs/tE4GA==
-----END PUBLIC KEY-----
```


