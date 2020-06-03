### Difficulty:
- Medium (Author)
- Medium (Specki)

### Notes:
- WinXP Virtual Machine is not useful at all. Volatility is much better

### Rabbit Holes:
- Data has been recovered from `mspaint.exe` temporary buffers in ctfs before [writeup](https://www.rootusers.com/google-ctf-2016-forensic-for1-write-up/)
	- Scanning the memory here does not yield any results, just a bunch of funky art
- 133t5p34k is not strictly defined and not consistent 
	- characters are difficult to distinguish e.g. 1, l, I, l
	- wrong character -> wrong flag

### Solution:
- Analyze memory dump with Volatility Framwork
- `pslist` shows interesting processes:
	- `CSCG_Delphi.exe` --> looks promising
	- `mspaint.exe` --> see Rabbit Holes
- Extract Delphi Exe via `procdump` into `1920.exe`
	- Unfortunately exe will not run in any compatibility mode or on WinXP VM
- Use DeDe decompiler to decompile exe
	- Decompilation shows `TForm1` with title `CrackMe`
	- `CheckFlag` Button can be decompiled into annotated assembly by DeDe
	- `CheckFlag` makes references to various hard-coded all-uppercase strings
	- `CheckFlag` uses `TIdHashMessagDigest5` which implements MD5 "Encryption"
	- `CheckFlag` uses `StrUtils.AnsiReverseString` so comparison string is possibly reversed
- Lookup String constants in online Database [hashes.com](https://hashes.com/en/decrypt/hash)
	- `1efc99b6046a0f2c7e8c7ef9dc416323:dl0`
	- `25db3350b38953836c36dfb359db4e27:kc4rc`
	- `40a00ca65772d7d102bb03c3a83b1f91:!3m`
	- `c129bd7796f23b97df994576448caa23:l00hcs`
	- `017efbc5b1d3fb2d4be8a431fa6d6258:1hp13d`
- Strings form valid words when reversed, but do not form a coherent sentence
	- Assembly comparing the strings differs, possibly not comparing in order
	- Assuming the flag is human-readable there are not many combinations that form a coherent sentence
- Manually reorder strings to form human.readable flag. Add CSCG\{\} around the flag
- WIN

### Flag
`CSCG{0ld_sch00l_d31ph1_cr4ck_m3!}`

### Remediation:
- Do not use MD5 constants as password/key