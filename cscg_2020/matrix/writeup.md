### Difficulty:

### Notes:
- Neo takes the Red pill in the movie
	- "You take the blue pill – the story ends, you wake up in your bed and believe whatever you want to believe"
	- "You take the red pill – you stay in Wonderland, and I show you how deep the rabbit hole goes."
	
### Rabbit Holes:
- https://github.com/kimci86/bkcrack needs at least 12 Bytes of plaintext to brute-force.
	- We only have "\xcfCSCG{" (MSB from CRC is first byte)
		- Maybe brute-force other characters? Unlikely in a CTF
- Comparison with original sound sample (`reference_quote.wav`) does not work
	- probrably modified? or from another source? online samples are all stereo
	
### Solution:
- Spectogram shows password `Th3-R3D-P1ll?`
- `steghide extract -sf matrix.wav -p Th3-R3D-P1ll? -xf output.jpg` ergibt gültigen JFIF File
- `output.jpg` has trailing PKZIP file (Header 50 4B 03 04)
	- stored in `extracted_from_output.zip`
- `extracted_from_output.zip` has password-encoded file `secret.txt`
- Google Search for "Oktoberfest Nacht" give original Image
	- Comparing original image with `output.jpg` shows differenzes in Lichterkette in bottom of the image
		- Color encodes Bit
		- Manually transposing gives `01101110 00100001 01000011 00110011 01011111 01010000 01010111 00111111` (and its complement)
		- Converting to ASCII gives Password `n!C3_PW?`
- Decrypt `secret.txt` mit Password `n!C3_PW?` into `secret_decrypt.txt`
- `secret_decrypt.txt` contains String `6W6?BHW,#BB/FK[?VN@u2e>m8`
	- May be final flag (first and third letter are equal, fourth and last are unequal), but in non-standard encoding or simple substitution cipher
	- Trying out different simple encodings/ciphers on [cryptii](https://cryptii.com/) shows ASCII85 is used and flag is given in plaintext
- WIN
### Flag
`CSCG{St3g4n0_M4s7eR}`
### Remediation: