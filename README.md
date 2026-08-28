# .NET / C# Fundamentals — Engineering Knowledge Base

A curated, production-shaped demonstration of the .NET fundamentals an L6 backend
engineer is expected to hold at **design recommendation, not memory** level:
the code compiles clean, ships with a green xUnit suite, and every sample is
justified by an explicit design decision in this document.

This repository is intentionally small. Depth over breadth: each concept is shown
once, done the way you would defend it in review — validated inputs, no hidden
I/O, no deprecated APIs, no hardcoded environment paths, tests as specification.

---

## Engineering context

The code here is the curated subset of a larger self-learning repository. The
tutorial scratchpad (a ~2,000-line monolith mirroring a public C# course),
hardcoded Windows paths, deprecated `ISerializable`/stream code, and duplicated
markdown notes were removed. What remains is code worth re-hosting: each file
independently demonstrates a fundamental, and each is unit-tested.

The bar for "fundamentals" here is not *"an L6 remembers the syntax."* It is:

1. **Correct by construction** — nullable analysis on, validation is fail-fast,
   state mutation is encapsulated.
2. **Testable by design** — domain code performs no I/O; randomness, time, and
   the console are behind seams.
3. **Modern idioms** — records, `INumber<T>`, `HashCode.Combine`, `System.Text.Json`,
   TAP with `CancellationToken`.
4. **Tradeoffs owned** — every sample has an explicit "why not the alternative".

---

## Quick start

```bash
# Build everything (solution-wide)
dotnet build KnowledgeBase.sln

# Run the ordered walkthrough (each demo maps to a README section)
dotnet run --project src/KnowledgeBase.Runner

# Run the test suite (79 tests)
dotnet test tests/KnowledgeBase.Samples.Tests

# With code coverage
dotnet test tests/KnowledgeBase.Samples.Tests --collect:"XPlat Code Coverage"
```

Requires the .NET 8 SDK (see `global.json`). No environment-specific paths,
settings, or credentials are needed; everything runs on macOS, Linux, or Windows.

---

## Solution layout

```
KnowledgeBase.sln
├── src/
│   ├── KnowledgeBase.Samples/      # The fundamentals, by topic
│   │   ├── Oop/                    # encapsulation, inheritance, composition, IEnumerable + indexer
│   │   ├── Contracts/              # interfaces, Command pattern, factory
│   │   ├── Polymorphism/           # abstract Shape family
│   │   ├── Operators/              # operator overloading + equality contract
│   │   ├── Generics/               # INumber<T> static-abstract math
│   │   ├── Delegates/              # events / multicast subscription
│   │   ├── Reflection/             # attributes + reflection access control
│   │   ├── Simulation/             # inheritance + strategy via injected randomness
│   │   ├── Concurrency/            # TAP, cancellation, bounded fan-out
│   │   └── Serialization/          # System.Text.Json records
│   └── KnowledgeBase.Runner/       # thin console walkthrough, zero logic
└── tests/
    └── KnowledgeBase.Samples.Tests/# 79 tests; the code below is the spec
```

---

## 1. OOP — `Oop/`

Demonstrates the four pillars plus composition and static state:

- **Encapsulation** (`Animal`): private state behind validated properties.
- **Inheritance + polymorphism** (`Dog`): overrides `MakeSound`, `Sound` is
  `virtual` with a protected setter.
- **Composition** (`AnimalIDInfo`): an `Animal` *has-a* registration record.
  `Animal` no longer derives from its registration — inheritance would model
  "is-a", which is wrong here.
- **`IEnumerable<T>` + indexer** (`AnimalFarm`): a foreach-able collection that
  owns its invariants.

**Decisions and why**

| Decision | Rationale |
|---|---|
| Validation throws (`ArgumentException`) instead of coercing bad input to a fallback name | Silent coercion hides bugs at write-time. Defaulting "Whiskers4" to "No Name" lets a bad value flow into production data. Failing fast surfaces the defect where it is made. |
| Property setters do no `Console.WriteLine` | Side effects in setters are unobservable, untestable, and surprising. The original sample printed from them. |
| Shared `static readonly Random`; `Interlocked`/`Volatile` around the instance counter | A `Random` per instance seeds identically for instances created in the same tick; a non-atomic counter is wrong under concurrency. |
| `AnimalIDInfo` is an immutable `record` | Value semantics (equality by data) come free; the *owner* reference is mutated with `with` expressions. |
| Farm indexer only allows contiguous appends; `ArgumentOutOfRangeException` otherwise | The original "filled holes with `null`" — iteration then NRE'd on the filler. An indexer is an implementation detail exposed safely: cap, check, throw. |

---

## 2. Contracts & patterns — `Contracts/`

- **Interfaces as contracts** (`IDrivable`, `IElectronicDevice`): capabilities,
  not identities. Implementers may be substituted behind the abstraction.
- **Command pattern** (`ICommand`, `PowerButton`, `Television`): the *invoker*
  depends only on the command abstraction; the *receiver* is unknown to it.
  This is what makes actions undoable and composable.
- **Factory** (`TvRemote`): clients never construct concrete devices.

**Decisions**: `Television` and `Vehicle` expose observable state (`IsOn`,
`Volume`, `Speed`) and mutate it only through behaviour; neither performs I/O.
That separation is what lets `ContractsTests` assert behaviour with no console
capture. The original counted on printing to demonstrate state — printing is a
presentation concern, not a domain one.

---

## 3. Polymorphism — `Polymorphism/`

`abstract class Shape` forces every derived type to supply `Area()` while
inheriting `Describe()`. Dispatch happens at **runtime, by type**, not by a
string argument.

**Anti-pattern (removed):** the original repo shipped `ShapeMath.GetArea(shape,
...)`, a static method that switched on a `string` shape name. That inverses
the polymorphism it sits beside: adding a shape meant editing a shared switch
instead of adding a class, and the string parameter could carry a typo the
compiler could not see. It was deleted, not kept, and this README preserves the
reason — that decision is the point.

---

## 4. Operators — `Operators/Box`

Operator overloading is rare and usually a sign a `record` would do. When it is
warranted, the correct pattern is:

- Overload the paired operators (`==`/`!=`, `+`/`-`).
- Back every operator with an overridden **`Equals` + `GetHashCode` pair that
  mutually agree** — violating this breaks dictionaries, hash sets, and LINQ.
- `GetHashCode` uses `HashCode.Combine`, not XOR. XOR produces symmetric
  collisions for `(a,b) === (b,a)`-shaped data with poor distribution; the BCL
  hash-combiner mixes bytes and spreads values.

Implicit/explicit conversions (`int ↔ Box`) demonstrate conversion operators,
which are also generally better expressed as `From`/`To` methods — a judgment
call documented in the XML comments.

---

## 5. Generics — `Generics/Numeric`

`INumber<T>` (the static abstract interface in `System.Numerics`, .NET 7+) lets
generic code require "this type has `+`" at **compile time**, checked:

```csharp
public static T Add<T>(T left, T right) where T : INumber<T> => left + right;
```

The removed original `GetSum<T>` accepted `ref T` and routed everything through
`Convert.ToDouble`, which boxes and throws at runtime for types that are not
numeric. `T.Zero`/`+` give zero-cost uniformity with no boxing and no
`Convert` — the constraint is the contract.

---

## 6. Delegates & events — `Delegates/ChannelBus`

Events are **multicast delegates** with add/remove semantics. The publisher
raises via `Published?.Invoke(...)`, opt-in subscriber delivery, safe with zero
subscribers. Covers: anonymous methods, lambda subscription, unsubscribe, and
`EventHandler<TEventArgs>` conventions. The contrast — *callback (delegate) vs
contract (interface) vs notification (event)* — is the practical decision point
when wiring components in a service.

---

## 7. Reflection & attributes — `Reflection/`

`AccessControlService` demonstrates the mechanism behind authorization:

- Metadata declaration: `[Role("admin")]` on a class, `[Authorize("admin")]`
  on a method, each restricted by `AttributeUsage` so it cannot be misapplied.
- Discovery and invocation by reflection.
- Awaited **Task flattening**: `Task<string>` and plain-returning methods are
  invoked through one surface, the result of an async method correctly awaited.
- **MethodInfo caching** in a `ConcurrentDictionary` — reflection discovery is
  expensive and repeated per (type, method). The dictionary gives lock-free reads.
- **Exception unwrapping**: the framework wraps thrown exceptions in
  `TargetInvocationException`; `ExceptionDispatchInfo.Capture(...).Throw()`
  rethrows the caller's original exception with its original stack trace rather
  than leaking a wrapper.

**Production note (the important line):** in ASP.NET Core you do **not** build
this. Policies + `IAuthorizationHandler` are the framework's version of the same
idea, battle-tested and optimized. The value of this sample is that it de-
mystifies the mechanism so the framework's design is *usable, not magic*.

---

## 8. Simulation — `Simulation/`

`Warrior` — `MagicWarrior` inheritance plus a strategy interface (`ITeleport`)
composed in, instead of teleporting baked into the subclass.

**The design judgment that carries weight:** randomness is injected through
`IRandomGenerator`. `Warrior.Attack()` depends on the abstraction, so the whole
battle engine runs **deterministically under test** with a scripted generator
(`SimulationTests`). This is the same reason production systems inject time,
clocks, and randomness — the seam is what makes behaviour verifiable. The
original held `new Random()` inside the class, which made it impossible to
assert anything except "some output happened."

---

## 9. Concurrency — `Concurrency/WorkloadRunner`

TAP fundamentals done without a single blocked thread:

- **Fan-out** with `Task.WhenAll`, cancellation flowing through a
  `CancellationToken` into `Task.Delay`.
- **Bounded fan-out** with `SemaphoreSlim`: unbounded `Task.WhenAll` over a
  large input set spawns one task per item, which is fine for CPU-free I/O but
  fatal for upstream rate limits or dozens of connections. `RunBoundedAsync`
  caps concurrency; `ConcurrencyTests` proves peak concurrency stays at/below
  the bound.
- **`ConfigureAwait(false)`** on every library-side await: no captured
  `SynchronizationContext`, no deadlock risk, no context marshaling cost.

**Tradeoff owned:** `Task.WhenAll` surfaces the *first* fault and aggregates the
rest via `AggregateException` semantics on `Exception`; callers who need every
failure at once must enumerate — that behaviour is a deliberate contract, not a
surprise.

---

## 10. Serialization — `Serialization/`

Records + `System.Text.Json`: an immutable `AnimalSnapshot` round-trips through
indented camelCase JSON, verified by `SerializationTests`.

**What was removed and why:** the original used `ISerializable` +
`XmlSerializer` and a demo of `BinaryFormatter`, which the docs deprecated for
**unsafe type deserialization** (a real-world attack surface — arbitrary types
instantiated from attacker-controlled streams). `ISerializable` has no place in
new code; `System.Text.Json` is the default for schemas you control, and if you
need manual control you implement `JsonConverter<T>`, not a serialization
constructor.

---

## Cross-cutting engineering standards

These apply to every file:

1. **Nullable reference types enabled; code is warning-free.** Not one
   `!` suppression trick, and `TreatWarningsAsErrors` could be turned on.
2. **Domain objects perform no I/O.** Console, files, and time live in the
   runner or behind seams — which is why 79 tests run in ~90 ms with zero setup.
3. **Input validation is fail-fast and at the boundary** (constructor or
   setter), never silent coercion.
4. **No environment coupling.** The deleted `C:\Users\...`-style paths, a
   locally-mutating working directory, and any "got it working on my machine"
   residues are the kind of thing that fails a senior review; none remain.
5. **Composition over inheritance, exceptions confirm the rule.**
6. **Tests are the specification.** They assert behaviour (`PowerButton`
   toggles a device), not implementation (the invoker calls `On`).

---

## Why this passes review

An L7-L9 reviewer is not looking for more topics; they are looking for the
shape of engineering opinion. This repo demonstrates it in three moves:

- **Curation as judgment.** The scratchpad, anti-patterns, deprecated APIs, and
  hardcoded paths are gone, and this document says *why* they are gone.
- **Every assertion is reproducible.** `dotnet run` and `dotnet test` produce
  identical output and a green suite on any machine.
- **Framework awareness.** The Reflection section explicitly says "ASP.NET Core
  already does this, here is the mechanism beneath it" — the difference between
  someone who knows how to write C# and someone who knows when *not* to.