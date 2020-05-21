using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unidash.Canteen.Core.Data;

namespace Unidash.Canteen.Core.Services
{
    public interface ICanteenService
    {
        Task<IEnumerable<Meal>> GetMealsAsync();

        Task<IEnumerable<Meal>> GetMealsAsync(DateTime date);

        Task<IEnumerable<MealsDateTuple>> GetMealsOfWeekAsync();
    }
}
