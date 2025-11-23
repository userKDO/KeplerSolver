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

# Install Node.js (required for full documentation)
Download from: https://nodejs.org/

# Clone and build the project
git clone https://github.com/yourusername/KeplerSolver.git
cd KeplerSolver
dotnet build

# Generate and serve documentation
mkdir docs
cd docs
docfx init

# When prompted:
# - Name: KeplerSolverDocs
# - Generate .NET API: y
# - .NET projects location: ../KeplerSolver.csproj
# - Use default values for other options

# Generate and serve documentation
docfx docfx.json --serve

# Opens http://localhost:8080 with full documentation
```
## Quickstart
```csharp
// Create a satellite
var satellite = new Satellite 
{
    Name = "ISS",
    Altitude = 400,
    Inclination = 51.6
};

// Calculate orbital period
var period = OrbitalCalculator.OrbitalPeriodviaHeight(satellite, Planet.Earth);
```
