# KeplerSolver

C# library for satellite orbit calculations using Keplerian mechanics.

## Features
- Orbital period & velocity calculations
- Multiple celestial bodies (Earth, Mars, Moon)
- Extensible architecture
- Clean API

## Documentation

**Quick reference:** [KeplerSolverDocs.pdf](KeplerSolverDocs.pdf) - API overview

**Full documentation:** To generate complete HTML documentation with search and examples:

```bash
# Install DocFX
dotnet tool install -g docfx

# Generate and serve documentation
cd docs
docfx docfx.json --serve
# Opens http://localhost:8080 with full documentatio
