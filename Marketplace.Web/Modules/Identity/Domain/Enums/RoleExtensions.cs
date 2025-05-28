namespace Marketplace.Web.Modules.Identity.Domain.Enums
{
    public static class RoleExtensions
    {
        public static RoleEnum ToRoleEnum(this string role)
        {
            return Enum.Parse<RoleEnum>(role);
        }
    }
}
