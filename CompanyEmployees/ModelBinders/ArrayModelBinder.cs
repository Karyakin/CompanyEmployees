using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CompanyEmployees.ModelBinders
{
    public class ArrayModelBinder : IModelBinder
    {
        #region Описание метода
        /// <summary>
        /// На первый взгляд этот код может быть трудным для понимания, но как только мы его объясним, его будет легче понять.
        // Создаем подшивку модели для IEnumerable тип.Следовательно, мы должны проверить, относится ли наш параметр к тому же типу.
        ///Затем мы извлекаем значение (строку идентификаторов GUID, разделенных запятыми) с ValueProvider.GetValue() выражение.Поскольку это строка типа, мы просто проверяем, является ли она пустой или пустой.Если это так, мы возвращаем null в качестве результата, потому что у нас есть проверка на null в нашем действии в контроллере.Если нет, идем дальше.
        ///в genericType переменной, с помощью отражения мы сохраняем тип IEnumerable состоит из.В нашем случае это GUID.С converter переменной создаем преобразователь в тип GUID.Как вы можете
        ///видите, мы не просто форсировали тип GUID в этом связывателе модели; вместо этого мы проверили, какой вложенный тип IEnumerable параметр, а затем создал преобразователь для этого точного типа, сделав это связующее универсальным.
        ///После этого создаем массив типа object (objectArray), которые состоят из всех значений GUID, которые мы отправили в API, а затем создаем массив типов GUID (guidArray) скопируйте все значения из objectArray к guidArrayи назначьте его bindingContext.
        ///Вот и все.Теперь нам нужно внести небольшие изменения в
        ///GetCompanyCollection действие:
        #endregion
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed(); 
                return Task.CompletedTask;
            }

            var providedValue = bindingContext.ValueProvider
            .GetValue(bindingContext.ModelName)
            .ToString(); if (string.IsNullOrEmpty(providedValue))
            {
                bindingContext.Result = ModelBindingResult.Success(null); 
                return Task.CompletedTask;
            }

            var genericType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            var converter = TypeDescriptor.GetConverter(genericType);

            var objectArray = providedValue.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => converter.ConvertFromString(x.Trim()))
            .ToArray();

            var guidArray = Array.CreateInstance(genericType, objectArray.Length); objectArray.CopyTo(guidArray, 0);
            bindingContext.Model = guidArray;

            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model); 
            return Task.CompletedTask;
        }
    }
}
