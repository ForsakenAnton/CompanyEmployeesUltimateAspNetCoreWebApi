namespace Shared.DataTransferObjects;

public record TokenDto(
    string AccessToken,
    string RefreshToken);

//public record TokenDto
//{
//    public string? AccessToken { get; init; }
//    public string? RefreshToken { get; init; }
//}
