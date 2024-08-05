using System.ComponentModel.DataAnnotations.Schema;

namespace RestWithASPNETUdemy.Model
{
    public class BaseEntity
    {
        [Column("id")]
        public long Id { get; set; }
    }
}
