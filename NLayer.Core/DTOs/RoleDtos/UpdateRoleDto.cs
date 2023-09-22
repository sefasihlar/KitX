namespace NLayer.Core.DTOs.RoleDtos
{
    public class UpdateRoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public Boolean Condition { get; set; }
    }
}
