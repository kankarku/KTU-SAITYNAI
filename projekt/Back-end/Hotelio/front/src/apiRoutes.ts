type Route = string;

const BASE_API_URI: string = "/api";

const Routes = {
    // hotel routes
    GetHotels: "api/hotel",
    GetHotel: "api/hotel/{0}",
    AddHotel: "api/hotel",
    DeleteHotel: "api/hotel/{0}",
    UpdateHotel: "api/hotel/{0}",

    //room routes
    GetRooms: "api/hotel/{0}/room",
    GetRoom: "api/hotel/{0}/room/{1}",
    AddRoom: "api/hotel/{0}/room",
    DeleteRoom: "api/hotel/{0}/room/{1}",
    UpdateRoom: "api/hotel/{0}/room/{1}",
    
    //additional service routes
    GetAdditionalServices: "api/hotel/{0}/room/{1}/additionalService",
    GetAdditionalService: "api/hotel/{0}/room/{1}/additionalService/{2}",
    AddAdditionalService: "api/hotel/{0}/room/{1}/additionalService",
    DeleteAdditionalService: "api/hotel/{0}/room/{1}/additionalService/{2}",
    UpdateAdditionalService: "api/hotel/{0}/room/{1}/additionalService/{2}",

    SignUp: BASE_API_URI + "/register",
    SignIn: BASE_API_URI + "/login",
    RefreshToken: BASE_API_URI + "/user/token/refresh",
    RevokeToken: BASE_API_URI + "/user/token/revoke",
};

const formatRoute = (route: Route, ...parameters: string[]) => {
    return route.replace(/{(\d+)}/g, function (match, number) {
        return typeof parameters[number] != 'undefined'
            ? parameters[number]
            : match;
    });
};

export { Routes, formatRoute };