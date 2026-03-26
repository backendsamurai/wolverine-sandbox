using System.Linq.Expressions;

namespace WolverineSandbox.Domain.Abstractions;

public interface ISpecification<T> where T: IAggregateRoot
{
    bool IsSatisfiedBy(T candidate);

    Expression<Func<T, bool>> ToExpression();
}

public abstract class Specification<T> : ISpecification<T> where T : IAggregateRoot
{
    public bool IsSatisfiedBy(T candidate)
    {
        var predicate = ToExpression().Compile();
        return predicate(candidate);
    }

    public abstract Expression<Func<T, bool>> ToExpression();

    public static ISpecification<T> operator &(Specification<T> left, Specification<T> right)
    {
        return new AndSpecification<T>(left, right);
    }

    public static ISpecification<T> operator |(Specification<T> left, Specification<T> right)
    {
        return new OrSpecification<T>(left, right);
    }

    public static ISpecification<T> operator !(Specification<T> specification)
    {
        return new NotSpecification<T>(specification);
    }
}

public class AndSpecification<T> : Specification<T> where T : IAggregateRoot
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    public AndSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpr = _left.ToExpression();
        var rightExpr = _right.ToExpression();

        var parameter = Expression.Parameter(typeof(T));

        var body = Expression.AndAlso(
            Expression.Invoke(leftExpr, parameter),
            Expression.Invoke(rightExpr, parameter)
        );

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}

public class OrSpecification<T> : Specification<T> where T : IAggregateRoot
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    public OrSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpr = _left.ToExpression();
        var rightExpr = _right.ToExpression();

        var parameter = Expression.Parameter(typeof(T));

        var body = Expression.OrElse(
            Expression.Invoke(leftExpr, parameter),
            Expression.Invoke(rightExpr, parameter)
        );

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}

public class NotSpecification<T> : Specification<T> where T : IAggregateRoot
{
    private readonly ISpecification<T> _specification;

    public NotSpecification(ISpecification<T> specification)
    {
        _specification = specification;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var expr = _specification.ToExpression();
        var parameter = Expression.Parameter(typeof(T));

        var body = Expression.Not(Expression.Invoke(expr, parameter));

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}