import { useEffect, useState } from "react";
import {
  Badge,
  Button,
  Card,
  CardImg,
  Col,
  ListGroup,
  ListGroupItem,
  Modal,
  Row,
  Spinner,
} from "react-bootstrap";
import { Link, useNavigate, useParams } from "react-router-dom";
import { formatRoute, Routes } from "./apiRoutes";
import { Role } from "./AuthenticationTypes";
import { useAuthState } from "./AuthProvider";
import { useAxiosContext } from "./AxiosInstanceProvider";
import { CreateHotelDTO, Hotel, UpdateHotelDTO } from "./HotelModels";
import { CreateRoomDTO, Room } from "./RoomModels";
import "./CSS/HotelList.css";
import "./CSS/Room.css";
//import placeholderImage from '../Images/placeholder_img.jpg';
import { capText } from "./Utils";
import { PlusSquareFill, Trash } from "react-bootstrap-icons";
import { UrlManager } from "./urlmanager";
import CardHeader from "react-bootstrap/esm/CardHeader";

type HotelViewParams = {
  hotelId: string;
};

function HotelView(props: any) {
  const authAxios = useAxiosContext();
  const auth = useAuthState();
  const [isLoading, setIsLoading] = useState(true);
  const [hotel, setHotel] = useState<Hotel | null>(null);
  const params = useParams<HotelViewParams>();
  const [rooms, setRooms] = useState<Room[]>([]);

  useEffect(() => {
    if (!params.hotelId) return;

    const hotelId = params.hotelId;
    if (hotelId === null) return;

    // const fetchAllRooms = async () => {
    //     try {
    //         //const response = await authAxios.get<Room[]>(`api/hotel/${hotelId}/room`);
    //         const response = await authAxios.get<Room[]>(UrlManager.getAllRoomsEndpoint(hotelId));
    //         console.log(JSON.stringify(response));
    //         setRooms(response.data);
    //         //setIsLoading(false);
    //     } catch (e) {
    //         console.error(e);
    //     }

    // };

    const fetchHotel = async (id: string) => {
      try {
        const response = await authAxios.get<Hotel>(
          UrlManager.getDeleteHotelEndpoint(id)
        );

        setHotel(response.data);
        setIsLoading(false);
      } catch (e) {
        console.error(e);
      }
    };

    fetchHotel(hotelId);
    // fetchAllRooms();
  }, []);

  let body;
  if (!auth.user) {
    body = <Spinner animation="border" />;
  } else if (isLoading) {
    body = <Spinner animation="border" />;
  } else if (auth.user.Role == Role.Owner) {
    body = <HotelAdminView hotel={hotel!} />;
  } else if (auth.user.Role == Role.Client) {
    body = <HotelBasicView hotel={hotel!} />;
  }

  return <div>{body}</div>;
}

type HotelProps = {
  hotel: Hotel;
};

function HotelBasicView({ hotel }: HotelProps) {
  return (
    <div>
      <Col>
        <Row>
          <div className="d-flex flex-row">
            <div className="me-auto">
              <h1>{hotel.name}</h1>
            </div>
          </div>
          <hr className="hr" />
        </Row>
        <Row>
          <div className="jumbotron mt-3">{hotel.location}</div>
          <hr className="hr" />
        </Row>
        {<RoomList hotel={hotel} />}
      </Col>

      {/*<h1>{genre.name}</h1>*/}
      {/*<hr className="hr" />*/}
      {/*<div className="jumbotron mt-3">*/}
      {/*    {genre.description}*/}
      {/*</div>*/}
      {/*<hr className="hr" />*/}
      {/*<h2>Series</h2>*/}
      {/*<div className="card-grid">*/}
      {/*    {genre.series.map((series, index) => {*/}
      {/*        return (*/}
      {/*            <SeriesItem key={index} fetchRoute={series} />*/}
      {/*        );*/}
      {/*    })}*/}
      {/*    {genre.series.length == 0 && <p className="text-center fw-light">No series in this genre</p>}*/}
      {/*</div>*/}
    </div>
  );
}

