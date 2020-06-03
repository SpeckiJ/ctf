### Difficulty:
- Easy (Author)
- Baby (Specki)

### Notes:
- Many anti-debugging checks. Probably hard to circumvent
- Ghidra does not like DotNet Binaries by default. Disassembly is practically unusable

### Rabbit Holes:

### Solution:
- Decompile in dnSpy
- Find `ìnitialCheck` function that validates args
- Find hardcoded check comparing input to decryption of encoded constant
- Copy&Paste decription Routine into seperate program(solve.cs), decrypt encoded constant and print to console
- Run reme.dll with password from previous step (`CanIHazFlag?`)
- WIN

### Flag
`CSCG{CanIHazFlag?}`

### Remediation:
- Do not use two-way encryption for securing secrets --> use one-way and only store hash
- Do not hardcode passwords inside decryption function