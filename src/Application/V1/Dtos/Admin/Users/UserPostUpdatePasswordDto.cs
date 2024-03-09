namespace Application.V1.Dtos.Admin.Users
{
    public record UserPostUpdatePasswordDto(string Id,
                                            string OldPassword,
                                            string NewPassword)
    {
    }
}
