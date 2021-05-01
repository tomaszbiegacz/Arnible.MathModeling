# Arnible

[![CI-Linux](https://github.com/tomaszbiegacz/Arnible.MathModeling/actions/workflows/ci-linux.yml/badge.svg)](https://github.com/tomaszbiegacz/Arnible.MathModeling/actions/workflows/ci-linux.yml)
[![CI-Windows](https://github.com/tomaszbiegacz/Arnible.MathModeling/actions/workflows/ci-windows.yml/badge.svg)](https://github.com/tomaszbiegacz/Arnible.MathModeling/actions/workflows/ci-windows.yml)
[![CodeQL](https://github.com/tomaszbiegacz/Arnible.MathModeling/actions/workflows/
codeql-analysis.yml/badge.svg)](https://github.com/tomaszbiegacz/Arnible.MathModeling/actions/workflows/codeql-analysis.yml)

.Net is great, but doesn't provide all the utilities that I need for writing code focused on minimal memory footprint from one side, but also on following defensive coding principles.
Arnible toolkit is collection of reusable things that I use in my R&D projects. 
The toolkit is split into libraries:
* [Arnible](./Arnible) contains basic interfaces, utilities used shared in all other libraries like logger interface or ReadOnlyArray value type.
* [Arnible.Assertions](./Arnible.Assertions) is yet another heavily shared library focused on defensive programming support. Unit tests are also based on this.
* [Arnible.Export](./Arnible.Export) is boxing free exporting library focused on minimal memory and processing footprint needed for diagnostics.
* [Arnible.Linq](./Arnible.Linq) support for LINQ with defensive API like "SumDefensive" or "SumWithDefault" together with LINQ for combinatorics and ReadOnlySpan support.
* [Arnible.MathModeling](./Arnible.MathModeling) and [Arnible.MathModeling.Formal](./Arnible.MathModeling.Formal) contains various tools for numeric and symbolic math analysis.
* [Arnible.xunit](./Arnible.xunit) simplifies writing xunit tests for projects using Arnible toolkit.

I haven't reached yet version 1.0, work is still in progress.

## License

Please see [License](./LICENSE).
