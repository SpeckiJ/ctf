### Difficulty:
- Easy (Author)
- Baby (Specki)

### Notes:
- Apparently `TrueCryptVolumeE` is not encrypted during runtime and subsequently not inside the memorydump

### Rabbit Holes:
	- `\Device\HarddiskVolume1\Documents and Settings\CSCG\Desktop\CSCG\cscg.flag.PNG` is a false flag

### Solution:
- Analyze memory dump with Volatility Framwork
- `filescan` shows some interesting files
	- `\Device\TrueCryptVolumeE\password.txt`
	- `\Device\TrueCryptVolumeE\flag.zip`
- `dumpfiles` the interesting files reveals:
	- `password.txt` contains plaintext: "BorlandDelphiIsReallyCool"
	- `flag.zip` contains file
- Decrypt file in flag.zip with password from `password.txt`
- WIN

### Flag
`CSCG{c4ch3d_p455w0rd_fr0m_0p3n_tru3_cryp1_c0nt41n3r5}`

### Remediation:
- Do not use WindowsXP in 2020
- Choose encryption methodology in accordance with requirements.
	- If files are not needed all the time do not decrypt them in advance
- Do not store passwords in plaintext next to encrypted data. Even on "encrypted volumes" this is a bad idea, as they are decrypted during their lifetime and therefore provide free access to the data!