using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Foodies.Foody.Canteen.Core.Data;

namespace Foodies.Foody.Canteen.Core.Services
{
    public interface ICanteenService
    {
        Task<IEnumerable<Meal>> GetMealsAsync();

        Task<IEnumerable<Meal>> GetMealsAsync(DateTime date);

        Task<Dictionary<DateTime, IEnumerable<Meal>>> GetMealsOfWeekAsync();
    }
}
