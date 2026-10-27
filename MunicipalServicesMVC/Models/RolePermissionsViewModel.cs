using Permission = MunicipalServicesMVC.Domain.Models.Permission;
﻿namespace MunicipalServicesMVC.Models
{
    public class RolePermissionsViewModel
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; } = string.Empty;

        public List<MunicipalServicesMVC.Domain.Models.Permission> Permissions { get; set; } = new List<MunicipalServicesMVC.Domain.Models.Permission>();

        public List<int> SelectedPermissionIds { get; set; } = new List<int>();
    }
}