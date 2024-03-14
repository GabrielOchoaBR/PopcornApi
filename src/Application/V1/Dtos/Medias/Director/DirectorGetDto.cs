﻿namespace Application.V1.Dtos.Medias.Director
{
    public record DirectorGetDto(string? Id, string Name, DateTime? BirthDate)
    {
        public DirectorGetDto():this(null, string.Empty, null)
        {
        }
    }
}
