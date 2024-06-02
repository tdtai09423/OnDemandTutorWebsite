import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Container } from 'react-bootstrap';
import images from '../../assets/images';
import { Link } from 'react-router-dom';
import './Policy.scss'

function Policy() {
    return (
        <div>
            <div bg="light" expand="lg" style={{ backgroundColor: '#ff7aac', paddingTop: '20px' }}>
                <Container>
                    <Link as={Link} to={"/"} >
                        <img src={images.logo} alt="OnDemandTutorLogo" />
                    </Link>
                </Container>
            </div>
            <div className="text-center py-5" style={{ backgroundColor: '#ff7aac' }}>
                <h1 className="text-black">Policy</h1>
            </div>

        </div>
    );
}

export default Policy
