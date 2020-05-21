using System;
using System.Collections.Generic;

namespace Unidash.Canteen.Core.Data
{
    public struct MealsDateTuple
    {
        public DateTime Date { get; set; }

        public IEnumerable<Meal> Meals { get; set; }

        public MealsDateTuple(DateTime date, IEnumerable<Meal> meals)
        {
            Date = date;
            Meals = meals;
        }
    }
}