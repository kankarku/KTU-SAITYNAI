using System.ComponentModel.DataAnnotations;

namespace Hotelio.Data
{
    public class BaseModel
    {
        [Key]
        public Guid Id { get; set; }
    }
}
