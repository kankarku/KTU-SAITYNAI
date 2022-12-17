import { useEffect, useState } from "react";
import { Button, Modal, Spinner } from "react-bootstrap";
import { PlusSquareFill } from "react-bootstrap-icons";
import { Link } from "react-router-dom";
import { Routes } from "./apiRoutes";
import { Role } from "./AuthenticationTypes";
import { useAuthState } from "./AuthProvider";
import { useAxiosContext } from "./AxiosInstanceProvider";
import { CreateHotelDTO, Hotel } from "./HotelModels";
import { UrlManager } from "./urlmanager";
import './CSS/HotelList.css';

function HotelList(props: any) {
    const authAxios = useAxiosContext();
    const [hotels, setHotels] = useState<Hotel[]>([]);
    const [isLoading, setIsLoading] = useState(true);

    const [isCreating, setCreating] = useState(false);
    const [hotelName, setHotelName] = useState<string>();
    const [hotelLocation, setHotelLocation] = useState<string>();

    const auth = useAuthState();

    const handleShow = () => setCreating(true);
    const handleClose = () => setCreating(false);
    const handleCreate = async () => {

        if (!hotelName || !hotelLocation) {
            alert('Name and location cannot be empty');
            return;
        }

        const hotelToCreate: CreateHotelDTO = {
            name: hotelName,
            location: hotelLocation
        };

        try {
            const response = await authAxios.post<Hotel>(UrlManager.getCreateHotelEndpoint(hotelToCreate.name, hotelToCreate.location), {name: hotelToCreate.name, location: hotelToCreate.location});

            if (response.status != 201) {
                throw Error('Could not create Hotel');
            }

            setHotels([...hotels, response.data]);
            setHotelName('');
            setHotelLocation('');
            setCreating(false);

        } catch (e) {
            console.log(e);

            return Promise.reject(e);
        }
    }

    useEffect(() => {
        const fetchHotels = async () => {

            try {
                const response = await authAxios.get<Hotel[]>(UrlManager.getAllHotelsEndpoint());

                setHotels(response.data);
                setIsLoading(false);
            } catch (e) {
                console.error(e);
            }

        };
        fetchHotels();
    }, []);

    let body = isLoading
        ? <Spinner animation="border" />
        : hotels.map((hotel, index) => {
            return (
                <Link key={index} to={`/hotel/${hotel.id}`} className="genre-item">{hotel.name}</Link>
            );
        });

    return (
        <div>
            <Modal className="text-light" backdrop={true} show={isCreating} onHide={handleClose}>
                <Modal.Header className="bg-dark">
                    <Modal.Title>Create new hotel</Modal.Title>
                </Modal.Header>
                <Modal.Body className="bg-dark">
                    <form>
                        <div className="form-group mb-3">
                            <label htmlFor="name" className="form-label">Name</label>
                            <input type="text" className="form-control" name="name" id="name" onChange={e => setHotelName(e.target.value)} value={hotelName} />
                        </div>
                        <div className="form-group mb-3">
                            <label htmlFor="description" className="form-label">Location</label>
                            <textarea className="form-control" name="description" id="description" onChange={e => setHotelLocation(e.target.value)} value={hotelLocation}></textarea>
                        </div>
                    </form>
                </Modal.Body>
                <Modal.Footer className="bg-dark">
                    <Button variant="secondary" onClick={handleClose}>
                        Cancel
                    </Button>
                    <Button variant="primary" onClick={e => handleCreate()}>
                        Create
                    </Button>
                </Modal.Footer>
            </Modal>
            <div className="genre-list-page">
                <h1>Available hotels</h1>
                { (auth.user && auth.user.Role == Role.Owner) &&
                    <div className="create-button" onClick={handleShow}>
                        <PlusSquareFill size={32} />
                    </div>
                }
                <hr className="hr" />
                <div className="genre-list">
                    {body}
                </div>
            </div>
        </div>
    );
}

export default HotelList;