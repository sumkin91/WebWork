using Microsoft.AspNetCore.Identity;

namespace WebWork.Domain.Entities.Identity;

//обычно состоит из имени и пароля(хэш) - подключаем пакет Identity
public class User : IdentityUser
{

    public const string Administrator = "Admin";

    public const string AdminPassword = "AdPass_123";
    public override string ToString() => UserName;

}
