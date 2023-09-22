namespace NLayer.Core.DTOs.TokenDtos
{
    public class TokenDto
    {

        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Role { get; set; }
        public string? AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public int UserId { get; set; }
    }
}
