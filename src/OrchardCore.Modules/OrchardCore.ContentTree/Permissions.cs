using System.Collections.Generic;
using OrchardCore.Security.Permissions;

namespace OrchardCore.ContentTree
{
    // todo
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageContentTree = new Permission("ManageContentTree", "Manage the content tree");

        public IEnumerable<Permission> GetPermissions()
        {
            return new[] { ManageContentTree };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new[] { ManageContentTree }
                }
            };
        }
    }
}