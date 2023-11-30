import canonicaljson # pip install canonicaljson

jsonFile = 'jcan.json'

with open(jsonFile,'r') as  data:
    dataRaw = data.read()

testRaw = canonicaljson.encode_canonical_json(dataRaw)
testStrU8 = testRaw.decode('utf-8')
print(testStrU8)

with open('signature.txt', 'wb') as signatureFile:
    signatureFile.write(testRaw)

# with open('signature.txt', 'w') as signatureFile:
#     signatureFile.write(testStrU8)

assert canonicaljson.encode_canonical_json({}) == b'{}'
