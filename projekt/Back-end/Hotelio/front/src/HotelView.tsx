import { useEffect, useState } from "react";
import { Badge, Button, Card, CardImg, Col, ListGroup, ListGroupItem, Modal, Row, Spinner } from "react-bootstrap";
import { Link, useNavigate, useParams } from "react-router-dom";
import { formatRoute, Routes } from "./apiRoutes";
import { Role } from "./AuthenticationTypes";
import { useAuthState } from "./AuthProvider";
import { useAxiosContext } from "./AxiosInstanceProvider";
import { Hotel, UpdateHotelDTO } from "./HotelModels";
import { Room } from "./RoomModels";
import './CSS/HotelList.css';
import './CSS/Room.css';
//import placeholderImage from '../Images/placeholder_img.jpg';
import { capText } from "./Utils";
import { Trash } from "react-bootstrap-icons";
import { UrlManager } from "./urlmanager";

type HotelViewParams = {
    hotelId: string
}

function HotelView(props: any) {
    const authAxios = useAxiosContext();
    const auth = useAuthState();
    const [isLoading, setIsLoading] = useState(true);
    const [hotel, setHotel] = useState<Hotel | null>(null);
    const params = useParams<HotelViewParams>();

    useEffect(() => {
        if (!params.hotelId) return;

        const hotelId = params.hotelId;
        if (hotelId === null) return;

        const fetchHotel = async (id: string) => {
            try {
                const response = await authAxios.get<Hotel>(UrlManager.getDeleteHotelEndpoint(id));

                setHotel(response.data);
                setIsLoading(false);
            } catch (e) {
                console.error(e);
            }
        };

        fetchHotel(hotelId);

    }, []);

    let body;
    if (!auth.user) {
        body = undefined;
    } else if (isLoading) {
        body = <Spinner animation="border" />
     } else if (auth.user.Role == Role.Owner) {
         body = <HotelAdminView hotel={hotel!} />
    } else {
        body = <HotelBasicView hotel={hotel!} />
    }

    return (
        <div>
            {body}
        </div>
    );
}

type HotelProps = {
    hotel: Hotel
}

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
                    <div className="jumbotron mt-3">
                        {hotel.location}
                    </div>
                    <hr className="hr" />
                </Row>
                {/* <RoomList hotel={genre} /> */}
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

            const response = await authAxios.delete(UrlManager.getDeleteHotelEndpoint(params.hotelId));

            navigate(`/hotels`);

        } catch (e) {
            console.error(e);

            return Promise.reject(e);
        }
    }


    const name = hotel.name;
    const [location, setLocation] = useState<string>(hotel.location);
    const handleSave = async () => {

        const updateRequest: UpdateHotelDTO = {
            location: location,
            name: name
        };

        const route = formatRoute(Routes.UpdateHotel, hotel.id.toString());
        try {
            debugger;
            if (!params.hotelId) return;
            const response = await authAxios.put(UrlManager.getUpdateHotelEndpoint(params.hotelId), {name: updateRequest.name, location: updateRequest.location});

            if (response.status != 200) {
                throw Error('Failed to update genre');
            }

        } catch (e) {
            console.error(e);

            return Promise.reject(e);
        }
    };

    return (
        <div>
            <Col>
                <Modal className="text-light" backdrop={true} show={isDeleting} onHide={handleClose}>
                    <Modal.Header className="bg-dark">
                        <Modal.Title>Deleting {hotel.name}</Modal.Title>
                    </Modal.Header>
                    <Modal.Body className="bg-dark">
                        <p>Are you sure you want to delete this hotel?</p>
                    </Modal.Body>
                    <Modal.Footer className="bg-dark">
                        <Button variant="secondary" onClick={handleClose}>
                            Cancel
                        </Button>
                        <Button variant="danger" onClick={e => handleDelete()}>
                            Delete
                        </Button>
                    </Modal.Footer>
                </Modal>
                <Row>
                    <div className="d-flex flex-row">
                        <div className="me-auto">
                            <h1>{hotel.name}</h1>
                        </div>
                        { auth.user && auth.user.Role == Role.Owner &&
                            <div className="mt-auto mb-auto" onClick={handleShow}>
                                <a href="#" className="text-light">
                                    <Trash size={32} />
                                </a>
                            </div>
                        }
                    </div>
                    <hr className="hr" />
                </Row>
                <Row>
                    <textarea className="form-control jumbotron mb-3" name="description" id="description" value={location} onChange={e => setLocation(e.target.value)} />
                </Row>
                <Row xl={4}>
                    <Button variant="success" className="mb-3" onClick={e => handleSave()}>Save Changes</Button>
                </Row>
                <Row>
                    <hr className="hr" />
                </Row>
                {/* <RoomList hotel={hotel} /> */}
            </Col>
        </div>
    );
}

