namespace NLayer.Core.DTOs.RoleDtos
{
    public class RoleAssingDto
    {

        public int UserId { get; set; }
        public List<int> SelectedRoles { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public bool Exists { get; set; }
    }
}