function HotelAdminView({ hotel }: HotelProps) {
  const auth = useAuthState();
  const authAxios = useAxiosContext();
  const params = useParams<HotelViewParams>();
  const navigate = useNavigate();

  const [isDeleting, setDeleting] = useState<boolean>(false);
  const handleShow = () => setDeleting(true);
  const handleClose = () => setDeleting(false);
  const handleDelete = async () => {
    //if (hotel.rooms.length != 0) return;

    try {
      if (!params.hotelId) return;

      const response = await authAxios.delete(
        UrlManager.getDeleteHotelEndpoint(params.hotelId)
      );

      navigate(`/hotels`);
    } catch (e) {
      console.error(e);

      return Promise.reject(e);
    }
  };

  const name = hotel.name;
  const rooms: Room[] = [];
  const [location, setLocation] = useState<string>(hotel.location);
  const handleSave = async () => {
    const updateRequest: UpdateHotelDTO = {
      location: location,
      name: name,
      rooms: rooms,
    };

    const route = formatRoute(Routes.UpdateHotel, hotel.id.toString());
    try {
      if (!params.hotelId) return;
      const response = await authAxios.put(
        UrlManager.getUpdateHotelEndpoint(params.hotelId),
        {
          name: updateRequest.name,
          location: updateRequest.location,
          rooms: updateRequest.rooms,
        }
      );

      if (response.status != 200) {
        throw Error("Failed to update genre");
      }
    } catch (e) {
      console.error(e);

      return Promise.reject(e);
    }
  };

  const [roomLevel, setRoomLevel] = useState<number>();
  const [roomType, setRoomType] = useState<number>();
  const [isCreating, setCreating] = useState(false);
  const [emptyHotel, setEmptyHotel] = useState<CreateHotelDTO>({
    location: "",
    name: "",
    rooms: [],
  } as unknown as Hotel);
  const handleCreate = async () => {
    if (!roomLevel || !roomType) {
      alert("Info cant be empty");
      return;
    }

    const roomToCreate: CreateRoomDTO = {
      roomLevel: roomLevel,
      roomType: roomType,
      hotel: emptyHotel,
    };

    try {
      const response = await authAxios.post<CreateRoomDTO>(
        UrlManager.getCreateRoomEndpoint(hotel.id),
        {
          roomLevel: roomToCreate.roomLevel,
          roomType: roomToCreate.roomType,
          hotel: roomToCreate.hotel,
        }
      );

      if (response.status != 201) {
        throw Error("Could not create Hotel");
      }

      //setHotels([...hotels, response.data]);
      setRoomLevel(1);
      setRoomType(1);
      setCreating(false);
      setEmptyHotel({} as Hotel);
    } catch (e) {
      console.log(e);

      return Promise.reject(e);
    }
  };

  return (
    <div>
      <Col>
        <Modal
          className="text-light"
          backdrop={true}
          show={isDeleting}
          onHide={handleClose}
        >
          <Modal.Header className="bg-dark">
            <Modal.Title>Deleting {hotel.name}</Modal.Title>
          </Modal.Header>
          <Modal.Body className="bg-dark">
            <p>Are you sure you want to delete this hotel?</p>
          </Modal.Body>
          <Modal.Footer className="bg-dark">
            <Button variant="secondary" onClick={() => setCreating(true)}>
              Cancel
            </Button>
            <Button variant="danger" onClick={(e) => handleDelete()}>
              Delete
            </Button>
          </Modal.Footer>
        </Modal>
        <Row>
          <div className="d-flex flex-row">
            <div className="me-auto">
              <h1>{hotel.name}</h1>
            </div>
            {auth.user && auth.user.Role == Role.Owner && (
              <div className="mt-auto mb-auto" onClick={handleShow}>
                <a href="#" className="text-light">
                  <Trash size={32} />
                </a>
              </div>
            )}
          </div>
          <hr className="hr" />
        </Row>
        <Row>
          <textarea
            className="form-control jumbotron mb-3"
            name="description"
            id="description"
            value={location}
            onChange={(e) => setLocation(e.target.value)}
          />
        </Row>
        <Row xl={4}>
          <Button
            variant="success"
            className="mb-3"
            onClick={(e) => handleSave()}
          >
            Save Changes
          </Button>
        </Row>
        <Row>
          <hr className="hr" />
        </Row>
        <div className="card-grid"> </div>
        {<RoomList hotel={hotel} />}
      </Col>
      <div>
        <Modal
          className="text-light"
          backdrop={true}
          show={isCreating}
          onHide={handleClose}
        >
          <Modal.Header className="bg-dark">
            <Modal.Title>Create a new room</Modal.Title>
          </Modal.Header>
          <Modal.Body className="bg-dark">
            <form>
              <div className="form-group mb-3">
                <label htmlFor="name" className="form-label">
                  Room Level
                </label>
                <input
                  type="text"
                  className="form-control"
                  name="name"
                  id="name"
                  onChange={(e) => setRoomLevel(parseInt(e.target.value))}
                  value={roomLevel}
                />
              </div>
              <div className="form-group mb-3">
                <label htmlFor="description" className="form-label">
                  Room Type
                </label>
                <textarea
                  className="form-control"
                  name="description"
                  id="description"
                  onChange={(e) => setRoomType(parseInt(e.target.value))}
                  value={roomType}
                ></textarea>
              </div>
            </form>
          </Modal.Body>
          <Modal.Footer className="bg-dark">
            <Button variant="secondary" onClick={() => setCreating(false)}>
              Cancel
            </Button>
            <Button variant="primary" onClick={(e) => handleCreate()}>
              Create
            </Button>
          </Modal.Footer>
        </Modal>
        <div className="genre-list-page">
          {auth.user && auth.user.Role == Role.Owner && (
            <div className="create-button" onClick={() => setCreating(true)}>
              <PlusSquareFill size={32} />
            </div>
          )}
          <hr className="hr" />
          {/* <div className="genre-list">{body}</div> */}
        </div>
      </div>
    </div>
  );
}

