import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Container, Row, Col, Card, Image } from 'react-bootstrap';
import images from '../../assets/images';
import { Link } from 'react-router-dom';
import './SendReport.scss'

function SendReport() {
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
                <h1 className="text-black">Hello, how can we help you?</h1>
            </div>
            <Container className="my-5">
                <Row>
                    <Col md={2} className="mb-4">
                    </Col>
                    <Col md={4} className="mb-4">
                        <Card className='card-component'>
                            <Image src="https://downloads.intercomcdn.com/i/o/457901/55e97a8fcc8b0373d2483022/26d542c879397f4fc4c9f9e8ba22fb61.png"
                                className='card-icon'
                                loading="lazy" />
                            <div>
                                <Card.Title>Help for Students</Card.Title>
                                <Card.Text>
                                    56 articles
                                </Card.Text>
                            </div>
                        </Card>
                    </Col>
                    <Col md={4} className="mb-4">
                        <Card className='card-component'>
                            <Image src="https://downloads.intercomcdn.com/i/o/457902/1a628b8b9165bd001ea0d4f9/3bdd0969fba73e412585de510bbd40d9.png"
                                className='card-icon'
                                loading="lazy" />
                            <div>
                                <Card.Title>Help for Tutors</Card.Title>
                                <Card.Text>
                                    61 articles
                                </Card.Text>
                            </div>
                        </Card>
                    </Col>
                    <Col md={2} className="mb-4">
                    </Col>
                </Row>
                <Row>
                    <Col md={2} className="mb-4">
                    </Col>
                    <Col md={4} className="mb-4">
                        <Card className='card-component'>
                            <Image src="https://downloads.intercomcdn.com/i/o/457903/488d245eee98ca36210263f6/b34d4fcecddae648b10791af6d50e0d0.png"
                                className='card-icon'
                                loading="lazy" />
                            <div>
                                <Card.Title>General Information</Card.Title>
                                <Card.Text>
                                    6 articles
                                </Card.Text>
                            </div>
                        </Card>
                    </Col>
                    <Col md={4} className="mb-4">
                        <Card className='card-component'>
                            <Image src="https://downloads.intercomcdn.com/i/o/457908/fc68b97580964a0cb5c0952a/bf544901e19fd7166e8dea7088bc582b.png"
                                className='card-icon'
                                loading="lazy" />
                            <div>
                                <Card.Title>Preply Business</Card.Title>
                                <Card.Text>
                                    7 articles
                                </Card.Text>
                            </div>
                        </Card>
                    </Col>
                    <Col md={2} className="mb-4">
                    </Col>
                </Row>
            </Container>
        </div>
    );
}

export default SendReport
