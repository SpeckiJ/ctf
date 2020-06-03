### Difficulty:
- Baby (Author)
- Baby (Specki)

### Notes:


### Rabbit Holes:


### Solution:
- Open rev2 in Ghidra
- Find `main` function
- See `strcmp` of Input variable with hardcoded bytes
- Input Variable is modified before comparison by while-loop
	```
	while (i < (int)sVar2 + -1) {
		input[i] = input[i] ^ (char)i + 10U;
		input[i] = input[i] - 2;
		i = i + 1;
	}
	iVar1 = strcmp((char *)input,"lp`7a<qLw\x1ekHopt(f-f*,o}V\x0f\x15J");
	```
- While loop modifies each character by subtracting XOR and subtraction
- Obfuscation can be easily reversed by applying the reverse instructions in reverse order:
	```
	void rev3()
	{
		char solution[50] = "lp`7a<qLw\x1ekHopt(f-f*,o}V\x0f\x15J";

		for (auto i = 0; i < 50; i++)
		{
			solution[i] += 2; // Revert subtraction
			solution[i] = solution[i] ^ (char)i + 10U; // Revert XOR
			std::cout << solution[i]; // Print password
		}
		std::cout << std::endl;
	}
	```
- Reversing obfuscation yiels password: `dyn4m1c_k3y_gen3r4t10n_y34h`
- `netcat` to the Server and enter password
- WIN

### Flag
`CSCG{pass_1_g3ts_a_x0r_p4ss_2_g3ts_a_x0r_EVERYBODY_GETS_A_X0R}`

### Remediation:
- Do not hardcode passwords unencrypted