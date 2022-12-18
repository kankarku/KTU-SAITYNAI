import { Room } from "./RoomModels";

export class UrlManager {
  static _serverUrl = "https://localhost:7181/api/";
  //static _serverUrl = "https://bendrabutissystem.azurewebsites.net/api/";

  /** Auth */

  public static getLoginEndpoint() {
    return `${UrlManager._serverUrl}login`;
  }

  public static getRegisterEndpoint() {
    return `${UrlManager._serverUrl}register`;
  }

  /** Dorm controller endpoints */
  private static _HotelController: string = `${UrlManager._serverUrl}hotel`;
  public static getAllHotelsEndpoint() {
    return `${UrlManager._HotelController}`;
  }
  public static getDeleteHotelEndpoint(id: string) {
    return `${UrlManager._HotelController}/${id}`;
  }
  public static getCreateHotelEndpoint(
    name: string,
    location: string,
    rooms: Room[]
  ) {
    return `${UrlManager._HotelController}`;
  }
  public static getUpdateHotelEndpoint(id: string) {
    return `${UrlManager._HotelController}/${id}`;
  }
  /** Dorm controller endpoints */
  private static _RoomController: string = `${UrlManager._serverUrl}room`;

  public static getAllRoomsEndpoint(id: string) {
    return `${UrlManager._HotelController}/${id}/room`;
  }
  public static getCreateRoomEndpoint(id: string) {
    return `${UrlManager._HotelController}/${id}/room`;
  }

  // public static getDeleteDormEndpoint(id: number) {
  //   return `${UrlManager._DormitoriesController}/${id}`;
  // }

  // public static getCreateDormEndpoint(
  //   name: string,
  //   address: string,
  //   roomCapacity: number
  // ) {
  //   return `${UrlManager._DormitoriesController}?name=${name}&address=${address}&roomCapacity=${roomCapacity}`;
  // }

  // public static getUpdateDormEndpoint(
  //   id: number,
  //   name: string,
  //   address: string,
  //   roomCapacity: number
  // ) {
  //   return `${UrlManager._DormitoriesController}/${id}?name=${name}&address=${address}&roomCapacity=${roomCapacity}`;
  // }

  // /** Floor controller endpoints */
  // private static _FloorsController: string = `${UrlManager._serverUrl}Floors`;

  // public static getAllDormFloorsEndpoint() {
  //   return `${UrlManager._FloorsController}`;
  // }

  // public static getDeleteFloorEndpoint(id: number) {
  //   return `${UrlManager._FloorsController}/${id}`;
  // }

  // public static getCreateFloorEndpoint(number: number, dormId: number) {
  //   return `${UrlManager._FloorsController}?number=${number}&dormId=${dormId}`;
  // }

  // public static getUpdateFloorEndpoint(id: number, number: number) {
  //   return `${UrlManager._FloorsController}/${id}?number=${number}`;
  // }

  // /** Room controller endpoints */
  // private static _RoomsController: string = `${UrlManager._serverUrl}Rooms`;

  // public static getFreeRoomsEndpoint(dormId: number) {
  //   return `${UrlManager._RoomsController}/GetFreeRooms?dormId=${dormId}`;
  // }

  // public static getRoomsEndpoint() {
  //   return `${UrlManager._RoomsController}`;
  // }

  // public static getDeleteRoomEndpoint(id: number) {
  //   return `${UrlManager._RoomsController}/${id}`;
  // }

  // public static getCreateRoomEndpoint(
  //   floorId: number,
  //   number: number,
  //   numberOfLivingPlaces: number,
  //   area: number
  // ) {
  //   return `${UrlManager._RoomsController}?floorId=${floorId}&number=${number}&numberOfLivingPlaces=${numberOfLivingPlaces}&area=${area}`;
  // }

  // public static getUpdateRoomEndpoint(
  //   id: number,
  //   number: number,
  //   numberOfLivingPlaces: number,
  //   area: number
  // ) {
  //   return `${UrlManager._RoomsController}/${id}?number=${number}&numberOfLivingPlaces=${numberOfLivingPlaces}&area=${area}`;
  // }

  // /** Request controller endpoints */
  // private static _RequestController: string = `${UrlManager._serverUrl}Requests`;

  // public static geAllRequestsEndpoint() {
  //   return `${UrlManager._RequestController}`;
  // }

  // public static getDeleteRequestEndpoint(id: number) {
  //   return `${UrlManager._RequestController}/${id}`;
  // }

  // public static getCreateRequestEndpoint() {
  //   return `${UrlManager._RequestController}`;
  // }

  // public static getUpdateRequestEndpoint(id: number, description: string) {
  //   return `${UrlManager._RequestController}/${id}?description=${description}`;
  // }
}
