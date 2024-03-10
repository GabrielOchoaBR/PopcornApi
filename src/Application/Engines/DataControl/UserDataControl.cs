using Domain.V1.Entities;
using MongoDB.Bson;

namespace Application.Engines.DataControl
{
    public class UserDataControl(IAuthorizationService authorizationService) : IUserDataControl
    {
        private readonly IAuthorizationService authorizationService = authorizationService;

        public void SetModifiedInfo(IDocument document)
        {
            string? userId = authorizationService.GetUserId();

            document.UpdatedAt = DateTime.UtcNow;
            document.UpdatedBy = new ObjectId(userId);
        }

        public void SetCreatedInfo(IDocument document)
        {
            string? userId = authorizationService.GetUserId();

            if (userId != null)
                document.CreatedBy = new ObjectId(userId);
        }
    }
}
