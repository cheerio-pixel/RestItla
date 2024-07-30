namespace RestItla.Application.DTO.User
{
    public class RegisterResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public string? JWToken { get; set; }
    }
}