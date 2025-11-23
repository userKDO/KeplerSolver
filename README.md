# KeplerSolver

C# library for satellite orbit calculations using Keplerian mechanics.

## Features
- Orbital period & velocity calculations
- Multiple celestial bodies (Earth, Mars, Moon)
- Extensible architecture
- Clean API

## Documentation

**Quick reference:** [Documentation.pdf](Documentation.pdf) - API overview

**Full documentation:** To generate complete HTML documentation with search and examples:

```bash
# Install DocFX
dotnet tool install -g docfx

# Install Node.js
Download from: https://nodejs.org/
# Clone and build the project
git clone https://github.com/yourusername/KeplerSolver.git
cd KeplerSolver
dotnet build

# Generate and serve documentation
mkdir docs
cd docs
docfx docfx.json --serve

# Opens http://localhost:8080 with full documentatio
