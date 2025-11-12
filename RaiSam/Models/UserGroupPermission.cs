using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class UserGroupPermission
    {
        public IEnumerable<Models.prs_TreeUserGroupSelect> TreeUserGroup { get; set; }
        public IEnumerable<Models.prs_TreeUserGroupWithGrantSelect> TreeUserGroupWithGrant { get; set; }
    }
}