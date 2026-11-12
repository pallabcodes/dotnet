using FluentAssertions;
using Shared.Domain.Common;
using Xunit;

namespace Shared.Tests;

public class ValueObjectTests
{
    private sealed class Money : ValueObject
    {
        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public decimal Amount { get; }
        public string Currency { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
    }

    private sealed class EntityStub : Entity
    {
        public EntityStub(Guid id) => Id = id;
    }

    [Fact]
    public void ValueObjects_WithSameComponents_AreEqual()
    {
        (new Money(10m, "USD") == new Money(10m, "USD")).Should().BeTrue();
        (new Money(10m, "USD") == new Money(10m, "EUR")).Should().BeFalse();
        (new Money(10m, "USD") == (Money?)null).Should().BeFalse();
        (null as Money == null as Money).Should().BeTrue();
    }

    [Fact]
    public void Entities_WithSameIdAndType_AreEqual()
    {
        var id = Guid.NewGuid();

        (new EntityStub(id) == new EntityStub(id)).Should().BeTrue();
        (new EntityStub(id) == new EntityStub(Guid.NewGuid())).Should().BeFalse();
    }
}
