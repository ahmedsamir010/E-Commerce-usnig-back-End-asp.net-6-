using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specification;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> InputQuery, ISpecification<TEntity> spec)
        {
            var query = InputQuery;

            if(spec.Criteria != null) 
            {
                query = query.Where(spec.Criteria); 
            }

            if(spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy);


            if (spec.OrderByDescending != null)
                query = query.OrderBy(spec.OrderByDescending);


            if (spec.IsPaginationEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            if (spec.Includes != null)
            {
                query = spec.Includes.Aggregate(query, (currentQuery, includeProperty) => currentQuery.Include(includeProperty));
            }

            //string[] names = { "Ahmed", "Samir", "Younes", "Sakr" };

            //string Message = "Hello";

            //Message = names.Aggregate(Message, (current, name) => $"{current} {name}"); 



            return query;
        }

    }
}
