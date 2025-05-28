namespace Marketplace.Web.Modules.Identity.Application.Queries.GetUsers
{
    public record UserDto
    {
        public Guid Id { get; init; }
        public string? Email { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? CompanyName { get; init; }
        public DateTime CreatedAt { get; init; }
        public IList<string> Roles { get; init; } = new List<string>();
    }
}