// function RoomList({ hotel }: HotelProps) {
//     return (
//         <Row>
//             <h4>Rooms</h4>
//             <div className="card-grid">
//                 {hotel.rooms.map((rooms, index) => {
//                     return (
//                         <RoomItem key={index} fetchRoute={rooms} />
//                     );
//                 })}
//                 {hotel.rooms.length == 0 && <p className="text-center fw-light">No rooms in this hotel</p>}
//             </div>
//         </Row>
//     );
// }

// type RoomItemProps = {
//     fetchRoute: string
// }

// function RoomItem({ fetchRoute }: RoomItemProps) {
//     const authAxios = useAxiosContext();

//     const [room, setRoom] = useState<Room | null>(null);

//     useEffect(() => {

//         const fetchSeries = async () => {

//             const route = `/api${fetchRoute}`;

//             try {

//                 const response = await authAxios.get<Room>(route);

//                 if (response.status !== 200) {
//                     throw Error('Could not fetch room');
//                 }

//                 setRoom(response.data);

//             } catch (e) {
//                 console.error(e);

//                 return Promise.reject(e);
//             }

//         };

//         fetchSeries();

//     }, []);

//     const handleImageError = (e: React.SyntheticEvent<HTMLImageElement, Event>) => {
//         console.log('Replaced broken image');
//         e.currentTarget.src = placeholderImage;
//     };

//     const cardDescription = room?.description
//         ? capText(room.description, 100)
//         : undefined;

//     const directorBadge = <Badge className="card-badge" key={'start'} bg="primary">Directors</Badge>
//     const directors = room != null && room.directors.length != 0
//         ? room.directors.map((director, idx) => {
//             return (<Badge className="card-badge" key={idx} bg="secondary">{director}</Badge>);
//         })
//         : [<Badge className="card-badge" key={'none'} bg="warning">No known directors</Badge>];
//     const cardDirectors = [directorBadge, ...directors];

//     const castBadge = <Badge className="card-badge" key={'start'} bg="primary">Cast</Badge>
//     const starringCast = room != null && room.starringCast.length != 0
//         ? room.starringCast.map((cast, idx) => {
//             return (<Badge className="card-badge" key={idx} bg="secondary">{cast}</Badge>);
//         })
//         : [<Badge className="card-badge" key={'none'} bg="warning">No known cast</Badge>];
//     const cardCast = [castBadge, ...starringCast];

//     const body = room === null
//         ? <Spinner animation="border" />
//         :
//         <Card className="card-item" bg="dark" text="white">
//             <Card.Img className="capped-img" variant="top" src={room.coverImagePath
//                 ? room.coverImagePath
//                 : placeholderImage
//             } onError={e => handleImageError(e)} />
//             <Card.Body>
//                 <div style={{ marginBottom: '1em' }}>
//                     <Card.Title>{room.name}</Card.Title>
//                     <Card.Text>
//                         {cardDescription}
//                     </Card.Text>
//                 </div>
//                 <ListGroup className="card-badges">
//                     <ListGroupItem className="bg-dark">{cardDirectors}</ListGroupItem>
//                     <ListGroupItem className="bg-dark">{cardCast}</ListGroupItem>
//                 </ListGroup>
//             </Card.Body>
//         </Card>

//     return (
//         <div className="series-item">
//             <Link to={fetchRoute}>
//                 {body}
//             </Link>
//         </div>
//     );
// }
export default HotelView;