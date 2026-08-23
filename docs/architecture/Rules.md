# Rules API

**Rules** in the RPG-Companion define the specific mechanics of a given TTRPG setting. They provide a functional-like environment for defining logical mutations and calculating results cleanly.

## Overview

All rules accept a single argument (the *subject*). This single-argument processing model makes it easy to employ functional programming patterns such as composition and joining.

If a rule requires non-pure dependencies, an `IRuleContext` (or `IAsyncRuleContext`) is provided alongside the subject. This context exposes a scoped DI registry, and in asynchronous scenarios, provides a `CancellationToken` or the ability to halt execution.

## Railway-Oriented Programming (ROP) & RuleResult

The Rules API avoids using exceptions for control flow in domain mechanics. Instead, it leverages a Railway-Oriented Programming (ROP) pattern via the `RuleResult<T>` abstract record. 

- **Success / Failure**: A rule returns either a `Success` (holding the computed value or mutated subject) or a `Failure` (wrapping an `Exception`).
- **Implicit Conversions**: For a seamless Developer Experience (DX), `RuleResult<T>` has implicit operators for both `T` and `Exception`. Inside a rule, a developer can simply `return subject;` or `return new InvalidOperationException("...");` without boilerplate wrappers.
- **Why Exceptions for Failures?**: Although instantiating `Exception` instances has a performance overhead, doing so ensures native integration with the C# ecosystem (including `AggregateException`), retains detailed stack trees, and allows robust pattern matching at the callsite.

## Composition, DX, and Short-Circuiting

The API leverages extension operators to provide an incredibly fluent DSL for chaining rules:

- `|` (**Then**): Chains rules sequentially, executing the left side and feeding its result into the right side.
- `&` (**Compose**): Function composition (evaluates the right side first, then feeds into the left side).
- `+` (**Join**): Bridges a rule that outputs an intermediary type (`TInner`) with a rule that accepts `TInner` and returns the original subject type.

**Invisible Short-Circuiting**: Because rules return `RuleResult<T>`, the composition operators utilize monadic binds (`FlatMap`). If a rule in the chain fails, the `FlatMap` immediately propagates the `Failure` down the pipeline. Subsequent rules are skipped, and the author of the next rule never has to manually check for invalid incoming state.

```csharp
// Example demonstrating the fluent rule chaining:
IRule<int> action = attack & checkAttack | getMonsterName + calculateVulnerabilities | calculateFinalDamage;
```

## Internal Architecture

The `RpgCompanion.Core/_Components/_Rules` folder contains:
- **Interfaces (`IRule<...>`, `IAsyncRule<...>`)**: The core abstractions for better reflection and DI support.
- **Struct Wrappers**: Lightweight wrappers (e.g., `Rule<T>`) that encapsulate `Func` delegates into the interface abstraction.
- **`Rule` Static Class**: The primary entry point containing factory methods (`Rule.Create()`) and the extension methods/operators for composing rules.
- **`RuleResult<T>`**: The union-type abstract record and its accompanying monadic extension mappers (`Map`, `FlatMap`).
- **`IRuleApplier`**: A service used to orchestrate the execution of a rule, generating the execution context (`IRuleContext`) and handling execution boundaries.

## Asynchronous Rules and "Function Coloring"

The framework provides an asynchronous variety of rules (`IAsyncRule<...>`). Due to C#'s "function coloring," composing or joining a synchronous rule with an asynchronous one will force the entire resulting composition to evaluate asynchronously.
