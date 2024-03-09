using Application.V1.Dtos.Shared;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PopcornApi.Swagger
{
    public class GetAllSampleFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(int))
            {
                if (context.MemberInfo.Name == "PageIndex")
                    schema.Example = new OpenApiInteger(1);
                if (context.MemberInfo.Name == "PageSize")
                    schema.Example = new OpenApiInteger(10);
            }
        }
    }
}
