<div align="center">
	<h1>Password CLI</h1>
	<p>
		<b>A simple and easy to use CLI to generate passwords.</b>
	</p>
	<br>
</div>

## Installation

.Net CLI
```bash
dotnet tool install -g Password.Tool
```

## Usage

The CLI provides a command to generate a password
```bash
password generate
```

> Note: By default the CLI generates the password of length 15 consists of lower case, upper case, digits and symbols.

You can pass additional options to customize password length and what type of characters your password should contain.

```csharp
password generate -l 20
```

You can get the help about the options supported by the `generate` command
```csharp
password generate -h
```

## Resources used
The icon this package is using is provided by flaticon and can be access at below link.

<a href="https://www.flaticon.com/free-icons/security" title="security icons">Security icons created by Freepik - Flaticon</a>
