using System.ComponentModel.DataAnnotations;

namespace Hotelio.Auth.Model
{
    public record UserDto(string Id, string Email);

    public record LoginDto(string Email, string password);

    public record SuccessfulLoginDto(string AccessToken);

    public record RegisterUserDto([Required] string Email, [Required] string Password, [Required] string UserName);
}
