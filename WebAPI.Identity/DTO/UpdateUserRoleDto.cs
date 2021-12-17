namespace WebAPI.Identity.DTO
{
    public class UpdateUserRoleDto
    {
        public string Email { get; set; }
        public bool Delete { get; set; }
        public string Role { get; set; }
    }
}
