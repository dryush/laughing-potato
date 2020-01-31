using mail_bank.App;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mail_bank.Formats
{

    //TODO: Если схожих форматов много, можно написать полноценный парсер для POCO
    // используя обход по полям/свойствам через рефлексию
    public class ProductInputFormatter : TextInputFormatter
    {
        public ProductInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/product"));
            SupportedEncodings.Add(Encoding.UTF8);
        }

        // TODO: Понятные ошибки формата
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding) {
            
            var reader = new StreamReader(
                context.HttpContext.Request.Body, 
                encoding);

            var inp = await reader.ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(inp))
                return await InputFormatterResult.NoValueAsync();

            try
            {
                var values = inp.Split("~").Select(v =>
                {
                    var nameValuePair = v.Split("=");
                    if (nameValuePair.Length != 2)
                        throw new FormatException();
                    return new
                    {
                        Name = nameValuePair[0],
                        Value = nameValuePair[1]
                    };
                });

                var namedValues = values.ToDictionary(v => v.Name, v => v.Value);

                if (namedValues.Count != 2)
                    throw new FormatException();

                if (!namedValues.TryGetValue("Name", out var name))
                    throw new FormatException();

                if (!namedValues.TryGetValue("Description", out var description))
                    throw new FormatException();

                return await InputFormatterResult.SuccessAsync (
                    new NewProduct(name, description)
                );

            } 
            catch 
            {
                return await InputFormatterResult.FailureAsync();
            }
        }

        // TODO: ProductType
        protected override bool CanReadType(Type type) => base.CanReadType(typeof(NewProduct));
    }
}
