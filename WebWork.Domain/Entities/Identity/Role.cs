using Microsoft.AspNetCore.Identity;

namespace WebWork.Domain.Entities.Identity;

//обычно состоит из имени(роли) - подключаем пакет Identity
public class Role : IdentityRole
{
    public const string Administrators = "Administrators";

    public const string Users = "Users";
}
