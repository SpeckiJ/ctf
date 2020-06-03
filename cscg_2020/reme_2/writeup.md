### Difficulty:
- Medium (Author)
- Medium (Specki)


### Notes:
- Many anti-debugging checks. Probably hard to circumvent
- Ghidra does not like DotNet Binaries by default. Disassembly is practically unusable

### Rabbit Holes:
- `.GetILAsByteArray()` returns different values when copy&pasted into seperate program
	- Dependent on internal Method Name Tables? --> Those change based on what the compiler likes most during compiling
	- Trying to recreate the exact assembly feels impossible. Even when copy&pasting the exact codelines, assembly still reorders stuff (Several hours used on this)
	- When reordering functions in Code and compiling values from `GetILAsByteArray` differ only slightly (+- 5) and differ only on fixed positions (7 total)
		- Bruteforcing the -15 to +15 range of possible values on all 7 locations does not yield correct key and takes a loong time (w/o optimization)
- Loading the dll into a seperate program is not trivial and requires more C# knowledge
- Debugging dynamically loaded functions is apparently not possible? Breakpoints are skipped

### Solution:
- Decompile in dnSpy
- Find `initialCheck` function that validates args (used for reme_1). Ignore for now
- Observations:
	- ilAsByteArray created based on Method body from `initialCheck` using `GetILAsByteArray` and is used as AES decryption key later on
		- `GetILAsByteArray` returns exact raw assembly bytes from the function, result can be extracted using ByteViewer (e.g. inside Ghidra)
	- Memory is searched for hardcoded String `THIS_IS_CSCG_NOT_A_MALWARE`. All Memory after this string is then decrypted
	- Decrypted Data is loaded dynamically and "Check" is called
- In Hex Editor String `THIS_IS_CSCG_NOT_A_MALWARE` can be found near the end of the assembly. Entropy graph suggests all data after this string is encrypted
	- Data can simply be copy-pasted into seperate file `encoded_function` to be easily accessible
- `AES_Decrypt` can be copy&pasted into seperate program and run on extracted key (bytes from `initialCheck` Function) and `encoded_function`.
- In original program flow this assembly was dynamically loaded and `Check` function called on it
	- Debugging of this dynamically loaded function seems difficult - Breakpoints are ignored
	- Observation: `decoded_flag_check` starts with bytes `4D 5A 90 00` and includes String "DOS" --> DOS MZ executable format
- Save decrypted data into new file tmp.exe
	- can be run and provides 
- Decompile tmp.exe via dnSpy. Copy Disassembled Function `Check` into `decrypted_check.txt`
- Analyze `Check` Function. It compares input against hardcoded constants
	- md5 has can easily be looked up in online md5 databases
	- other constants are human-readable and can be extracted by hand
- WIN


### Flag:

`CSCG{n0w_u_know_st4t1c_and_dynamic_dotNet_R3333}`

### Remediation:
- Do not use program text as encryption key
- Security through obscurity (aka obfuscation) is generally a bad idea