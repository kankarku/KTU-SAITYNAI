type Hotel = {
    id: string,
    name: string,
    location: string,
    rooms: string[]
}

type CreateHotelDTO = {
    name: string,
    location: string
}

type UpdateHotelDTO = {
    location: string,
    name: string
}

export type { Hotel, CreateHotelDTO, UpdateHotelDTO }