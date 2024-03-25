using Application.V1.Dtos.Admin.Users;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PopcornApi.Swagger
{
    public class LoginSampleFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(UserGetByNameAndPasswordDto))
            {
                schema.Example = new OpenApiObject()
                {
                    ["name"] = new OpenApiString("root"),
                    ["password"] = new OpenApiString("MyPassword@123"),
                };
            }
        }
    }
}
