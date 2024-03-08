﻿using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.SpecificationModel
{
    public class BaseSpecification<T>: ISpecifications<T>
    {
        public BaseSpecification(Expression<Func<T, bool>> criteria
       )
    {
        Criteria = criteria;

    }
    public BaseSpecification()
    {
    }

    public Expression<Func<T, bool>> Criteria { get; set; }

    public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

    public Expression<Func<T, object>> OrderBy { get; private set; }

    public Expression<Func<T, object>> OrderByDescending { get; private set; }

    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool IsPagingEnabled { get; private set; }

        //   public int m {get;}
        protected void Filter(Expression<Func<T, bool>> filter)
        {
            Criteria = filter;

        }
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);

    }
    protected void AddOrderBy(Expression<Func<T, object>> OrderByExpression)
    {
        OrderBy = OrderByExpression;

    }
    protected void AddOrderByDescending(Expression<Func<T, object>> OrderByDesExpression)
    {
        OrderByDescending = OrderByDesExpression;
    }

    protected void ApplayPadding(int skip, int take)
    {
        Take = take;
        Skip = skip;
        IsPagingEnabled = true;
    }
}
}  