import json
import base64
import hashlib
from ecdsa import SigningKey,VerifyingKey, NIST256p

with open('sample.json','rb') as  data:
    dataRaw = data.read()

with open('prv.pem') as priKey:
    sk = SigningKey.from_pem(priKey.read(),hashlib.sha256 )

# sk = SigningKey.generate(curve=NIST256p)
vk = sk.verifying_key
signatureRaw = sk.sign(dataRaw)
# signatureStr = signatureRaw.to_string()
res= vk.verify(signatureRaw, dataRaw)

with open('signature.txt', 'wb') as signatureFile:
    f.write()


# f = open('sample.json')
# dataStr = json.load(f)
# # data = open('sample.json','rb')
# dataRaw = base64.b64decode(f)

with open('pub8.pem') as pubKey:
    vk = VerifyingKey.from_pem(pubKey.read())

# signatureStr = b'MEQCIFd/8JUDaFRnNZVAPSImQLFde5sqbfmrPcH59D5JrnutAiA/9XFbKsjkkKs/OqfI3KFbSI2EGpgchKEyk7eu49+eSQ=='
signatureStr = "MEUCIGibIR×PTEIipIN+H5cfVkx2HYi5A4yY+dSApGA8wapvAiEAifBeNrRW7I2Bw6sX1cQ535fCRZQHJAzFUGUsi/Ep9Nk="
signatureRaw = base64.b64decode(signatureStr)

res = vk.verify(signatureRaw,dataRaw)
# res = vk.verify(signatureStr,dataRaw)

print(res)