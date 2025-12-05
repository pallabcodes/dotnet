# ADR-001: Modular Monolith for SaaSUsageBilling

## Status
Accepted

## Context
We need clear boundaries for billing logic (customers, plans, subscriptions, invoicing, usage) without the operational overhead of microservices. The team is small and latency between components must remain low.

## Decision
- Implement a **modular monolith** with bounded contexts:
  - CustomerManagement
  - PlanManagement
  - SubscriptionManagement
  - Billing
- Use MediatR vertical slices with validation/logging/idempotency behaviors.
- Persist with EF Core + UnitOfWork + optimistic concurrency; outbox/inbox patterns for reliable delivery.
- Expose a versioned HTTP API (v1) with JWT auth, rate limiting, and standardized errors.

## Consequences
- High cohesion within a single deployable; simple local dev and CI.
- Clear seams to extract services later if scale or org boundaries require.
- Outbox + idempotency provide durability even inside a monolith.
- Observability and health endpoints are centralized.

