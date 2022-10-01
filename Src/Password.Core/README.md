<div align="center">
	<h1>Password Core library</h1>
	<p>
		<b>A simple and easy to use library to generate passwords.</b>
	</p>
	<br>
</div>

## Installation

.Net CLI
```bash
dotnet add package Password.Core
```

## Usage

The library provides a `PasswordGeneratorFactory` that you can use to create the instance of PasswordGenerator.
```csharp
var passwordGenerator = PasswordGeneratorFactory.GetGenerator();
```

once you have a generator you can use it to generate a password.

```csharp
string password = passwordGenerator.Generate();
```

> Note: By default the `PasswordGenerator` generates the password of length 15 consists of lower case, upper case, digits and symbols.

There is another overload of the `Generate(PasswordOptions options)` method which takes `PasswordOptions` as an input so you can customize password length and what type of characters your password should contain.

```csharp
string password = passwordGenerator.Generate(new PasswordOptions{
    Length = 20,
    IncludeLowerCaseLetters = true
    IncludeUpperCaseLetters = true
    IncludeDigits = false
    IncludeSymbols = true
});
```

## Resources used
The icon this package is using is provided by flaticon and can be access at below link.

<a href="https://www.flaticon.com/free-icons/security" title="security icons">Security icons created by Freepik - Flaticon</a>
