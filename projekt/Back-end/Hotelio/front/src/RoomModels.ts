//import { Link } from "./Other"

type Room = {
    id: number,
    name: string,
    description: string | null,
    coverImagePath: string | null,
    directors: string[],
    starringCast: string[],
 //   poster: Link,
    genres: string[],
    reviews: string[],
}

type CreateRoomDTO = {
    name: string,
    description: string | null,
    coverImagePath: string | null,
    directors: string[],
    starringCast: string[],
    poster: number,
    genres: number[]
}

type UpdateRoomDTO = {
    coverImagePath: string | null,
    description: string | null,
    directors: string[],
    starringCast: string[]
}

export type { Room, CreateRoomDTO, UpdateRoomDTO }