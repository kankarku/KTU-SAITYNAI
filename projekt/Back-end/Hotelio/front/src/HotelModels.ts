import { Room } from "./RoomModels"

type Hotel = {
    id: string,
    name: string,
    location: string,
    rooms: Room[]
}

type CreateHotelDTO = {
    name: string,
    location: string,
    rooms: Room[]
}

type UpdateHotelDTO = {
    location: string,
    name: string,
    rooms: Room[]
}

export type { Hotel, CreateHotelDTO, UpdateHotelDTO }