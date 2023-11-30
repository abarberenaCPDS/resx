import jcs
import json

f = open('sample.json')
data = json.load(f)
print('json ===')
print(data)
print('=== json')
f.close()

print('=== canonicalized ===')
canon = jcs.canonicalize(data)
print(canon)
print('=== canonicalized ===')


