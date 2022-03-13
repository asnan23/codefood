using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Repository
{
    public interface IValidationErrorRepository
    {
        string Validate(ModelStateDictionary modelState);
    }
    public class ValidationErrorRepository : IValidationErrorRepository
    {
        public string Validate(ModelStateDictionary modelState)
        {
            var list = modelState.ToDictionary(x => x.Key, y => y.Value.Errors.Select(x => x.ErrorMessage).ToArray())
                  .Where(m => m.Value.Count() > 0);
            var message = new StringBuilder();
            foreach (var itm in list)
            {
                message.Append(string.Concat(string.Join(",", itm.Value.ToArray()), ", "));
            }
            return message.ToString();
        }

    }
}
