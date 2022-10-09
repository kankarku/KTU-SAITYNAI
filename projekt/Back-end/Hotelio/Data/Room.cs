using Hotelio.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotelio.Data
{
    public class Room : BaseModel
    {
        public int RoomLevel { get; set; }
        public RoomType RoomType { get; set; }
        public Hotel Hotel { get; set; }
        public List<AdditionalService> AdditionalServices;
    }
}
