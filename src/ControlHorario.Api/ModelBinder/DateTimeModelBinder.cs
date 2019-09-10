using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace ControlHorario.Api.ModelBinder
{
    public class DateTimeModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var stringValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;

            if (bindingContext.ModelType == typeof(DateTime?) && string.IsNullOrWhiteSpace(stringValue))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
            }
            else
            {
                bindingContext.Result = DateTime.TryParse(stringValue, null, DateTimeStyles.RoundtripKind, out DateTime result)
                      ? ModelBindingResult.Success(result) : ModelBindingResult.Failed();

            }
            await Task.FromResult(0);
        }
    }
}
