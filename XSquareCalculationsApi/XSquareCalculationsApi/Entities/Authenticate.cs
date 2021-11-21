using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XSquareCalculationsApi.Entities
{
    [Table("ATHENTICATES")]
    public sealed class Authenticate
    {
        [Key]
        [Column("AUTHENTICATE_ID")]
        public int AuthenticateId { get; set; }

        [Column("USER_ID", TypeName = "int")]
        public int UserId { get; set; }

        [Column("ID_TOKEN", TypeName = "text")]
        public string IdToken { get; set; }

        [Column("EXPIRED_DATETIME", TypeName = "datetime")]
        public DateTime ExpiredDateTime { get; set; }

        [Column("CREATED_TIME", TypeName = "datetime")]
        public DateTime CreatedTime { get; set; }
    }
}