function RoomList({ hotel }: HotelProps) {
  return (
    <Row>
      <h4>Rooms</h4>
      <div className="card-grid">
        {hotel.rooms?.map((room, index) => {
          return (
            <p key={index}>
              Room level: {room.roomLevel} <br></br>
              Room Type: {nameOfEnum(room.roomType)}
            </p>
          );
        })}
        {hotel.rooms?.length == 0 && (
          <p className="text-center fw-light">No rooms in this hotel</p>
        )}
      </div>
    </Row>
  );
}

function nameOfEnum(enumValue: number) {
  var name = "nothing";
  switch (enumValue) {
    case 0:
      name = "Basic";
      break;
    case 1:
      name = "Advanced";
      break;
    case 2:
      name = "Luxury";
      break;
    case 3:
      name = "Presidential";
      break;
  }
  return name;
}

type RoomItemProps = {
  fetchRoute: string;
  hotel: Hotel;
};

function RoomItem(props: RoomItemProps) {
  const authAxios = useAxiosContext();

  const [room, setRoom] = useState<Room | null>(null);

  useEffect(() => {
    const fetchRooms = async () => {
      const route = `api/hotel/${props.hotel.id}/room`;
      const response = await authAxios.get<Room>(route);
      try {
        console.log(JSON.stringify(response));

        if (response.status !== 200) {
          throw Error("Could not fetch room");
        }

        setRoom(response.data);
      } catch (e) {
        console.error(e);

        return Promise.reject(e);
      }
    };

    fetchRooms();
  }, []);

  // const handleImageError = (e: React.SyntheticEvent<HTMLImageElement, Event>) => {
  //     console.log('Replaced broken image');
  //     e.currentTarget.src = 'placeholderImage';
  // };

  // const cardDescription = room?.description
  //     ? capText(room.description, 100)
  //     : undefined;

  // const directorBadge = <Badge className="card-badge" key={'start'} bg="primary">Directors</Badge>
  // const directors = room != null && room.directors.length != 0
  //     ? room.directors.map((director, idx) => {
  //         return (<Badge className="card-badge" key={idx} bg="secondary">{director}</Badge>);
  //     })
  //     : [<Badge className="card-badge" key={'none'} bg="warning">No known directors</Badge>];
  // const cardDirectors = [directorBadge, ...directors];

  // const castBadge = <Badge className="card-badge" key={'start'} bg="primary">Cast</Badge>
  // const starringCast = room != null && room.starringCast.length != 0
  //     ? room.starringCast.map((cast, idx) => {
  //         return (<Badge className="card-badge" key={idx} bg="secondary">{cast}</Badge>);
  //     })
  //     : [<Badge className="card-badge" key={'none'} bg="warning">No known cast</Badge>];
  // const cardCast = [castBadge, ...starringCast];

  // const body = room === null
  //     ? <Spinner animation="border" />
  //     :
  //     <Card className="card-item" bg="dark" text="white">
  //         <Card.Img className="capped-img" variant="top" src={room.coverImagePath
  //             ? room.coverImagePath
  //             : 'placeholderImage'
  //         } onError={e => handleImageError(e)} />
  //         <Card.Body>
  //             <div style={{ marginBottom: '1em' }}>
  //                 <Card.Title>{room.roomLevel}</Card.Title>
  //                 <Card.Text>
  //                     {room.roomType}
  //                 </Card.Text>
  //             </div>
  //             <ListGroup className="card-badges">
  //                 <ListGroupItem className="bg-dark">{cardDirectors}</ListGroupItem>
  //                 <ListGroupItem className="bg-dark">{cardCast}</ListGroupItem>
  //             </ListGroup>
  //         </Card.Body>
  //     </Card>

  return (
    <div className="series-item">
      <p>{room?.roomLevel}</p>
      {/* <Link to={props.hotel}>
                {room?.roomLevel}
            </Link> */}
    </div>
  );
}
export default HotelView;
