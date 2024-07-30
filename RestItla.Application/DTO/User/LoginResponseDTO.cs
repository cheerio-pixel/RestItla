namespace RestItla.Application.DTO.User
{
    public class LoginResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public string? JWTToken { get; set; }
    }
}