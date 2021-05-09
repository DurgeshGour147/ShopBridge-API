using LinqToDB.Mapping;
using System;

namespace Jewelry_Store.Domain
{
    [Table(Name = "tblUser")]
    public class UserDomain
    {
        [Column, PrimaryKey, Identity]
        public int U_Id { get; set; }
        [Column]
        public string U_FirstName { get; set; }
        [Column]
        public string U_LastName { get; set; }
        [Column]
        public string U_UserName { get; set; }
        [Column]
        public string U_Password { get; set; }
        [Column]
        public string U_EmailId { get; set; }
        [Column]
        public string U_PhoneNumber { get; set; }
        [Column]
        public bool U_IsActive { get; set; }
        [Column]
        public DateTime U_CreatedOn { get; set; }
        [Column]
        public string U_CreatedBy { get; set; }
        [Column]
        public DateTime U_ModifiedOn { get; set; }
        [Column]
        public string U_ModifiedBy { get; set; }
    }

    [Table(Name = "tblUserRole")]
    public class UserRoleDomain
    {
        [Column, PrimaryKey, Identity]
        public int UR_Id { get; set; }
        [Column]
        public int UR_U_Id { get; set; }
        [Column]
        public int UR_R_Id { get; set; }
        [Column]
        public bool UR_IsActive { get; set; }
        [Column]
        public DateTime UR_CreatedOn { get; set; }
        [Column]
        public string UR_CreatedBy { get; set; }
        [Column]
        public DateTime UR_ModifiedOn { get; set; }
        [Column]
        public string UR_ModifiedBy { get; set; }

    }


    [Table(Name = "tblRole")]
    public class RoleDomain
    {
        [Column, PrimaryKey, Identity]
        public int R_Id { get; set; }
        [Column]
        public string R_Name { get; set; }
        [Column]
        public bool R_IsActive { get; set; }
        [Column]
        public DateTime R_CreatedOn { get; set; }
        [Column]
        public string R_CreatedBy { get; set; }
        [Column]
        public DateTime R_ModifiedOn { get; set; }
        [Column]
        public string R_ModifiedBy { get; set; }
    }
}
