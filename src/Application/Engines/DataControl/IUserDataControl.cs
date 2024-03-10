using Domain.V1.Entities;

namespace Application.Engines.DataControl
{
    public interface IUserDataControl
    {
        void SetCreatedInfo(IDocument document);
        void SetModifiedInfo(IDocument document);
    }
}