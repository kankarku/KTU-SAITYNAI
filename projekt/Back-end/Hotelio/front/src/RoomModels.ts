//import { Link } from "./Other"

import { CreateHotelDTO, Hotel } from "./HotelModels";

enum RoomType {
  Basic = 0,
  Advanced = 1,
  Luxury = 2,
  Presidential = 3,
}

type Room = {
  roomLevel: number;
  roomType: RoomType;
};

type CreateRoomDTO = {
  roomLevel: number;
  roomType: number;
  hotel: CreateHotelDTO;
};

type UpdateRoomDTO = {
  roomLevel: number;
  roomType: number;
};

export type { Room, CreateRoomDTO, UpdateRoomDTO, RoomType };
