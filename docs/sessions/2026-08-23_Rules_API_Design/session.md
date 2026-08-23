# Session: Rules API Design
**Date:** 2026-08-23

## Purpose
Discussing and refining the functional design of the Rules API, focusing on the Developer Experience (DX) for logical mutations and mechanics calculation in the TTRPG context. 

## Decisions Made

1. **Functional-like Environment**
   - **Decision:** Rules accept a single argument (the subject) and an execution context. They return a computed result or a mutated version of the subject.
   - **Rationale:** This enables standard functional patterns like composition (`&`), joining (`+`), and sequential chaining (`|`), mimicking mathematical or logical pipelines.

2. **Error Handling via Railway-Oriented Programming (ROP)**
   - **Decision:** The API utilizes a custom `RuleResult<T>` abstract record rather than raw exceptions for control flow.
   - **Rationale:** A union-type result explicitly handles the expected failures in an RPG (e.g., target immune, missing an attack). The `RuleResult<T>` includes `Success` and `Failure` records.

3. **Exception-Backed Failures**
   - **Decision:** The `Failure` record in `RuleResult<T>` wraps a standard C# `Exception`.
   - **Rationale:** While allocating exceptions has a performance overhead, it integrates flawlessly with the existing C# ecosystem (like `AggregateException`), retains deep stack traces, and removes the need to reinvent custom error mapping trees. The trade-off is considered acceptable for the rich tooling it provides.

4. **Invisible Short-Circuiting & DX**
   - **Decision:** Extension operators (`|`, `&`, `+`) use monadic mappers (`FlatMap`) under the hood to automatically short-circuit rule execution if a failure occurs.
   - **Rationale:** This frees rule authors from manually checking inputs for failures. The API also uses implicit conversion operators (`T -> RuleResult<T>.Success`, `Exception -> RuleResult<T>.Failure`) so that the rules' internal logic stays completely clean from result-wrapping boilerplate.

5. **Execution Abstraction**
   - **Decision:** `IRuleApplier` acts as the execution boundary.
   - **Rationale:** Handles context creation (`IRuleContext` / `IAsyncRuleContext`), scoping, and DI containers, keeping the pure rules isolated from lifecycle management.

## Next Steps
- Finalize the monadic implementations (`FlatMap`, `Map`) in the `RuleExtensions` static class for operators like `Compose` (`&`) and `Join` (`+`), ensuring all extensions compile against the new `RuleResult<T>`.
