# Contributing to CPAP-Exporter

First off, thank you for considering contributing to CPAP-Exporter! We're excited to collaborate with you. Below are some guidelines to ensure a smooth contribution process.

## How Can You Contribute?

1. **Report Issues**  
   Found a bug or have a suggestion? Create an issue [here](https://github.com/CascadePass/CPAP-Exporter/issues). Please be as detailed as possible.

2. **Submit Pull Requests**  
   - Fork the repository and create your branch from `main`.
   - Commit your changes with clear messages.
   - Open a pull request and describe what you've done.

3. **Code Style Guidelines**  
   Follow these conventions:
   - [Code formatting rules specific to your project]
   - Add comments where necessary for clarity.

4. **Testing**  
   If your changes involve code, ensure all tests pass. Add tests if you're introducing new functionality.

5. **License**
   All contributions must be licensed under MIT to be used.

## Testing Policy

CPAP-Exporter has a lot of automated tests.  They will all run on the server and only allow pull requests to be merged when all tests pass.  Hopefully this means you can't break anything.

The testing workflow is a little unusual and might surprise you, so here are some details to be aware of.
- *Dependency Injection* is used for testing, this is widely considered a best practice.  However,
- Dependency injection is disabled in release builds, for security reasons.  This prevents the application from loading plugin-like objects that can change its behavior at runtime, including stealing medical data.  That scenario isn't possible, but this means the UI unit tests only build in debug configuration.  Release builds are tested for integration of the code base, against actual machine data.

## Ground Rules

- Write clear, concise commit messages.
- Adhere to the project's license and policies.
- Small pull requests are preferred, please!
- Be respectful and inclusive.

## Need Help?

Feel free to reach out, talk spaces will be established soon.

Thanks for helping make CPAP-Exporter awesome!

