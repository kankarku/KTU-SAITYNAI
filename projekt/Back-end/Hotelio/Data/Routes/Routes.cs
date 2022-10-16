namespace Hotelio.Data.Routes
{
    public class Routes
    {
        // hotel routes
        public const string GetHotels = "api/hotel";
        public const string GetHotel = "api/hotel/{id:Guid}";
        public const string AddHotel = "api/hotel";
        public const string DeleteHotel = "api/hotel/{id:Guid}";
        public const string UpdateHotel = "api/hotel/{id:Guid}";

        //room routes
        public const string GetRooms = "api/hotel/{hotelId:Guid}/room";
        public const string GetRoom = "api/hotel/{hotelId:Guid}/room/{roomId:Guid}";
        public const string AddRoom = "api/hotel/{hotelId:Guid}/room";
        public const string DeleteRoom = "api/hotel/{hotelId:Guid}/room/{roomId:Guid}";
        public const string UpdateRoom = "api/hotel/{hotelId:Guid}/room/{roomId:Guid}";
        
        //additional service routes
        public const string GetAdditionalServices = "api/hotel/{hotelId:Guid}/room/{roomId:Guid}/additionalService";
        public const string GetAdditionalService = "api/hotel/{hotelId:Guid}/room/{roomId:Guid}/{serviceId:Guid}";
        public const string AddAdditionalService = "api/hotel/{hotelId:Guid}/room/{roomId:Guid}/additionalService";
        public const string DeleteAdditionalService = "api/hotel/{hotelId:Guid}/room/{roomId:Guid}/{serviceId:Guid}";
        public const string UpdateAdditionalService = "api/hotel/{hotelId:Guid}/room/{roomId:Guid}/{serviceId:Guid}";
    }
}
