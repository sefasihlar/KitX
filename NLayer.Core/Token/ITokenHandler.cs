
using NLayer.Core.DTOs.TokenDtos;

namespace NLayer.Core.Token
{

    public interface ITokenHandler
    {
        TokenDto CreateAccessToken(TokenInfo tokenInfo);
    }

}
