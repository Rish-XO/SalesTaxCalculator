namespace SalesTaxCalculator.Core.Domain.ValueObjects;

public class Money : IEquatable<Money>, IComparable<Money>
{
    private readonly decimal _amount;

    public Money(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));
        
        _amount = Math.Round(amount, 2);
    }

    public decimal Amount => _amount;

    public Money Add(Money other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));
        
        return new Money(_amount + other._amount);
    }

    public Money Subtract(Money other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));
        
        return new Money(_amount - other._amount);
    }

    public Money Multiply(decimal factor)
    {
        return new Money(_amount * factor);
    }

    public Money RoundUpToNearest(decimal nearest)
    {
        if (nearest <= 0)
            throw new ArgumentException("Rounding value must be positive", nameof(nearest));
        
        var rounded = Math.Ceiling(_amount / nearest) * nearest;
        return new Money(rounded);
    }

    public static Money Zero => new Money(0);

    public static Money operator +(Money left, Money right)
    {
        return left.Add(right);
    }

    public static Money operator -(Money left, Money right)
    {
        return left.Subtract(right);
    }

    public static Money operator *(Money money, decimal factor)
    {
        return money.Multiply(factor);
    }

    public static implicit operator decimal(Money money)
    {
        return money._amount;
    }

    public static implicit operator Money(decimal amount)
    {
        return new Money(amount);
    }

    public bool Equals(Money? other)
    {
        if (other is null) return false;
        return _amount == other._amount;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Money);
    }

    public override int GetHashCode()
    {
        return _amount.GetHashCode();
    }

    public int CompareTo(Money? other)
    {
        if (other is null) return 1;
        return _amount.CompareTo(other._amount);
    }

    public override string ToString()
    {
        return _amount.ToString("F2");
    }
}