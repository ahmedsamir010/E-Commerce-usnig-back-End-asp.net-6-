using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public interface ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T,bool>>  Criteria { get; set; } 
        public List<Expression<Func<T,object>>> Includes  { get; set; } 


        public Expression<Func<T,object>> OrderBy { get; set; } 
        public Expression<Func<T,object>> OrderByDescending { get; set; }


        public int Take { get; set; }
        public int Skip { get; set; }

        public bool IsPaginaionEnable { get; set; }


    }
}
