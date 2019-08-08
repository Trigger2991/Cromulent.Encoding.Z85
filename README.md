# Z85

This is a C# implementation of the Z85 specification found at: https://rfc.zeromq.org/spec:32/Z85/

#### Why use Z85?
Z85 is a derivative of existing Ascii85 encoding mechanisms, modified for better usability, particularly for use in source code.

The Z85 encoded string is string-safe, so can be used cleanly in source code, XML, JSON, and so on.
This is achieved because the character set does not include the single quote, double quote, or backslash characters.

#### The benefits of Base85 over Base64

Base85 uses 5 ASCII characters to represent 4 bytes of binary data, whereas
Base64 uses 4 ASCII characters to represent 3 bytes of binary data.

This makes Base85 only 25% larger than the original data as compared to Base64 being 33% larger. 

## Getting Started
### Installation via NuGet

Using NuGet search for `Cromulent.Encoding.Z85` or run the following command in the Package Manager Console:

```
PM> Install-Package Cromulent.Encoding.Z85
```

## Usage

By default the `ToZ85String()` method matches the formal Z85 specification and the input binary frame must be divisible by 4, otherwise it will throw an **ArgumentException**. This ensures that it is interoperable with other implementations of the formal Z85 specification.

`ToZ85String()` takes an optional parameter `autoPad` which when set to `true` will pad the input bytes if required and append a padding character to the end of the output string. Use of autoPad is not interoperable with other implementations of Z85.

#### Encoding (without padding)
```C#
var bytes = new byte[] { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7, 0x5B };
var output = Z85.ToZ85String(bytes);
// output == "HelloWorld"
```

#### Decoding
```C#
var encodedString = "HelloWorld";
var output = Z85.FromZ85String(encodedString);
// output == [0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7, 0x5B]
```

#### Encoding (with padding)
```C#
var input = "HelloWorld"; // 10 bytes - NOT divisible by 4 with no remainder
var inputBytes = System.Text.Encoding.Default.GetBytes(input);
var output = Z85.ToZ85String(inputBytes, autoPad: true);
// output == "nm=QNz=Z<$y?9IC2"
```


## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
