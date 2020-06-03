import OpenSSL.crypto as crypto
import math
from Crypto.Util.number import long_to_bytes
from datetime import datetime

def main():
	key = crypto.load_publickey(crypto.FILETYPE_PEM, open('pubkey.pem','r').read())
	pubkey = key.to_cryptography_key().public_numbers()

	message = open('message.txt', 'r').read()

	for i in range(622750, 10000000):
		print(i)
		if pubkey.n % i == 0:
			p = i
			q = pubkey.n // i
			phi = (q-1)*(p-1)
			d = getModInverse(pubkey.e, phi)

			decrypted = pow(int(message), d, pubkey.n)
			print('found p:' + str(p))
			print('found q:' + str(q))
			print('phi is:' + str(phi))
			print('d is:' + str(d))
			print('decrypted is' + str(decrypted))

			print(long_to_bytes(decrypted))
			break

def egcd(a, b):
	x,y, u,v = 0,1, 1,0
	while a != 0:
		q, r = b//a, b%a
	m, n = x-u*q, y-v*q
	b,a, x,y, u,v = a,r, u,v, m,n
	gcd = b
	return gcd, x, y

def getModInverse(a, m):
	if math.gcd(a, m) != 1:
		return None
	u1, u2, u3 = 1, 0, a
	v1, v2, v3 = 0, 1, m
	while v3 != 0:
		print(datetime.now())
		q = u3 // v3
		v1, v2, v3, u1, u2, u3 = (u1 - q * v1), (u2 - q * v2), (u3 - q * v3), v1, v2, v3
	return u1 % m

if __name__ == "__main__":
    main()
